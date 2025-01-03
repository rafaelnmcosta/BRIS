using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using bris_API.Data;
using Microsoft.AspNetCore.Authentication.JwtBearer;


namespace bris_API.Services
{
    public class TokenService : ITokenService
    {
        private readonly AppDbContext _dbContext;
        private readonly IConfiguration _configuration;

        public TokenService(IConfiguration configuration, AppDbContext dbContext)
        {
            _configuration = configuration;
            _dbContext = dbContext;
        }

        // Gera um token básico de login contendo apenas o ID do usuário
        public string GenerateTokenLogin(string userId, string userIp, string userAgent)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[]
                {
                    new Claim(ClaimTypes.NameIdentifier, userId),
                    new Claim("UserIP", userIp),
                    new Claim("UserAgent", userAgent),
                    new Claim("AcessoLogin", "true"),
                }),
                Expires = DateTime.UtcNow.AddMinutes(double.Parse(_configuration["Jwt:ExpiresInMinutes"])),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature),
                
                Issuer = _configuration["Jwt:Issuer"],
                Audience = _configuration["Jwt:Audience"]
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }

        // Gera um token com informações do vínculo selecionado pelo usuário
        public string GenerateTokenVinculo(string vinculoId, string userIp, string userAgent)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[]
                {
                    new Claim(ClaimTypes.NameIdentifier, vinculoId),
                    new Claim("UserIP", userIp),
                    new Claim("UserAgent", userAgent),
                }),
                Expires = DateTime.UtcNow.AddMinutes(double.Parse(_configuration["Jwt:ExpiresInMinutes"])),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature),
                
                Issuer = _configuration["Jwt:Issuer"],
                Audience = _configuration["Jwt:Audience"]
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }


        // Adiciona o token ao cookie http-only no contexto http
        public void SetCookieToken(HttpContext context, string token)
        {
            var isSecure = bool.Parse(_configuration["CookieSecure"]);
            context.Response.Cookies.Append("auth_token", token, new CookieOptions
            {
                HttpOnly = true,
                Secure = isSecure, // Usa https ou não dependendo do ambiente
                SameSite = SameSiteMode.Strict,
                Expires = DateTime.UtcNow.AddMinutes(double.Parse(_configuration["Jwt:ExpiresInMinutes"]))
            });
        }
        
        // Valida as informações do usuário contidas no context
        public async Task ValidaContext(TokenValidatedContext context)
        {
            Console.WriteLine($"\n\ntoken na ValidaContext:{context.SecurityToken}\n");

            var acessoLogin = context.Principal.FindFirst("AcessoLogin")?.Value;

            // Pula a validação se o usuário acabou de fazer login está escolhendo o vínculo ainda
            if (string.IsNullOrEmpty(acessoLogin)){

                // Valida se o Ip e o Agent da requisição são iguais aos presentes no token
                var currentIp = context.HttpContext.Connection.RemoteIpAddress?.ToString();
                var currentUserAgent = context.HttpContext.Request.Headers["User-Agent"].ToString();
                var tokenIpClaim = context.Principal.FindFirst("UserIP")?.Value;
                var tokenUserAgentClaim = context.Principal.FindFirst("UserAgent")?.Value;

                if (tokenIpClaim != currentIp || tokenUserAgentClaim != currentUserAgent)
                {
                    context.Fail("IP ou navegador não correspondentes com a geração do token.");
                }

                var vinculoIdClaim = context.Principal.FindFirst(ClaimTypes.NameIdentifier)?.Value;

                // Valida se o ID do vinculo é um valor numérico válido
                if (!int.TryParse(vinculoIdClaim, out int vinculoId)){
                    context.Fail("O Id contido no token não é um valor numérico válido!");
                }

                // Valida se o vínculo ainda existe no banco e se sim define sua Role no contexto da requisição
                try
                {
                    var vinculo = await _dbContext.Vinculos
                    .Include(v => v.Role)
                    .FirstOrDefaultAsync(v => v.Id == vinculoId);

                    if (vinculo == null)
                    {
                        Console.WriteLine($"Erro: Vínculo com ID {vinculoId} não encontrado.");
                        context.Fail("Id do vínculo presente no token não existe no banco de dados!");
                    }
                    else
                    {
                        var identity = context.Principal.Identity as ClaimsIdentity;
                        identity?.AddClaim(new Claim(ClaimTypes.Role, vinculo.Role.Nome));
                        Console.WriteLine($"\nRole do usuário adicionada ao contexto: {vinculo.Role.Nome}");
                    }

                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Erro ao validar vínculo no banco de dados: {ex.Message}");
                    context.Fail("Erro interno ao validar o vínculo.");
                }
            }


        }

        public async Task RenovaToken(TokenValidatedContext context)
        {
            var token = context.SecurityToken;
            var timeToExpire = token.ValidTo - DateTime.UtcNow;

            // Só roda se o token expirar em menos que o prazo configurado
            if (timeToExpire.TotalMinutes < double.Parse(_configuration["Jwt:RenewInMinutesLeft"]))
            {
                var userId = context.Principal.FindFirst(ClaimTypes.NameIdentifier).Value;
                var userIp = context.Principal.FindFirst("UserIP").Value;
                var userAgent = context.Principal.FindFirst("UserAgent").Value;
                var acessoLogin = context.Principal.FindFirst("AcessoLogin")?.Value;

                // gera o token correspondente ao que existia anteriormente
                string newToken = string.IsNullOrEmpty(acessoLogin) 
                    ? GenerateTokenVinculo(userId, userIp, userAgent) 
                    : GenerateTokenLogin(userId, userIp, userAgent);

                SetCookieToken(context.HttpContext, newToken);
                Console.WriteLine($"Token renovado com sucesso para o usuário: {userId}");
            }
        }
    }
}

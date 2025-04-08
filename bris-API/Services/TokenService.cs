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
            // Tratamento seguro para CookieSecure
            if (!bool.TryParse(_configuration["CookieSecure"], out var isSecure))
            {
                isSecure = true;
            }

            // Tratamento seguro para Expires
            if (!double.TryParse(_configuration["Jwt:ExpiresInMinutes"], out var expiresMinutes))
            {
                expiresMinutes = 60; // Valor padrão seguro
            }

            context.Response.Cookies.Append("auth_token", token, new CookieOptions
            {
                HttpOnly = true,
                Secure = isSecure,
                SameSite = SameSiteMode.Strict,
                Expires = DateTime.UtcNow.AddMinutes(expiresMinutes)
            });
        }

        // Valida as informações do usuário contidas no context
        public async Task ValidaContext(TokenValidatedContext context)
        {
            Console.WriteLine($"\n\ntoken na ValidaContext:{context.SecurityToken}\n");

            var acessoLogin = context.Principal.FindFirst("AcessoLogin")?.Value;

            // Pula a validação se o usuário acabou de fazer login está escolhendo o vínculo ainda
            if (string.IsNullOrEmpty(acessoLogin))
            {

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
                if (!int.TryParse(vinculoIdClaim, out int vinculoId))
                {
                    context.Fail("O Id contido no token não é um valor numérico válido!");
                }

                // Valida se o vínculo ainda existe no banco e se sim define sua Role no contexto da requisição
                try
                {
                    var vinculo = await _dbContext.Vinculos
                    .AsNoTracking()
                    .Include(v => v.Role)          // Carrega a Role
                    .Include(v => v.Granja)        // Carrega a Granja relacionada
                    .Include(v => v.Agroindustria) // Carrega a Agroindústria
                    .Include(v => v.Usuario)       // Carrega o Usuário
                    .FirstOrDefaultAsync(v => v.Id == vinculoId);

                    if (vinculo == null)
                    {
                        Console.WriteLine($"Erro: Vínculo com ID {vinculoId} não encontrado.");
                        context.Fail("Id do vínculo presente no token não existe no banco de dados!");
                    }
                    else
                    {
                        var identity = context.Principal.Identity as ClaimsIdentity;
                        identity?.AddClaim(new Claim(ClaimTypes.Role, vinculo.Role?.Nome ?? "N/A"));

                        // Granja - Nome e ID
                        identity?.AddClaim(new Claim("Granja", vinculo.Granja?.NomePropriedade ?? "N/A"));
                        identity?.AddClaim(new Claim("GranjaId", vinculo.Granja?.Id.ToString() ?? "0"));  // Usando ID numérico

                        // Agroindústria - Nome e ID
                        identity?.AddClaim(new Claim("Agroindustria", vinculo.Agroindustria?.NomeFantasia ?? "N/A"));
                        identity?.AddClaim(new Claim("AgroindustriaId", vinculo.Agroindustria?.Id.ToString() ?? "0"));  // Usando ID numérico

                        // Usuário
                        identity?.AddClaim(new Claim("UsuarioNome", vinculo.Usuario?.Nome ?? "N/A"));

                        Console.WriteLine("Claims adicionadas com sucesso!");
                    }

                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Erro ao validar vínculo no banco de dados: {ex.Message}");
                    context.Fail("Erro interno ao validar o vínculo.");
                }
            }


        }

        public void RenovaToken(TokenValidatedContext context)
        {

            Console.WriteLine($"\n\ntoken na RenovaToken:{context.SecurityToken}\n");

            var timeToExpire = context.SecurityToken.ValidTo - DateTime.UtcNow;

            // Configuração de renovação
            if (!double.TryParse(_configuration["Jwt:RenewInMinutesLeft"], out var renewThreshold))
            {
                renewThreshold = 5; // Valor padrão se a configuração falhar
            }

            if (timeToExpire.TotalMinutes < renewThreshold)
            {
                // Validação das claims obrigatórias
                var userId = context.Principal?.FindFirst(ClaimTypes.NameIdentifier)?.Value
                    ?? throw new SecurityTokenException("Claim 'NameIdentifier' ausente.");

                var userIp = context.Principal?.FindFirst("UserIP")?.Value
                    ?? throw new SecurityTokenException("Claim 'UserIP' ausente.");

                var userAgent = context.Principal?.FindFirst("UserAgent")?.Value
                    ?? throw new SecurityTokenException("Claim 'UserAgent' ausente.");

                // Geração do novo token
                var acessoLogin = context.Principal?.FindFirst("AcessoLogin")?.Value;
                string newToken = string.IsNullOrEmpty(acessoLogin)
                    ? GenerateTokenVinculo(userId, userIp, userAgent)
                    : GenerateTokenLogin(userId, userIp, userAgent);

                SetCookieToken(context.HttpContext, newToken);
                Console.WriteLine($"Token renovado para o usuário: {userId}");
            }
        }
    }
}

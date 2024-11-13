using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using bris_API.Data;


namespace bris_API.Services
{
    public class TokenService : ITokenService
    {
        private readonly AppDbContext _context;
        private readonly IConfiguration _configuration;

        public TokenService(IConfiguration configuration)
        {
            _configuration = configuration;
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
                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
                }),
                Expires = DateTime.UtcNow.AddMinutes(double.Parse(_configuration["Jwt:ExpiresInMinutes"])),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
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
                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
                }),
                Expires = DateTime.UtcNow.AddMinutes(double.Parse(_configuration["Jwt:ExpiresInMinutes"])),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }

        // Adiciona o token ao cookie http-only no contexto http
        public void SetCookieToken(HttpContext context, string token)
        {
            context.Response.Cookies.Append("auth_token", token, new CookieOptions
            {
                HttpOnly = true,
                Secure = true, // Caso esteja em produção com HTTPS
                SameSite = SameSiteMode.Strict,
                Expires = DateTime.UtcNow.AddMinutes(double.Parse(_configuration["Jwt:ExpiresInMinutes"]))
            });
        }
        
        // Valida as informações do usuário contidas no token
        public async Task<bool> ValidaUsuario(string token)
        {
            // Extrai informações do token
            var handler = new JwtSecurityTokenHandler();
            var jwtToken = handler.ReadToken(token) as JwtSecurityToken;

            if (jwtToken == null)
                return false;

            // Obtém os valores das claims no token
            var acessoLogin = jwtToken.Claims.FirstOrDefault(c => c.Type == "AcessoLogin")?.Value;
            var vinculoIdClaim = jwtToken.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;

            // Verifica se o token é de um usuário que acabou de fazer login (pula a validação)
            if (string.IsNullOrEmpty(acessoLogin))
                return true;

            // Valida se o ID do vinculo é um valor numérico válido
            if (!int.TryParse(vinculoIdClaim, out int vinculoId))
                return false;

            // Verifica se o vínculo ainda existe no banco
            var vinculo = await _context.Vinculos
                .FirstOrDefaultAsync(v => v.Id == vinculoId);

            if (vinculo == null)
            {
                return false;
            }

            return true;
        }
    }
}

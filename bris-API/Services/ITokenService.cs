using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;

namespace bris_API.Services
{
    public interface ITokenService
    {
        string GenerateToken(string UsuarioId, string UsuarioEmail, string TipoUsuario, string GranjaId);
    }

    public class TokenService : ITokenService
    {
        private readonly IConfiguration _configuration;

        public TokenService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public string GenerateToken(string UsuarioId, string UsuarioEmail, string TipoUsuario, string GranjaId)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]);
            var now = DateTime.UtcNow;

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(JwtRegisteredClaimNames.Sub, UsuarioId),
                    new Claim(JwtRegisteredClaimNames.Email, UsuarioEmail),
                    new Claim(ClaimTypes.Role, TipoUsuario),
                    new Claim("GranjaID", GranjaId) // Adicionando a informação da GranjaID
                }),
                Expires = now.AddMinutes(Convert.ToDouble(_configuration["Jwt:ExpiresInMinutes"])),
                NotBefore = now,
                Issuer = _configuration["Jwt:Issuer"],
                Audience = _configuration["Jwt:Audience"],
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }
    }
}

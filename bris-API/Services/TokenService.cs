using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Jose;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace bris_API.Services
{
    public class TokenService : ITokenService
    {
        private readonly IConfiguration _configuration;
        private readonly byte[] _encryptionKey;

        public TokenService(IConfiguration configuration)
        {
            _configuration = configuration;
            // Gerando a chave de encriptação para usar nas funções de encriptação e decriptação
            _encryptionKey = GenerateEncryptionArray(_configuration["Jwt:EncryptionKey"]);
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
                    new Claim(JwtRegisteredClaimNames.Sub, userId),
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


        // Gera um token com informações adicionais do vínculo selecionado pelo usuário
        public string GenerateTokenVinculo(string userId, string vinculoId, string role, string granjaId, string agroindustriaId, string userIp, string userAgent)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[]
                {
                    new Claim(JwtRegisteredClaimNames.Sub, userId),
                    new Claim("VinculoId", vinculoId),
                    new Claim(ClaimTypes.Role, role),
                    new Claim("GranjaId", granjaId),
                    new Claim("AgroindustriaId", agroindustriaId),
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

        // Converte a chave de encriptação de string para um array de bytes
        public byte[] GenerateEncryptionArray(string key)
        {
            return Encoding.UTF8.GetBytes(key);
        }

        // Encripta o token usando JWE com Jose.JWT
        public string EncryptToken(string token)
        {
            return JWT.Encode(token, _encryptionKey, JweAlgorithm.A256GCMKW, JweEncryption.A256GCM);
        }

        // Decripta o token encriptado para obter o JWT original
        public string DecryptToken(string encryptedToken)
        {
            return JWT.Decode(encryptedToken, _encryptionKey);
        }

        // decripta o token, valida, obtém os claims e retorna as claims apenas se o usuário for válido no sistema
        public ClaimsPrincipal ValidateToken(string token, string currentIp, string currentUserAgent)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]);

            var validationParameters = new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true,
                ValidIssuer = _configuration["Jwt:Issuer"],
                ValidAudience = _configuration["Jwt:Audience"],
                IssuerSigningKey = new SymmetricSecurityKey(key)
            };
            var principal = tokenHandler.ValidateToken(token, validationParameters, out _);

            // Verifica se o IP e o User-Agent do token coincidem com a requisição atual
            var ipClaim = principal.FindFirst("UserIP")?.Value;
            var agentClaim = principal.FindFirst("UserAgent")?.Value;

            if (ipClaim != currentIp || agentClaim != currentUserAgent)
            {
                throw new SecurityTokenException("IP ou navegador não correspondentes com a geração do token.");
            }

            return principal;
        }
    }
}

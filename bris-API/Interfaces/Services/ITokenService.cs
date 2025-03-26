using Microsoft.AspNetCore.Authentication.JwtBearer;

namespace bris_API.Services
{
    public interface ITokenService
    {
        string GenerateTokenLogin(string userId, string userIp, string userAgent);
        string GenerateTokenVinculo(string vinculoId, string userIp, string userAgent);
        void SetCookieToken(HttpContext context, string token);
        Task ValidaContext(TokenValidatedContext context);
        void RenovaToken(TokenValidatedContext context);
    }
}

using System.Security.Claims;

namespace bris_API.Services
{
    public interface ITokenService
    {
        string GenerateTokenLogin(string userId, string userIp, string userAgent);
        string GenerateTokenVinculo(string vinculoId, string userIp, string userAgent);
        void SetCookieToken(HttpContext context, string token);
    }
}

using System.Security.Claims;

namespace bris_API.Services
{
    public interface ITokenService
    {
        string GenerateTokenLogin(string userId, string userIp, string userAgent);
        string GenerateTokenVinculo(string userId, string role, string granjaId, string agroindustriaId, string userIp, string userAgent);
        byte[] GenerateEncryptionArray(string key);
        string EncryptToken(string token);
        string DecryptToken(string encryptedToken); 
        ClaimsPrincipal ValidateGetClaims(string token, string currentIp, string currentUserAgent);
    }
}

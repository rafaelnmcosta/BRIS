namespace bris_API.Services
{
    public interface IPasswordService
    {
        string GenerateSalt();
        string HashPassword(string password, string salt);
        bool VerifyPassword(string password, string salt, string hash);
        string GenerateRandomPassword(int length);
    }
}

using System.Security.Cryptography;
using System.Text;

namespace bris_API.Services
{
    public class PasswordService
    {
        public static string GenerateSalt()
        {
            var rng = new RNGCryptoServiceProvider();
            var saltBytes = new byte[16];
            rng.GetBytes(saltBytes);
            return Convert.ToBase64String(saltBytes);
        }

        public static string HashPassword(string password, string salt)
        {
            var sha256 = SHA256.Create();
            var saltedPassword = password + salt;
            var saltedPasswordBytes = Encoding.UTF8.GetBytes(saltedPassword);
            var hashBytes = sha256.ComputeHash(saltedPasswordBytes);
            return Convert.ToBase64String(hashBytes);
        }

        public static bool VerifyPassword(string password, string salt, string hash)
        {
            var hashOfInput = HashPassword(password, salt);
            return hashOfInput == hash;
        }
    }
}
using System.Security.Cryptography;
using System.Text;

namespace bris_API.Services
{
    public class PasswordService : IPasswordService
    {
        private static readonly char[] Characters = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ1234567890!@#$%^&*()".ToCharArray();

        public string GenerateSalt(){
            var rng = new RNGCryptoServiceProvider();
            var saltBytes = new byte[16];
            rng.GetBytes(saltBytes);
            return Convert.ToBase64String(saltBytes);
        }

        public string HashPassword(string password, string salt){
            var sha256 = SHA256.Create();
            var saltedPassword = password + salt;
            var saltedPasswordBytes = Encoding.UTF8.GetBytes(saltedPassword);
            var hashBytes = sha256.ComputeHash(saltedPasswordBytes);
            return Convert.ToBase64String(hashBytes);
        }

        public bool VerifyPassword(string password, string salt, string hash){
            var hashOfInput = HashPassword(password, salt);
            return hashOfInput == hash;
        }

        public string GenerateRandomPassword(int length){
            var password = new StringBuilder();
            using (var rng = RandomNumberGenerator.Create())
            {
                var randomBytes = new byte[length];
                rng.GetBytes(randomBytes);

                for (int i = 0; i < length; i++)
                {
                    var randomIndex = randomBytes[i] % Characters.Length;
                    password.Append(Characters[randomIndex]);
                }
            }

            return password.ToString();
        }
    }
}

using System.Security.Cryptography;
using System.Text;

namespace PMSCRM.Utilities
{
    public class PasswordSecurity
    {
        public static readonly string AppSalt = "bUp/bLcb+mYu8GD1JHSQ14ej4BNyweH6Cx6sthXD4Nc=";

        public static string HashPassword(string password)
        {
            using (var sha256 = SHA256.Create())
            {
                var saltedPassword = Encoding.UTF8.GetBytes(password + AppSalt);
                var hash = sha256.ComputeHash(saltedPassword);
                return Convert.ToBase64String(hash);
            }
        }

        public static bool VerifyPassword(string enteredPassword, string storedHash)
        {
            var hash = HashPassword(enteredPassword);
            return hash == storedHash;
        }
    }
}

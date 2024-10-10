using Newtonsoft.Json;
using PMSCRM.Models;
using System.Security.Cryptography;
using System.Text;

namespace PMSCRM.Utilities
{
    public class PasswordSecurity
    {
        private readonly PmscrmContext _db;

        public PasswordSecurity(PmscrmContext db)
        {
            _db = db;
        }

        public string GenerateTemporaryPassword()
        {
            return Guid.NewGuid().ToString().Substring(0, 8);
        }

        public string HashPassword(string password)
        {
            string salt = GetSalt();

            using (var sha256 = SHA256.Create())
            {
                var saltedPassword = Encoding.UTF8.GetBytes(password + salt);
                var hash = sha256.ComputeHash(saltedPassword);
                return Convert.ToBase64String(hash);
            }
        }

        public string GetSalt()
        {
            var application = _db.Applications.FirstOrDefault();

            if (application != null)
            {
                var salt = JsonConvert.DeserializeObject<Dictionary<string, string>>(application.Data);
                return salt["Salt"];
            }
            throw new Exception("Salt not found in the database.");
        }

        public bool VerifyPassword(string enteredPassword, string storedHash)
        {
            var hash = HashPassword(enteredPassword);
            return hash == storedHash;
        }
    }
}

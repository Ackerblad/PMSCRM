using Microsoft.AspNetCore.Mvc;
using PMSCRM.Models;
using PMSCRM.Utilities;

namespace PMSCRM.Services
{
    public class UserService
    {
        private readonly PmscrmContext _db;
        private readonly EmailService _emailService;

        public UserService(PmscrmContext db, EmailService emailService)
        {
            _db = db;
            _emailService = emailService;
        }

        public List<User> GetUsers()
        {
            
            return _db.Users.ToList();
        }

        public bool AddUser(Guid companyId, Guid roleId, string username, string firstName, string lastName, string phoneNumber, string emailAddress, string plainPassword)
        {
            bool userExists = _db.Users.Any(u => u.Username == username);

            if (userExists)
            {
                return false;
            }

            var user = new User
            {
                CompanyId = companyId,
                RoleId = roleId,
                Username = username,
                FirstName = firstName,
                LastName = lastName,
                PhoneNumber = phoneNumber,
                EmailAddress = emailAddress
            };

            var salt = PasswordSecurity.GenerateSalt();
            var hashedPassword = PasswordSecurity.HashPassword(plainPassword, salt);

            user.PasswordHash = hashedPassword;
            user.PasswordSalt = salt;

            _db.Users.Add(user);
            _db.SaveChanges();
            return true;
        }

        //public bool UpdateUser(Guid userId, UserUpdate updatedUser)
        //{
        //    var existingUser = _db.Users.FirstOrDefault(u => u.UserId == userId);

        //    if (existingUser == null)
        //    {
        //        return false;
        //    }

        //    if (updatedUser.CompanyId == Guid.Empty || updatedUser.RoleId == Guid.Empty)
        //    {
        //        return false;
        //    }

        //    existingUser.Username = updatedUser.Username;
        //    existingUser.FirstName = updatedUser.FirstName;
        //    existingUser.LastName = updatedUser.LastName;
        //    existingUser.PhoneNumber = updatedUser.PhoneNumber;
        //    existingUser.EmailAddress = updatedUser.EmailAddress;
        //    existingUser.CompanyId = updatedUser.CompanyId;
        //    existingUser.RoleId = updatedUser.RoleId;

        //    _db.SaveChanges();
        //    return true;
        //}

        public bool DeleteUser(Guid userId)
        {
            var userToDelete = _db.Users.Find(userId);

            if (userToDelete == null)
            {
                return false;
            }
            _db.Users.Remove(userToDelete);
            _db.SaveChanges();
            return true;
        }

        public User AuthenticateUser(string username, string plainPassword)
        {
            var user = _db.Users.FirstOrDefault(u => u.Username == username);

            if (user != null && PasswordSecurity.VerifyPassword(plainPassword, user.PasswordHash, user.PasswordSalt))
            {
                return user;
            }
            return null;
        }

        public bool GeneratePasswordToken(string emailAddress)
        {
            var user = _db.Users.FirstOrDefault(u => u.EmailAddress == emailAddress);

            if (user == null)
            {
                return false;
            }

            var token = Guid.NewGuid().ToString();

            user.PasswordToken = token;
            user.TokenExpiry = DateTime.UtcNow.AddHours(24);
            _db.SaveChanges();

            var resetLink = $"https://pmscrm.com/reset-password?token={token}";
            var message = $"Use the following link to reset your password: {resetLink}";

            _emailService.SendEmail(user.EmailAddress, "Password Reset", message);
            return true;
        }

        public bool ResetPassword(string token, string newPassword)
        {
            var user = _db.Users.FirstOrDefault(u => u.PasswordToken == token && u.TokenExpiry > DateTime.UtcNow);

            if (user == null)
            {
                return false; 
            }

            var salt = PasswordSecurity.GenerateSalt();
            var hashedPassword = PasswordSecurity.HashPassword(newPassword, salt);

            user.PasswordHash = hashedPassword;
            user.PasswordSalt = salt;
            user.PasswordToken = null;
            user.TokenExpiry = null;

            _db.SaveChanges();

            return true;
        }
    }
}

using Microsoft.AspNetCore.Mvc;
using PMSCRM.Models;
using PMSCRM.Utilities;

namespace PMSCRM.Services
{
    public class UserService
    {
        private readonly PmscrmContext _db;

        public UserService(PmscrmContext db)
        {
            _db = db;
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

        //public bool UpdateUser(Guid userId, User updatedUser)
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
        //    existingUser.Password = updatedUser.Password;
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
    }
}

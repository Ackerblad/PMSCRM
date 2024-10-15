using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PMSCRM.Models;
using PMSCRM.Utilities;
using System.ComponentModel.Design;

namespace PMSCRM.Services
{
    public class UserService
    {
        private readonly PmscrmContext _db;
        private readonly EmailService _emailService;
        private readonly PasswordSecurity _passwordSecurity;

        public UserService(PmscrmContext db, EmailService emailService, PasswordSecurity passwordSecurity)
        {
            _db = db;
            _emailService = emailService;
            _passwordSecurity = passwordSecurity;
        }

        public async Task<List<User>> GetUsers(Guid companyId)
        {
            return await _db.Users.Where(u => u.CompanyId == companyId).ToListAsync();
        }

        public async Task<User> GetById(Guid id, Guid companyId)
        {
            return await _db.Users.FirstOrDefaultAsync(u => u.UserId == id && u.CompanyId == companyId);
        }

        public async Task<bool> AddUser(Guid companyId, Guid roleId, string emailAddress, string firstName, string lastName, string phoneNumber, string plainPassword)
        {
            bool userExists = await _db.Users.AnyAsync(u => u.EmailAddress == emailAddress && u.CompanyId == companyId);

            if (userExists)
            {
                return false;
            }

            var user = new User
            {
                CompanyId = companyId,
                RoleId = roleId,
                EmailAddress = emailAddress,
                FirstName = firstName,
                LastName = lastName,
                PhoneNumber = phoneNumber,
                Password = _passwordSecurity.HashPassword(plainPassword)
            };

            await _db.Users.AddAsync(user);
            await _db.SaveChangesAsync();
            return true;
        }

        public async Task<bool> UpdateUser(Guid userId, UserUpdate updatedUser, Guid companyId)
        {
            var existingUser = await _db.Users.FirstOrDefaultAsync(u => u.UserId == userId && u.CompanyId == companyId);

            if (existingUser == null)
            {
                return false;
            }

            existingUser.EmailAddress = updatedUser.EmailAddress;
            existingUser.FirstName = updatedUser.FirstName;
            existingUser.LastName = updatedUser.LastName;
            existingUser.PhoneNumber = updatedUser.PhoneNumber;
            existingUser.RoleId = updatedUser.RoleId;

            await _db.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeleteUser(Guid userId, Guid companyId)
        {
            var userToDelete = await _db.Users.FirstOrDefaultAsync(u => u.UserId == userId && u.CompanyId == companyId);

            if (userToDelete == null)
            {
                return false;
            }

            _db.Users.Remove(userToDelete);
            await _db.SaveChangesAsync();
            return true;
        }

        public async Task<User> AuthenticateUser(string emailAddress, string plainPassword)
        {
            var user = await _db.Users.FirstOrDefaultAsync(u => u.EmailAddress == emailAddress);

            if (user != null && _passwordSecurity.VerifyPassword(plainPassword, user.Password))
            {
                return user;
            }
            return null;
        }

        public string GenerateTemporaryPassword()
        {
            return _passwordSecurity.GenerateTemporaryPassword();
        }

        public async Task<bool> GeneratePasswordToken(string emailAddress)
        {
            var user = await _db.Users.FirstOrDefaultAsync(u => u.EmailAddress == emailAddress);

            if (user == null)
            {
                return false;
            }

            user.ResetToken = Guid.NewGuid();
            user.ResetTokenExpiryDate = DateTime.UtcNow.AddHours(24);
            
            await _db.SaveChangesAsync();

            var resetLink = $"https://localhost:7027/User/reset-password?token={user.ResetToken}";
            var message = $"Use the following link to reset your password: {resetLink}";

            await _emailService.SendEmail(user.EmailAddress, "Password Reset", message);
            return true;
        }

        public async Task<bool> ResetPassword(Guid token, string newPassword)
        {
            var user = await _db.Users.FirstOrDefaultAsync(u => u.ResetToken == token && u.ResetTokenExpiryDate > DateTime.UtcNow);

            if (user == null)
            {
                return false; 
            }

            user.Password = _passwordSecurity.HashPassword(newPassword); ;
            user.ResetToken = Guid.Empty;
            user.ResetTokenExpiryDate = DateTime.UtcNow.AddHours(-24);

            await _db.SaveChangesAsync();
            return true;
        }
    }
}

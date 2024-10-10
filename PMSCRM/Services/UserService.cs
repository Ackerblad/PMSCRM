﻿using Microsoft.AspNetCore.Mvc;
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

        public List<User> GetUsers(Guid companyId)
        {

            return _db.Users.Where(u => u.CompanyId == companyId).ToList();
        }

        public bool AddUser(Guid companyId, Guid roleId, string emailAddress, string firstName, string lastName, string phoneNumber, string plainPassword)
        {
            bool userExists = _db.Users.Any(u => u.EmailAddress == emailAddress && u.CompanyId == companyId);

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

            _db.Users.Add(user);
            _db.SaveChanges();
            return true;
        }

        public bool UpdateUser(Guid userId, UserUpdate updatedUser, Guid companyId)
        {
            var existingUser = _db.Users.FirstOrDefault(u => u.UserId == userId && u.CompanyId == companyId);

            if (existingUser == null)
            {
                return false;
            }

            existingUser.EmailAddress = updatedUser.EmailAddress;
            existingUser.FirstName = updatedUser.FirstName;
            existingUser.LastName = updatedUser.LastName;
            existingUser.PhoneNumber = updatedUser.PhoneNumber;
            existingUser.CompanyId = updatedUser.CompanyId;
            existingUser.RoleId = updatedUser.RoleId;

            _db.SaveChanges();
            return true;
        }

        public bool DeleteUser(Guid userId, Guid companyId)
        {
            var userToDelete = _db.Users.FirstOrDefault(u => u.UserId == userId && u.CompanyId == companyId);

            if (userToDelete == null)
            {
                return false;
            }
            _db.Users.Remove(userToDelete);
            _db.SaveChanges();
            return true;
        }

        public User AuthenticateUser(string emailAddress, string plainPassword)
        {
            var user = _db.Users.FirstOrDefault(u => u.EmailAddress == emailAddress);

            if (user != null && _passwordSecurity.VerifyPassword(plainPassword, user.Password))
            {
                return user;
            }
            return null;
        }

        public string GenerateTemporaryPassword()
        {
            return Guid.NewGuid().ToString().Substring(0, 8);
        }

        public bool GeneratePasswordToken(string emailAddress)
        {
            var user = _db.Users.FirstOrDefault(u => u.EmailAddress == emailAddress);

            if (user == null)
            {
                return false;
            }

            user.ResetToken = Guid.NewGuid();
            user.ResetTokenExpiryDate = DateTime.UtcNow.AddHours(24);
            _db.SaveChanges();

            var resetLink = $"https://pmscrm.com/reset-password?token={user.ResetToken}";
            var message = $"Use the following link to reset your password: {resetLink}";

            _emailService.SendEmail(user.EmailAddress, "Password Reset", message);
            return true;
        }

        public bool ResetPassword(Guid token, string newPassword)
        {
            var user = _db.Users.FirstOrDefault(u => u.ResetToken == token && u.ResetTokenExpiryDate > DateTime.UtcNow);

            if (user == null)
            {
                return false; 
            }

            var hashedPassword = _passwordSecurity.HashPassword(newPassword);

            user.Password = hashedPassword;
            user.ResetToken = Guid.Empty;
            user.ResetTokenExpiryDate = DateTime.UtcNow.AddHours(-24);

            _db.SaveChanges();
            return true;
        }
    }
}

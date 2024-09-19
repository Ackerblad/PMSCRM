using Microsoft.AspNetCore.Mvc;
using PMSCRM.Models;
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

        // Lägg även till så att en användare inte kan ha en existerande email? 
        public bool AddUser(User user) 
        {
            bool userExists = _db.Users.Contains(user);

            if (user.Username == "" || userExists) 
            {
                return false;
            }
            _db.Users.Add(user);
            _db.SaveChanges();
            return true;
        }

        public bool UpdateUser(Guid userId, User updatedUser)
        {
            var existingUser = _db.Users.FirstOrDefault(u => u.UserId == userId);

            if (existingUser == null)
            {
                return false; 
            } 

            if (updatedUser.CompanyId == Guid.Empty || updatedUser.RoleId == Guid.Empty)
            {
                return false;
            }

            existingUser.Username = updatedUser.Username;
            existingUser.Password = updatedUser.Password;
            existingUser.FirstName = updatedUser.FirstName;
            existingUser.LastName = updatedUser.LastName;
            existingUser.PhoneNumber = updatedUser.PhoneNumber;
            existingUser.EmailAddress = updatedUser.EmailAddress;

            existingUser.CompanyId = updatedUser.CompanyId;
            existingUser.RoleId = updatedUser.RoleId;

            _db.SaveChanges();
            return true;
        }

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





    }
}

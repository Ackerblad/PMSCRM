using PMSCRM.Models;

namespace PMSCRM.Services
{
    public class RoleService
    {
        private readonly PmscrmContext _db;

        public RoleService(PmscrmContext db)
        {
            _db = db;
        }

        public List<Role> GetRoles()
        {
            return _db.Roles.ToList();
        }

        public bool AddRole(Role role)
        {
            bool roleExists = _db.Roles.Contains(role);

            if (roleExists)
            {
                return false;
            }

            _db.Roles.Add(role);
            _db.SaveChanges();
            return true;
        }

        public bool UpdateRole(Guid roleId, Role updatedRole)
        {
            var existingRole = _db.Roles.FirstOrDefault(r => r.RoleId == roleId);

            if (existingRole == null)
            {
                return false;
            }

            existingRole.CompanyId = updatedRole.CompanyId;
            existingRole.Name = updatedRole.Name;


            _db.SaveChanges();
            return true;
        }

        public bool DeleteRole(Guid roleId)
        {
            var roleToDelete = _db.Roles.Find(roleId);

            if (roleToDelete == null)
            {
                return false;
            } 

            _db.Roles.Remove(roleToDelete);
            _db.SaveChanges();
            return true;
        }
    }
}

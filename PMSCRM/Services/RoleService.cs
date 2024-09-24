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

        public List<Role> GetAll()
        {
            return _db.Roles.ToList();
        }

        public bool Add(Role role)
        {
            bool exists = _db.Roles.Contains(role);

            if (exists)
            {
                return false;
            }

            _db.Roles.Add(role);
            _db.SaveChanges();
            return true;
        }

        public bool Update(Guid guid, Role updated)
        {
            var existing = _db.Roles.FirstOrDefault(r => r.RoleId == guid);

            if (existing == null)
            {
                return false;
            }

            existing.CompanyId = updated.CompanyId;
            existing.Name = updated.Name;

            _db.SaveChanges();
            return true;
        }

        public bool Delete(Guid guid)
        {
            var toDelete = _db.Roles.Find(guid);

            if (toDelete == null)
            {
                return false;
            } 

            _db.Roles.Remove(toDelete);
            _db.SaveChanges();
            return true;
        }
    }
}

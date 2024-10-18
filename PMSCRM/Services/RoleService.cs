using Microsoft.EntityFrameworkCore;
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

        public async Task<List<Role>> GetAllAsync(Guid companyId)
        {
            return await _db.Roles
                .Where(r => r.CompanyId == companyId)
                .ToListAsync();
        }
        public async Task<Role?> GetByIdAsync(Guid id, Guid companyId)
        {
            return await _db.Roles
                .FirstOrDefaultAsync(r => r.RoleId == id && r.CompanyId == companyId);
        }

        public async Task<bool> AddAsync(Role role)
        {
            bool exists = await _db.Roles
                .AnyAsync(r => r.Name == role.Name && r.CompanyId == role.CompanyId);

            if (exists)
            {
                return false;
            }

            await _db.Roles.AddAsync(role);
            await _db.SaveChangesAsync();
            return true;
        }

        public async Task<bool> UpdateAsync(Guid id, Role updatedRole)
        {
            var existing = await _db.Roles
                .FirstOrDefaultAsync(r => r.RoleId == id && r.CompanyId == updatedRole.CompanyId);

            if (existing == null)
            {
                return false;
            }

            existing.Name = updatedRole.Name;

            await _db.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeleteAsync(Guid id, Guid companyId)
        {
            var toDelete = await _db.Roles
                .FirstOrDefaultAsync(r => r.RoleId == id && r.CompanyId == companyId);

            if (toDelete == null)
            {
                return false;
            }

            _db.Roles.Remove(toDelete);
            await _db.SaveChangesAsync();
            return true;
        }
    }
}

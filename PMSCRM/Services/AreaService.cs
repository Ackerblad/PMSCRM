using PMSCRM.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.Design;

namespace PMSCRM.Services
{
    public class AreaService
    {
        private readonly PmscrmContext _db;

        public AreaService(PmscrmContext db)
        {
            _db = db;
        }

        // Get all areas asynchronously
        public async Task<List<Area>> GetAllAsync(Guid companyId)
        {
            return await _db.Areas.Where(a => a.CompanyId == companyId).ToListAsync();
        }

        // Get an area by Id asynchronously
        public async Task<Area?> GetByIdAsync(Guid id, Guid companyId)
        {
            return await _db.Areas.FirstOrDefaultAsync(a => a.AreaId == id && a.CompanyId == companyId);
        }

        public async Task<bool> AddAsync(Area area)
        {
            // Check if the area already exists (asynchronously)
            bool exists = await _db.Areas.AnyAsync(a => a.AreaId == area.AreaId && a.CompanyId == area.CompanyId);

            if (exists)
            {
                return false;
            }

            await _db.Areas.AddAsync(area);  // Use async version of Add
            await _db.SaveChangesAsync();    // Use async version of SaveChanges
            return true;
        }

        // Update an area asynchronously
        public async Task<bool> UpdateAsync(Guid id, Area updated)
        {
            var area = await _db.Areas.FirstOrDefaultAsync(a => a.AreaId == id && a.CompanyId == updated.CompanyId);  // Find the area asynchronously

            if (area == null)
            {
                return false;
            }

            // Update fields
            
            area.Name = updated.Name;
            area.Description = updated.Description;

            await _db.SaveChangesAsync();   // Save changes asynchronously
            return true;
        }

        // Delete an area asynchronously
        public async Task<bool> DeleteAsync(Guid id, Guid companyId)
        {
            var toDelete = await  _db.Areas.FirstOrDefaultAsync(a => a.AreaId == id && a.CompanyId == companyId);  // Find the area asynchronously

            if (toDelete == null)
            {
                return false;
            }

            _db.Areas.Remove(toDelete);
            await _db.SaveChangesAsync();  // Use async version of SaveChanges
            return true;
        }
    }
}

using PMSCRM.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

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
        public async Task<List<Area>> GetAllAsync()
        {
            return await _db.Areas.ToListAsync();
        }

        // Get an area by Id asynchronously
        public async Task<Area?> GetByIdAsync(Guid id)
        {
            return await _db.Areas.FindAsync(id);
        }

        public async Task<bool> AddAsync(Area area)
        {
            // Check if the area already exists (asynchronously)
            bool exists = await _db.Areas.AnyAsync(a => a.AreaId == area.AreaId);

            if (exists)
            {
                return false;
            }

            await _db.Areas.AddAsync(area);  // Use async version of Add
            await _db.SaveChangesAsync();    // Use async version of SaveChanges
            return true;
        }

        // Update an area asynchronously
        public async Task<bool> UpdateAsync(Guid guid, Area updated)
        {
            var existing = await _db.Areas.FirstOrDefaultAsync(a => a.AreaId == guid);  // Find the area asynchronously

            if (existing == null)
            {
                return false;
            }

            // Update fields
            
            existing.Name = updated.Name;
            existing.Description = updated.Description;

            await _db.SaveChangesAsync();   // Save changes asynchronously
            return true;
        }

        // Delete an area asynchronously
        public async Task<bool> DeleteAsync(Guid guid)
        {
            var toDelete = await _db.Areas.FindAsync(guid);  // Find the area asynchronously

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

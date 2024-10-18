﻿using Microsoft.EntityFrameworkCore;
using PMSCRM.Models;

namespace PMSCRM.Services
{
    public class AreaService
    {
        private readonly PmscrmContext _db;

        public AreaService(PmscrmContext db)
        {
            _db = db;
        }

        public async Task<List<Area>> GetAllAsync(Guid companyId)
        {
            return await _db.Areas.Where(a => a.CompanyId == companyId).ToListAsync();
        }

        public async Task<Area?> GetByIdAsync(Guid id, Guid companyId)
        {
            return await _db.Areas.FirstOrDefaultAsync(a => a.AreaId == id && a.CompanyId == companyId);
        }

        public async Task<bool> AddAsync(Area area)
        {
            bool exists = await _db.Areas.AnyAsync(a => a.AreaId == area.AreaId && a.CompanyId == area.CompanyId);

            if (exists)
            {
                return false;
            }

            await _db.Areas.AddAsync(area);
            await _db.SaveChangesAsync();
            return true;
        }

        public async Task<bool> UpdateAsync(Guid id, Area updated)
        {
            var area = await _db.Areas.FirstOrDefaultAsync(a => a.AreaId == id && a.CompanyId == updated.CompanyId);

            if (area == null)
            {
                return false;
            }

            area.Name = updated.Name;
            area.Description = updated.Description;

            await _db.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeleteAsync(Guid id, Guid companyId)
        {
            var toDelete = await _db.Areas.FirstOrDefaultAsync(a => a.AreaId == id && a.CompanyId == companyId);

            if (toDelete == null)
            {
                return false;
            }

            _db.Areas.Remove(toDelete);
            await _db.SaveChangesAsync();
            return true;
        }
    }
}

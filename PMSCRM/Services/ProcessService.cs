using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PMSCRM.Models;

namespace PMSCRM.Services
{
    public class ProcessService : Controller
    {
        private readonly PmscrmContext _db;

        public ProcessService(PmscrmContext db)
        {
            _db = db;
        }

        public async Task<List<Process>> GetAllAsync(Guid companyId)
        {
            return await _db.Processes
                .Where(p => p.CompanyId == companyId)
                .ToListAsync();
        }

        public async Task<Process?> GetByIdAsync(Guid id, Guid companyId)
        {
            return await _db.Processes
                 .FirstOrDefaultAsync(p => p.ProcessId == id && p.CompanyId == companyId);
        }

        public async Task<bool> AddAsync(Process process)
        {
            bool exist = await _db.Customers
                .AnyAsync(p => p.Name == process.Name && p.CompanyId == process.CompanyId);

            if (exist)
            {
                return false;
            }

            await _db.Processes.AddAsync(process);
            await _db.SaveChangesAsync();
            return true;
        }

        public async Task<bool> UpdateAsync(Guid id, Process updatedProcess)
        {
            var existing = await _db.Processes
                .FirstOrDefaultAsync(p => p.ProcessId == id && p.CompanyId == updatedProcess.CompanyId);

            if (existing == null)
            {
                return false;
            }

            existing.Name = updatedProcess.Name;
            existing.Description = updatedProcess.Description;
            existing.Duration = updatedProcess.Duration;

            await _db.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeleteAsync(Guid id, Guid companyId)
        {
            var toDelete = await _db.Processes
                .FirstOrDefaultAsync(p => p.ProcessId == id && p.CompanyId == companyId);

            if (toDelete == null)
            {
                return false;
            }
            _db.Processes.Remove(toDelete);
            await _db.SaveChangesAsync();
            return true;
        }
    }
}

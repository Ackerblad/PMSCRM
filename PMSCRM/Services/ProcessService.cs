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
            var processes = await _db.Processes
                .Include(p => p.Area)  
                .Where(p => p.CompanyId == companyId)
                .ToListAsync();

            foreach (var process in processes)
            {
                Console.WriteLine($"Process ID: {process.ProcessId}, Area ID: {process.AreaId}, Area Name: {process.Area?.Name}");
            }

            return processes;
        }

        public async Task<Process?> GetByIdAsync(Guid id, Guid companyId)
        {
            return await _db.Processes
                .Include(p => p.Area)  
                .FirstOrDefaultAsync(p => p.ProcessId == id && p.CompanyId == companyId);
        }

        public async Task<bool> AddAsync(Process process)
        {
            bool exist = await _db.Processes
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

            var taskProcessAreasToUpdate = _db.TaskProcessAreas
            .Where(tpa => tpa.ProcessId == id);

            foreach (var tpa in taskProcessAreasToUpdate)
            {
                tpa.AreaId = updatedProcess.AreaId;
            }

            if (existing == null)
            {
                return false;
            }

            existing.Name = updatedProcess.Name;
            existing.Description = updatedProcess.Description;
            existing.Duration = updatedProcess.Duration;
            existing.AreaId = updatedProcess.AreaId;

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

        public async Task<List<Process>> SearchProcessesAsync(Guid companyId, string query)
        {
            query = query.Trim();

            if (string.IsNullOrEmpty(query))
            {
                return await _db.Processes
                    .Where(p => p.CompanyId == companyId)
                    .ToListAsync();
            }

            return await _db.Processes
                .Where(t => t.CompanyId == companyId &&
                           (t.Name.ToLower().Contains(query.ToLower())))
                .ToListAsync();
        }
    }
}

using Microsoft.EntityFrameworkCore;
using PMSCRM.Models;

namespace PMSCRM.Services
{
    public class TaskService
    {
        private readonly PmscrmContext _db;

        public TaskService(PmscrmContext db)
        {
            _db = db;
        }

        public async Task<List<Models.Task>> GetAllAsync(Guid companyId)
        {
            return await _db.Tasks.
                Where(t => t.CompanyId == companyId)
                .ToListAsync();
        }

        public async Task<Models.Task?> GetByIdAsync(Guid id, Guid companyId)
        {
            return await _db.Tasks
                .FirstOrDefaultAsync(t => t.TaskId == id && t.CompanyId == companyId);
        }

        public async Task<bool> AddAsync(Models.Task task)
        {
            bool exists = await _db.Tasks
                .AnyAsync(t => t.Name == task.Name && t.CompanyId == task.CompanyId);

            if (exists)
            {
                return false;
            }

            await _db.Tasks.AddAsync(task);
            await _db.SaveChangesAsync();
            return true;
        }

        public async Task<bool> UpdateAsync(Guid id, Models.Task updated)
        {
            var task = await _db.Tasks
                .FirstOrDefaultAsync(t => t.TaskId == id && t.CompanyId == updated.CompanyId);

            if (task == null)
            {
                return false;
            }

            task.Name = updated.Name;
            task.Description = updated.Description;
            task.Duration = updated.Duration;

            await _db.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeleteAsync(Guid id, Guid companyId)
        {
            var toDelete = await _db.Tasks
                .FirstOrDefaultAsync(t => t.TaskId == id && t.CompanyId == companyId);

            if (toDelete == null)
            {
                return false;
            }

            _db.Tasks.Remove(toDelete);
            await _db.SaveChangesAsync();
            return true;
        }

        public async Task<List<Models.Task>> SearchTasksAsync(Guid companyId, string query)
        {
            query = query.Trim();

            if (string.IsNullOrEmpty(query))
            {
                return await _db.Tasks
                    .Where(t => t.CompanyId == companyId)
                    .ToListAsync();
            }

            return await _db.Tasks
                .Where(t => t.CompanyId == companyId &&
                           (t.Name.ToLower().Contains(query.ToLower())))
                .ToListAsync();
        }
    }
}

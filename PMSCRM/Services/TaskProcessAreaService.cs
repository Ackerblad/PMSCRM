
using Microsoft.EntityFrameworkCore;
using PMSCRM.Models;


namespace PMSCRM.Services
{
    public class TaskProcessAreaService
    {
        private readonly PmscrmContext _db;

        public TaskProcessAreaService(PmscrmContext db)
        {
            _db = db;
        }

        public List<TaskProcessArea> GetAllWithDetails()
        {
            return _db.TaskProcessAreas
                      .Include(tpa => tpa.Task)
                      .Include(tpa => tpa.Process)
                      //.Include(tpa => tpa.Area)
                      .ToList();
        }

        public List<TaskProcessArea> GetAll()
        {
            return _db.TaskProcessAreas.ToList();
        }

        public async Task<bool> AddAsync(TaskProcessArea taskProcessArea)
        {
            bool exists = await _db.TaskProcessAreas
                .AnyAsync(t => t.TaskId == taskProcessArea.TaskId &&
                          t.ProcessId == taskProcessArea.ProcessId &&
                          //t.AreaId == taskProcessArea.AreaId &&
                          t.CompanyId == taskProcessArea.CompanyId);

            if (exists)
            {
                return false;
            }

            await _db.TaskProcessAreas.AddAsync(taskProcessArea);
            await _db.SaveChangesAsync();
            return true;
        }

        public async Task<bool> UpdateAsync(TaskProcessArea taskProcessArea)
        {
            var existing = await _db.TaskProcessAreas.FindAsync(taskProcessArea.TaskProcessAreaId);
            if (existing == null)
            {
                return false;
            }

            existing.TaskId = taskProcessArea.TaskId;
            existing.ProcessId = taskProcessArea.ProcessId;
            //existing.AreaId = taskProcessArea.AreaId;

            await _db.SaveChangesAsync();
            return true;
        }

        public async Task<TaskProcessArea?> GetByIdAsync(Guid id, Guid companyId)
        {
            return await _db.TaskProcessAreas
                .Include(t => t.Task)
                .Include(t => t.Process)
                //.Include(t => t.Area)
                .FirstOrDefaultAsync(tpa => tpa.TaskProcessAreaId == id && tpa.CompanyId == companyId);
        }

        public async Task<bool> DeleteAsync(Guid id, Guid companyId)
        {
            var taskProcessArea = await _db.TaskProcessAreas.FindAsync(id);
            if (taskProcessArea == null || taskProcessArea.CompanyId != companyId)
            {
                return false;
            }

            _db.TaskProcessAreas.Remove(taskProcessArea);
            await _db.SaveChangesAsync();
            return true;
        }

    }
}

using Microsoft.EntityFrameworkCore;
using PMSCRM.Models;
using PMSCRM.ViewModels;

namespace PMSCRM.Services
{
    public class TaskProcessAreaUserCustomerService
    {
        private readonly PmscrmContext _db;

        public TaskProcessAreaUserCustomerService(PmscrmContext db)
        {
            _db = db;
        }

        public async Task<IEnumerable<TaskProcessAreaUserCustomer>> GetTasksForUser(Guid userId)
        {
            return await _db.TaskProcessAreaUserCustomers
                .Include(tpauc => tpauc.TaskProcessArea)
                    .ThenInclude(tpa => tpa.Task)
                .Include(tpauc => tpauc.TaskProcessArea)
                    .ThenInclude(tpa => tpa.Process)
                .Include(tpauc => tpauc.TaskProcessArea)
                    .ThenInclude(tpa => tpa.Area)
                .Include(tpauc => tpauc.Customer)
                .Where(tpauc => tpauc.UserId == userId)
                .ToListAsync();
        }

        public List<TaskProcessAreaUserCustomer> GetAll()
        {
            return _db.TaskProcessAreaUserCustomers.ToList();
        }

        public async Task<List<TaskProcessAreaDisplayViewModel>> GetAllWithDetailsAsync()
        {
            return await _db.TaskProcessAreas
                .Include(tpa => tpa.Task) 
                .Include(tpa => tpa.Process) 
                .Include(tpa => tpa.Area) 
                .Select(tpa => new TaskProcessAreaDisplayViewModel
                {
                    TaskProcessAreaId = tpa.TaskProcessAreaId,
                    TaskName = tpa.Task.Name, 
                    ProcessName = tpa.Process.Name, 
                    AreaName = tpa.Area.Name 
                })
                .ToListAsync();
        }

        public async Task<List<TaskProcessAreaUserCustomerDisplayViewModel>> GetAllWithDetailsToDisplayAsync()
        {
            return await _db.TaskProcessAreaUserCustomers
                .Include(tpauc => tpauc.TaskProcessArea)
                    .ThenInclude(tpa => tpa.Task)
                .Include(tpauc => tpauc.TaskProcessArea)
                    .ThenInclude(tpa => tpa.Process)
                .Include(tpauc => tpauc.TaskProcessArea)
                    .ThenInclude(tpa => tpa.Area)
                .Include(tpauc => tpauc.User)
                .Include(tpauc => tpauc.Customer)
                .Select(tpauc => new TaskProcessAreaUserCustomerDisplayViewModel
                {
                    TaskProcessAreaUserCustomerId = tpauc.TaskProcessAreaUserCustomerId,
                    TaskName = tpauc.TaskProcessArea.Task.Name,
                    ProcessName = tpauc.TaskProcessArea.Process.Name,
                    AreaName = tpauc.TaskProcessArea.Area.Name,
                    UserName = tpauc.User.FirstName + " " + tpauc.User.LastName,
                    CustomerName = tpauc.Customer.Name,
                    StartDate = tpauc.StartDate,
                    EndDate = tpauc.EndDate,
                    Status = tpauc.Status,
                    Timestamp = tpauc.Timestamp
                }).ToListAsync();
        }

        public async Task<bool> AddAsync(IEnumerable<TaskProcessAreaUserCustomer> entities)
        {
            try
            {
                await _db.TaskProcessAreaUserCustomers.AddRangeAsync(entities);
                await _db.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
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
            existing.AreaId = taskProcessArea.AreaId;

            await _db.SaveChangesAsync();
            return true;
        }


        public async Task<TaskProcessArea?> GetByIdAsync(Guid id, Guid companyId)
        {
            return await _db.TaskProcessAreas
                .Include(t => t.Task)
                .Include(t => t.Process)
                .Include(t => t.Area)
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

        // For USER
        public async Task<List<TaskProcessAreaUserCustomerDisplayViewModel>> GetAllWithDetailsToDisplayForUserAsync(Guid userId)
        {
            return await _db.TaskProcessAreaUserCustomers
                .Where(tpauc => tpauc.UserId == userId) // Filter by user ID
                .Include(tpauc => tpauc.TaskProcessArea)
                    .ThenInclude(tpa => tpa.Task)
                .Include(tpauc => tpauc.TaskProcessArea)
                    .ThenInclude(tpa => tpa.Process)
                .Include(tpauc => tpauc.TaskProcessArea)
                    .ThenInclude(tpa => tpa.Area)
                .Include(tpauc => tpauc.Customer)
                .Select(tpauc => new TaskProcessAreaUserCustomerDisplayViewModel
                {
                    TaskProcessAreaUserCustomerId = tpauc.TaskProcessAreaUserCustomerId,
                    TaskName = tpauc.TaskProcessArea.Task.Name,
                    ProcessName = tpauc.TaskProcessArea.Process.Name,
                    AreaName = tpauc.TaskProcessArea.Area.Name,
                    CustomerName = tpauc.Customer.Name,
                    StartDate = tpauc.StartDate,
                    EndDate = tpauc.EndDate,
                    Status = tpauc.Status,
                    Timestamp = tpauc.Timestamp
                })
                .ToListAsync();
        }


    }
}

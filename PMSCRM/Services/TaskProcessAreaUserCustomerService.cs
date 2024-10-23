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

		public async Task<bool> UpdateAsync(TaskProcessAreaUserCustomer tpauc)
		{
			var existing = await _db.TaskProcessAreaUserCustomers
									.FirstOrDefaultAsync(t => t.TaskProcessAreaUserCustomerId == tpauc.TaskProcessAreaUserCustomerId);

			if (existing == null)
			{
				return false;
			}
            existing.TaskProcessAreaId = tpauc.TaskProcessAreaId;
			existing.UserId = tpauc.UserId;
			existing.CustomerId = tpauc.CustomerId;
			existing.StartDate = tpauc.StartDate;
			existing.EndDate = tpauc.EndDate;
			existing.Status = tpauc.Status;

			await _db.SaveChangesAsync();
			return true;
		}

		public async Task<TaskProcessAreaUserCustomer?> GetByIdAsync(Guid id, Guid companyId)
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
				.FirstOrDefaultAsync(tpauc => tpauc.TaskProcessAreaUserCustomerId == id && tpauc.TaskProcessArea.CompanyId == companyId);
		}

        public async Task<bool> DeleteAsync(Guid id, Guid companyId)
        {
            var tpauc = await _db.TaskProcessAreaUserCustomers
                .FirstOrDefaultAsync(tpauc => tpauc.TaskProcessAreaUserCustomerId == id
                                     && tpauc.TaskProcessArea.CompanyId == companyId);

            if (tpauc == null)
            {
                return false;
            }

            _db.TaskProcessAreaUserCustomers.Remove(tpauc);
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

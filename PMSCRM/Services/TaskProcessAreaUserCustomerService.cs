using PMSCRM.Models;

namespace PMSCRM.Services
{
    public class TaskProcessAreaUserCustomerService
    {
        private readonly PmscrmContext _db;

        public TaskProcessAreaUserCustomerService(PmscrmContext db)
        {
            _db = db;
        }

        public List<TaskProcessAreaUserCustomer> GetAll()
        {
            return _db.TaskProcessAreaUserCustomers.ToList();
        }

        public bool Add(TaskProcessAreaUserCustomer tpauc)
        {
            bool exists = _db.TaskProcessAreaUserCustomers.Contains(tpauc);

            if (exists)
            {
                return false;
            }
            _db.Add(tpauc);
            _db.SaveChanges();
            return true;
        }

        public bool Update(Guid id, TaskProcessAreaUserCustomer updated)
        {
            var existing = _db.TaskProcessAreaUserCustomers.FirstOrDefault(x => x.TaskProcessAreaUserCustomerId == id);
            if (existing == null)
            {
                return false;
            }

            existing.CompanyId = updated.CompanyId;
            existing.TaskProcessAreaId = updated.TaskProcessAreaId;
            existing.UserId = updated.UserId;
            existing.CustomerId = updated.CustomerId;
            existing.StartDate = updated.StartDate;
            existing.EndDate = updated.EndDate;
            existing.Status = updated.Status;

            _db.SaveChanges();
            return true;
        }

        public bool Delete(Guid id)
        {
            var toDelete = _db.TaskProcessAreaUserCustomers.Find(id);

            if (toDelete == null)
            {
                return false;
            }

            _db.TaskProcessAreaUserCustomers.Remove(toDelete);
            _db.SaveChanges();
            return true;
        }
    }
}

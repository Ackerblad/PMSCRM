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

        public bool Add(TaskProcessAreaUserCustomer customer)
        {
            bool exists = _db.TaskProcessAreaUserCustomers.Contains(customer);

            if (exists)
            {
                return false;
            }
            _db.Add(customer);
            _db.SaveChanges();
            return true;
        } 

        public bool Update(Guid guid, TaskProcessAreaUserCustomer updated)
        {
            var existing = _db.TaskProcessAreaUserCustomers.FirstOrDefault(x => x.TaskProcessAreaUserCustomerId == guid);
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

        public bool Delete(Guid guid)
        {
            var toDelete = _db.TaskProcessAreaUserCustomers.Find(guid);

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

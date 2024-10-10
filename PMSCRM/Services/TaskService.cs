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

        public List<Models.Task> GetAll(Guid companyId)
        {
            return _db.Tasks.Where(t => t.CompanyId == companyId).ToList();
        }

        public Models.Task? GetById(Guid id, Guid companyId)
        {
            return _db.Tasks.FirstOrDefault(t => t.TaskId == id && t.CompanyId == companyId);
        }

        public bool Add(Models.Task task)
        {
            bool exists = _db.Tasks.Any(t => t.Name == task.Name && t.CompanyId == task.CompanyId);

            if (exists)
            {
                return false;
            }

            _db.Tasks.Add(task);
            _db.SaveChanges();
            return true;
        }

        public bool Update(Guid id, Models.Task updated)
        {
            var task = _db.Tasks.FirstOrDefault(t => t.TaskId == id && t.CompanyId == updated.CompanyId);

            if (task == null)
            {
                return false;
            }

            task.CompanyId = updated.CompanyId;
            task.Name = updated.Name;
            task.Description = updated.Description;
            task.Duration = updated.Duration;

            _db.SaveChanges();
            return true;
        }

        public bool Delete(Guid id, Guid companyId)
        {
            var toDelete = _db.Tasks.FirstOrDefault(t => t.TaskId == id && t.CompanyId == companyId);

            if (toDelete == null)
            {
                return false;
            }

            _db.Tasks.Remove(toDelete);
            _db.SaveChanges();
            return true;
        }
    }
}

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

        public List<Models.Task> GetAll()
        {
            return _db.Tasks.ToList();
        }

        public Models.Task? GetById(Guid id)
        {
            return _db.Tasks.Find(id);
        }

        public bool Add(Models.Task task)
        {
            bool exists = _db.Tasks.Contains(task);

            if (exists)
            {
                return false;
            }

            _db.Tasks.Add(task);
            _db.SaveChanges();
            return true;
        }

        public bool Update(Guid guid, Models.Task updated)
        {
            var task = _db.Tasks.Find(guid);

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

        public bool Delete(Guid guid)
        {
            var toDelete = _db.Tasks.Find(guid);

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

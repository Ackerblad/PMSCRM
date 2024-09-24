using Microsoft.AspNetCore.Mvc;
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

        public List<TaskProcessArea> GetAll()
        {
            return _db.TaskProcessAreas.ToList();
        }

        public bool Add(TaskProcessArea taskProcessArea)
        {
            bool exists = _db.TaskProcessAreas.Contains(taskProcessArea);

            if (exists)
            {
                return false;
            }
            _db.TaskProcessAreas.Add(taskProcessArea);
            _db.SaveChanges();
            return true;
        }

        public bool Update(Guid guid, TaskProcessArea updated)
        {
            var existing = _db.TaskProcessAreas.FirstOrDefault(t => t.TaskProcessAreaId == guid);
            
            if (existing == null)
            {
                return false;
            }

            existing.CompanyId = updated.CompanyId;
            existing.TaskId = updated.TaskId;
            existing.ProcessId = updated.ProcessId;
            existing.AreaId = updated.AreaId;

            _db.SaveChanges();
            return true;
        }

        public bool Delete(Guid guid)
        {
            var toDelete = _db.TaskProcessAreas.Find(guid);

            if (toDelete == null)
            {
                return false;
            }

            _db.TaskProcessAreas.Remove(toDelete);
            _db.SaveChanges();
            return true;

        }
    }
}

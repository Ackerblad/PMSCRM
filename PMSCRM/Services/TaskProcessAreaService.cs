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

        public List<TaskProcessArea> GetTaskProcessAreas()
        {
            return _db.TaskProcessAreas.ToList();
        }

        public bool AddTaskProcessArea(TaskProcessArea taskProcessArea)
        {
            bool taskProcessAreaExist = _db.TaskProcessAreas.Contains(taskProcessArea);

            if (taskProcessAreaExist)
            {
                return false;
            }
            _db.TaskProcessAreas.Add(taskProcessArea);
            _db.SaveChanges();
            return true;
        }

        public bool UpdateTaskProcessArea(Guid taskProcessAreaId, TaskProcessArea updatedTaskProcessArea)
        {
            var existingTaskProcessArea = _db.TaskProcessAreas.FirstOrDefault(t => t.TaskProcessAreaId == taskProcessAreaId);
            
            if (existingTaskProcessArea == null)
            {
                return false;
            }

            existingTaskProcessArea.CompanyId = updatedTaskProcessArea.CompanyId;
            existingTaskProcessArea.TaskId = updatedTaskProcessArea.TaskId;
            existingTaskProcessArea.ProcessId = updatedTaskProcessArea.ProcessId;
            existingTaskProcessArea.AreaId = updatedTaskProcessArea.AreaId;

            _db.SaveChanges();
            return true;
        }

        public bool DeleteTaskProcessArea(Guid taskProcessAreaId)
        {
            var taskProcessAreaToDelete = _db.TaskProcessAreas.Find(taskProcessAreaId);

            if (taskProcessAreaToDelete == null)
            {
                return false;
            }

            _db.TaskProcessAreas.Remove(taskProcessAreaToDelete);
            _db.SaveChanges();
            return true;

        }
    }
}

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

        public List<Models.Task> GetTasks()
        {
            return _db.Tasks.ToList();
        }

        public bool AddTask(Models.Task task)
        {
            bool taskExist = _db.Tasks.Contains(task);

            if (taskExist)
            {
                return false;
            }

            _db.Tasks.Add(task);
            _db.SaveChanges();
            return true;
        }

        public bool UpdateTask(Guid taskId, Models.Task updatedTask)
        {
            var existingTask = _db.Tasks.FirstOrDefault(u => u.TaskId == taskId); 

            if (existingTask == null || updatedTask.CompanyId == Guid.Empty)
            {
                return false;
            }

            existingTask.CompanyId = updatedTask.CompanyId;
            existingTask.Name = updatedTask.Name;
            existingTask.Description = updatedTask.Description;
            existingTask.Duration = updatedTask.Duration;

            _db.SaveChanges();
            return true;
        } 

        public bool DeleteTask(Guid taskId)
        {
            var taskToDelete = _db.Tasks.Find(taskId);

            if (taskToDelete == null)
            {
                return false;
            }
            _db.Tasks.Remove(taskToDelete);
            _db.SaveChanges();
            return true;
        }

    }
}

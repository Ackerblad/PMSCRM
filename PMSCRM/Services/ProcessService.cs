using Microsoft.AspNetCore.Mvc;
using PMSCRM.Models;

namespace PMSCRM.Services
{
    public class ProcessService : Controller
    {
        private readonly PmscrmContext _db;

        public ProcessService(PmscrmContext db)
        {
            _db = db;
        }

        public List<Process> GetProcesses()
        {
            return _db.Processes.ToList();
        }

        public bool AddProcess(Process process)
        {
            bool processExist = _db.Processes.Contains(process);

            if (processExist)
            {
                return false;
            }

            _db.Processes.Add(process);
            _db.SaveChanges();
            return true;
        }

        public bool UpdateProcess(Guid processId, Process updatedProcess)
        {
            var existingProcess = _db.Processes.FirstOrDefault(p => p.ProcessId == processId);

            if (existingProcess == null || existingProcess.CompanyId == Guid.Empty)
            {
                return false;
            }

            existingProcess.CompanyId = updatedProcess.CompanyId;
            existingProcess.Name = updatedProcess.Name;
            existingProcess.Description = updatedProcess.Description;
            existingProcess.Duration = updatedProcess.Duration;


            _db.SaveChanges();
            return true;
        }

        public bool DeleteProcess(Guid processId)
        {
            var processToDelete = _db.Processes.Find(processId);

            if (processToDelete == null)
            {
                return false;
            }
            _db.Processes.Remove(processToDelete);
            _db.SaveChanges();
            return true;
        }
        public IActionResult Index()
        {
            return View();
        }
    }
}

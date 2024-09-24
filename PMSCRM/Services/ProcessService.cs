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

        public List<Process> GetAll()
        {
            return _db.Processes.ToList();
        }

        public bool Add(Process process)
        {
            bool exist = _db.Processes.Contains(process);

            if (exist)
            {
                return false;
            }

            _db.Processes.Add(process);
            _db.SaveChanges();
            return true;
        }

        public bool Update(Guid guid, Process updated)
        {
            var existing = _db.Processes.FirstOrDefault(p => p.ProcessId == guid);

            if (existing == null || existing.CompanyId == Guid.Empty)
            {
                return false;
            }

            existing.CompanyId = updated.CompanyId;
            existing.Name = updated.Name;
            existing.Description = updated.Description;
            existing.Duration = updated.Duration;

            _db.SaveChanges();
            return true;
        }

        public bool Delete(Guid guid)
        {
            var toDelete = _db.Processes.Find(guid);

            if (toDelete == null)
            {
                return false;
            }
            _db.Processes.Remove(toDelete);
            _db.SaveChanges();
            return true;
        }
        public IActionResult Index()
        {
            return View();
        }
    }
}

using PMSCRM.Models;

namespace PMSCRM.Services
{
    public class CommunicationLogService
    {
        private readonly PmscrmContext _db;

        public CommunicationLogService(PmscrmContext db)
        {
            _db = db;
        }

        public List<CommunicationLog> GetAll()
        {
            return _db.CommunicationLogs.ToList();
        }

        public bool Add(CommunicationLog comLog)
        {
            bool exists = _db.CommunicationLogs.Contains(comLog);

            if (exists)
            {
                return false;
            }

            _db.CommunicationLogs.Add(comLog);
            _db.SaveChanges();
            return true;
        }

        public bool Update(Guid guid, CommunicationLog updated)
        {
            var existing = _db.CommunicationLogs.FirstOrDefault(c => c.CommunicationLogId == guid);

            if (existing == null || existing.CompanyId == Guid.Empty)
            {
                return false;
            }

            existing.CompanyId = updated.CompanyId;
            existing.CustomerId = updated.CustomerId;
            existing.TaskId = updated.TaskId;

            _db.SaveChanges();
            return true;
        }

        public bool Delete(Guid guid)
        {
            var toDelete = _db.CommunicationLogs.Find(guid);

            if (toDelete == null)
            {
                return false;
            }

            _db.CommunicationLogs.Remove(toDelete);
            _db.SaveChanges();
            return true;
        }
    }
}

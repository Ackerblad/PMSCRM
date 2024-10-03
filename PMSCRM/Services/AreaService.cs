using PMSCRM.Models;

namespace PMSCRM.Services
{
    public class AreaService
    {
        private readonly PmscrmContext _db;

        public AreaService(PmscrmContext db)
        {
            _db = db;
        }

        public List<Area> GetAll()
        {
            return _db.Areas.ToList();
        }

        public Area? GetById(Guid id)
        {
            return _db.Areas.Find(id);
        }

        public bool Add(Area area)
        {
            bool exists = _db.Areas.Contains(area);

            if (exists)
            {
                return false;
            }

            _db.Areas.Add(area);
            _db.SaveChanges();
            return true;
        }

        public bool Update(Guid guid, Area updated)
        {
            var existing = _db.Areas.FirstOrDefault(a => a.AreaId == guid);

            if(existing == null)
            {
                return false;
            }

            existing.CompanyId = updated.CompanyId;
            existing.Name = updated.Name;
            existing.Description = updated.Description;

            _db.SaveChanges();
            return true;
        }

        public bool Delete(Guid guid)
        {
            var toDelete = _db.Areas.Find(guid);

            if (toDelete == null)
            {
                return false;
            }
            _db.Areas.Remove(toDelete);
            _db.SaveChanges();
            return true;
        }
    }
}

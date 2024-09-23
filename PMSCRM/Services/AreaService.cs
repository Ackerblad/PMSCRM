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

        public List<Area> GetAreas()
        {
            return _db.Areas.ToList();
        }

        public bool AddArea(Area area)
        {
            bool areaExists = _db.Areas.Contains(area);

            if (areaExists)
            {
                return false;
            }

            _db.Areas.Add(area);
            _db.SaveChanges();
            return true;
        }

        public bool UpdateArea(Guid areaId, Area updatedArea)
        {
            var existingArea = _db.Areas.FirstOrDefault(a => a.AreaId == areaId);

            if(existingArea == null || existingArea.CompanyId == Guid.Empty)
            {
                return false;
            }

            existingArea.CompanyId = updatedArea.CompanyId;
            existingArea.Name = updatedArea.Name;
            existingArea.Description = updatedArea.Description;

            _db.SaveChanges();
            return true;
        }

        public bool DeleteArea(Guid areaId)
        {
            var areaToDelete = _db.Areas.Find(areaId);

            if (areaToDelete == null)
            {
                return false;
            }
            _db.Areas.Remove(areaToDelete);
            _db.SaveChanges();
            return true;
        }


    }
}

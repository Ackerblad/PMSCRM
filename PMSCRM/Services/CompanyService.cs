using PMSCRM.Models;

namespace PMSCRM.Services
{
    public class CompanyService
    {
        private readonly PmscrmContext _db;

        public CompanyService(PmscrmContext db)
        {
            _db = db;
        }

        public List<Company> GetCompanies()
        {
            return _db.Companies.ToList();
        }

        public bool AddCompany(Company company)
        {
            bool companyExists = _db.Companies.Contains(company);

            if (companyExists)
            {
                return false;
            }

            _db.Companies.Add(company);
            _db.SaveChanges();
            return true;
        }

        public bool UpdateCompany(Guid companyId, Company updatedCompany)
        {
            var existingCompany = _db.Companies.FirstOrDefault(x => x.CompanyId == companyId);

            if (existingCompany == null || existingCompany.CompanyId == Guid.Empty)
            {
                return false;
            }

            existingCompany.Name = updatedCompany.Name;

            _db.SaveChanges();
            return true;
        }

        public bool DeleteCompany(Guid companyId)
        {
            var companyToDelete = _db.Companies.Find(companyId);

            if (companyToDelete == null)
            {
                return false;
            }

            _db.Companies.Remove(companyToDelete);
            _db.SaveChanges();
            return true;
        }
    }
}

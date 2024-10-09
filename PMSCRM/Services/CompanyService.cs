using Microsoft.EntityFrameworkCore;
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

        public List<Company> GetAll()
        {
            return _db.Companies.ToList();
        }

        public Company? GetById(Guid id)
        {
            return _db.Companies.Find(id);
        }

        public List<string> GetCompanyNames(string term)
        {
            // Query to find company names containing the search term
            return _db.Companies
                           .Where(c => c.Name.Contains(term))
                           .Select(c => c.Name)
                           .ToList();
        }

        public Company GetCompanyByName(string companyName)
        {
            // Find the company by name
            return _db.Companies.FirstOrDefault(c => c.Name == companyName);
        }

        public bool Add(Company company)
        {
            bool exists = _db.Companies.Contains(company);

            if (exists)
            {
                return false;
            }

            _db.Companies.Add(company);
            _db.SaveChanges();
            return true;
        }

        public bool Update(Guid guid, Company updated)
        {
            var existing = _db.Companies.FirstOrDefault(x => x.CompanyId == guid);

            if (existing == null)
            {
                return false;
            }

            existing.Name = updated.Name;

            _db.SaveChanges();
            return true;
        }

        public bool Delete(Guid guid)
        {
            var toDelete = _db.Companies.Find(guid);

            if (toDelete == null)
            {
                return false;
            }

            _db.Companies.Remove(toDelete);
            _db.SaveChanges();
            return true;
        }
    }
}

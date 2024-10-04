using Microsoft.AspNetCore.Mvc;
using PMSCRM.Models;

namespace PMSCRM.Services
{
    public class CustomerService : Controller
    {
        private readonly PmscrmContext _db;

        public CustomerService(PmscrmContext db)
        {
            _db = db;
        }

        public List<Customer> GetAll()
        {
            return _db.Customers.ToList();
        }
        public Customer? GetById(Guid id)
        {
            return _db.Customers.Find(id);
        }

        public bool Add(Customer customer)
        {
            bool exists = _db.Customers.Contains(customer);

            if (exists)
            {
                return false;
            }

            _db.Customers.Add(customer);
            _db.SaveChanges();
            return true;
        }

        public bool Update(Guid guid, Customer updated)
        {
            var existing = _db.Customers.FirstOrDefault(c => c.CustomerId == guid);

            if (existing == null)
            {
                return false;
            }

            existing.CompanyId = updated.CompanyId;
            existing.Name = updated.Name;
            existing.PhoneNumber = updated.PhoneNumber;
            existing.EmailAddress = updated.EmailAddress;
            existing.StreetAddress = updated.StreetAddress;
            existing.City = updated.City;
            existing.Country = updated.Country;

            _db.SaveChanges();
            return true;
        }

        public bool Delete(Guid guid)
        {
            var toDelete = _db.Customers.Find(guid);

            if (toDelete == null)
            {
                return false;
            }

            _db.Customers.Remove(toDelete);
            _db.SaveChanges();
            return true;
        }

        public IActionResult Index()
        {
            return View();
        }
    }
}

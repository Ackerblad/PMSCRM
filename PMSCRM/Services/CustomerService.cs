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

        public List<Customer> GetCustomers()
        {
            return _db.Customers.ToList();
        }

        public bool AddCustomer(Customer customer)
        {
            bool customerExists = _db.Customers.Contains(customer);

            if (customerExists)
            {
                return false;
            }

            _db.Customers.Add(customer);
            _db.SaveChanges();
            return true;
        }

        public bool UpdateCustomer(Guid id, Customer updatedCustomer)
        {
            var existingCustomer = _db.Customers.FirstOrDefault(c => c.CustomerId == id);

            if (existingCustomer == null || updatedCustomer.CompanyId == Guid.Empty)
            {
                return false;
            }

            existingCustomer.CompanyId = updatedCustomer.CompanyId;
            existingCustomer.Name = updatedCustomer.Name;
            existingCustomer.PhoneNumber = updatedCustomer.PhoneNumber;
            existingCustomer.EmailAddress = updatedCustomer.EmailAddress;
            existingCustomer.StreetAddress = updatedCustomer.StreetAddress;
            existingCustomer.City = updatedCustomer.City;
            existingCustomer.Country = updatedCustomer.Country;

            _db.SaveChanges();
            return true;
        }

        public bool DeleteCustomer(Guid customerId)
        {
            var customerToDelete = _db.Customers.Find(customerId);

            if (customerToDelete == null)
            {
                return false;
            }

            _db.Customers.Remove(customerToDelete);
            _db.SaveChanges();
            return true;
        }

        public IActionResult Index()
        {
            return View();
        }
    }
}

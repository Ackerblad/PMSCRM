using Microsoft.EntityFrameworkCore;
using PMSCRM.Models;

namespace PMSCRM.Services
{
    public class CustomerService
    {
        private readonly PmscrmContext _db;

        public CustomerService(PmscrmContext db)
        {
            _db = db;
        }

        public async Task<List<Customer>> GetAllAsync(Guid companyId)
        {
            return await _db.Customers
                .Where(c => c.CompanyId == companyId)
                .ToListAsync();
        }

        public async Task<Customer?> GetByIdAsync(Guid id, Guid companyId)
        {
            return await _db.Customers
                .FirstOrDefaultAsync(c => c.CustomerId == id && c.CompanyId == companyId);
        }

        public async Task<bool> AddAsync(Customer customer)
        {
            bool exists = await _db.Customers
                .AnyAsync(c => c.Name == customer.Name && c.CompanyId == customer.CompanyId);

            if (exists)
            {
                return false;
            }

            await _db.Customers.AddAsync(customer);
            await _db.SaveChangesAsync();
            return true;
        }

        public async Task<bool> UpdateAsync(Guid id, Customer updatedCustomer)
        {
            var existing = await _db.Customers
                .FirstOrDefaultAsync(c => c.CustomerId == id && c.CompanyId == updatedCustomer.CompanyId);

            if (existing == null)
            {
                return false;
            }

            existing.Name = updatedCustomer.Name;
            existing.PhoneNumber = updatedCustomer.PhoneNumber;
            existing.EmailAddress = updatedCustomer.EmailAddress;
            existing.StreetAddress = updatedCustomer.StreetAddress;
            existing.City = updatedCustomer.City;
            existing.Country = updatedCustomer.Country;

            await _db.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeleteAsync(Guid id, Guid companyId)
        {
            var toDelete = await _db.Customers
                .FirstOrDefaultAsync(c => c.CustomerId == id && c.CompanyId == companyId);

            if (toDelete == null)
            {
                return false;
            }

            _db.Customers.Remove(toDelete);
            await _db.SaveChangesAsync();
            return true;
        }

        public async Task<List<Customer>> SearchCustomersAsync(Guid companyId, string query)
        {
            query = query.Trim();

            if (string.IsNullOrEmpty(query))
            {
                return await _db.Customers
                    .Where(c => c.CompanyId == companyId)
                    .ToListAsync();
            }

            return await _db.Customers
                .Where(c => c.CompanyId == companyId &&
                           (c.Name.ToLower().Contains(query.ToLower()) ||
                            c.EmailAddress.ToLower().Contains(query.ToLower()) ||
                            c.StreetAddress.ToLower().Contains(query.ToLower()) ||
                            c.City.ToLower().Contains(query.ToLower()) ||
                            c.StateOrProvince.ToLower().Contains(query.ToLower()) ||
                            c.PostalCode.ToLower().Contains(query.ToLower()) ||
                            c.Country.ToLower().Contains(query.ToLower())))
                .ToListAsync();
        }
    }
}

using PMSCRM.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PMSCRM.Services
{
    public class CustomerService
    {
        private readonly PmscrmContext _db;

        public CustomerService(PmscrmContext db)
        {
            _db = db;
        }

        // Get all customers for the specified company (Async)
        public async Task<List<Customer>> GetAllAsync(Guid companyId)
        {
            return await _db.Customers
                .Where(c => c.CompanyId == companyId)
                .ToListAsync();
        }

        // Get customer by ID and company (Async)
        public async Task<Customer?> GetByIdAsync(Guid id, Guid companyId)
        {
            return await _db.Customers
                .FirstOrDefaultAsync(c => c.CustomerId == id && c.CompanyId == companyId);
        }

        // Add a new customer (Async)
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

        // Update an existing customer (Async)
        public async Task<bool> UpdateAsync(Guid id, Customer updatedCustomer)
        {
            var existing = await _db.Customers
                .FirstOrDefaultAsync(c => c.CustomerId == id && c.CompanyId == updatedCustomer.CompanyId);

            if (existing == null)
            {
                return false;
            }

            // Update customer properties
            existing.Name = updatedCustomer.Name;
            existing.PhoneNumber = updatedCustomer.PhoneNumber;
            existing.EmailAddress = updatedCustomer.EmailAddress;
            existing.StreetAddress = updatedCustomer.StreetAddress;
            existing.City = updatedCustomer.City;
            existing.Country = updatedCustomer.Country;

            await _db.SaveChangesAsync();
            return true;
        }

        // Delete a customer (Async)
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
    }
}

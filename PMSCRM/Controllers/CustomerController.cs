using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PMSCRM.Models;
using PMSCRM.Services;
using PMSCRM.Utilities;

namespace PMSCRM.Controllers
{
    [Authorize]
    [Route("[controller]")]
    public class CustomerController : Controller
    {
        private readonly CustomerService _customerService;
        private readonly CompanyDivider _companyDivider;

        public CustomerController(CustomerService customerService, CompanyDivider companyDivider)
        {
            _customerService = customerService;
            _companyDivider = companyDivider;
        }

        // Add Customer (POST)
        [HttpPost]
        public async Task<ActionResult> AddCustomer(Customer customer)
        {
            if (!ModelState.IsValid)
            {
                return View(customer);
            }

            customer.CompanyId = _companyDivider.GetCompanyId();

            bool success = await _customerService.AddAsync(customer);
            if (success)
            {
                return RedirectToAction("Success");
            }

            ModelState.AddModelError(string.Empty, "Failed to add customer.");
            return View(customer);
        }

        // Edit Customer (GET)
        [HttpGet("EditCustomer/{id}")]
        public async Task<IActionResult> EditCustomer(Guid id)
        {
            var companyId = _companyDivider.GetCompanyId();
            var customer = await _customerService.GetByIdAsync(id, companyId);

            if (customer == null)
            {
                return NotFound("Customer not found");
            }
            return View(customer);
        }

        // Edit Customer (POST)
        [HttpPost("EditCustomer/{id}")]
        public async Task<IActionResult> EditCustomer(Guid id, Customer updatedCustomer)
        {
            if (!ModelState.IsValid)
            {
                return View(updatedCustomer);
            }

            updatedCustomer.CompanyId = _companyDivider.GetCompanyId();

            bool success = await _customerService.UpdateAsync(id, updatedCustomer);
            if (success)
            {
                return RedirectToAction("ViewCustomers");
            }

            ModelState.AddModelError(string.Empty, "Failed to update customer.");
            return View(updatedCustomer);
        }

        // Delete Customer (GET)
        [HttpGet("DeleteCustomer/{id}")]
        public async Task<IActionResult> DeleteCustomer(Guid id)
        {
            var companyId = _companyDivider.GetCompanyId();
            var customer = await _customerService.GetByIdAsync(id, companyId);

            if (customer == null)
            {
                return NotFound("Customer not found");
            }

            return View(customer);
        }

        // Delete Customer (POST)
        [HttpPost("DeleteConfirmed/{id}")]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            var companyId = _companyDivider.GetCompanyId();
            var customer = await _customerService.GetByIdAsync(id, companyId);

            if (customer == null)
            {
                return NotFound("Customer not found.");
            }

            bool success = await _customerService.DeleteAsync(id, companyId);
            if (success)
            {
                TempData["SuccessMessage"] = "Customer deleted successfully!";
                return RedirectToAction("ViewCustomers");
            }

            ModelState.AddModelError(string.Empty, "Failed to delete customer.");
            return View("DeleteCustomer", customer);
        }

        public IActionResult AddCustomer()
        {
            return View();
        }

        // View Customers (GET)
        [HttpGet("ViewCustomers")]
        public async Task<IActionResult> ViewCustomers(string sortBy, string sortDirection = "asc")
        {
            var companyId = _companyDivider.GetCompanyId();
            var customers = await _customerService.GetAllAsync(companyId);

            customers = customers.SortBy(sortBy, sortDirection).ToList();

            // Pass current sorting info to the view (used for toggling sort direction)
            ViewBag.CurrentSort = sortBy;
            ViewBag.CurrentSortDirection = sortDirection;
            return View(customers);
        }

        // Success View (GET)
        [HttpGet("Success")]
        public IActionResult Success()
        {
            return View();
        }

        // Unused Views
        [HttpGet("EditCustomer")]
        public IActionResult EditCustomer()
        {
            return View();
        }

        [HttpGet("DeleteCustomer")]
        public IActionResult DeleteCustomer()
        {
            return View();
        }
    }
}

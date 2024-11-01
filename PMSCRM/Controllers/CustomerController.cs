using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PMSCRM.Models;
using PMSCRM.Services;
using PMSCRM.Utilities;
using System.Threading.Tasks;

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

        private Guid GetCompanyId() 
        {
            return _companyDivider.GetCompanyId();
        }

        [HttpGet]
        public IActionResult AddCustomer()
        {
            ViewBag.IsEditMode = false;
            return View();
        }

        [HttpPost]
        public async Task<ActionResult> AddCustomer(Customer customer)
        {
            ViewBag.IsEditMode = false;
            if (!ModelState.IsValid) return View(customer);

            customer.CompanyId = GetCompanyId();
            if (customer.CompanyId == Guid.Empty) return View(customer);

            bool success = await _customerService.AddAsync(customer);
            if (!success)
            {
                ViewBag.Message = "Failed to add customer. Please try again.";
                ViewBag.MessageType = "error";
                return View(customer);
            }

            ViewBag.Message = "Customer added successfully!";
            ViewBag.MessageType = "success";
            return View("AddCustomer", customer);
        }

        [HttpGet("EditCustomer/{id}")]
        public async Task<IActionResult> EditCustomer(Guid id)
        {
            var companyId = GetCompanyId();
            var customer = await _customerService.GetByIdAsync(id, companyId);

            if (customer == null)
            {
                return NotFound("Customer not found");
            }
            ViewBag.IsEditMode = true;
            return View("AddCustomer", customer);
        }

        [HttpPost("EditCustomer/{id}")]
        public async Task<IActionResult> EditCustomer(Guid id, Customer updatedCustomer)
        {
            if (!ModelState.IsValid)
            {
                return View(updatedCustomer);
            }

            updatedCustomer.CompanyId = GetCompanyId();

            bool success = await _customerService.UpdateAsync(id, updatedCustomer);
            if (success)
            {
                return RedirectToAction("ViewCustomers");
            }

            ModelState.AddModelError(string.Empty, "Failed to update customer.");
            return View(updatedCustomer);
        }

        [HttpGet("DeleteCustomer/{id}")]
        public async Task<IActionResult> DeleteCustomer(Guid id)
        {
            var companyId = GetCompanyId();
            var customer = await _customerService.GetByIdAsync(id, companyId);

            if (customer == null)
            {
                return NotFound("Customer not found");
            }

            return View(customer);
        }

        [HttpPost("DeleteConfirmed/{id}")]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            var companyId = GetCompanyId();
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

        [HttpGet("ViewCustomers")]
        public async Task<IActionResult> ViewCustomers(string sortBy = "TimeStamp", string sortDirection = "desc")
        {
            var companyId = GetCompanyId();
            var customers = await _customerService.GetAllAsync(companyId);

            customers = customers.SortBy(sortBy, sortDirection).ToList();

            ViewBag.CurrentSort = sortBy;
            ViewBag.CurrentSortDirection = sortDirection;
            return View(customers);
        }

        [HttpGet("Details/{id}")]
        public async Task<IActionResult> Details(Guid id)
        {
            var companyId = GetCompanyId();
            var customer = await _customerService.GetByIdAsync(id, companyId);

            if (customer == null)
            {
                ViewBag.Message = "Customer not found.";
                return View("ViewCustomers", customer);
            }

            return View(customer);
        }
    }
}

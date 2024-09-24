using Microsoft.AspNetCore.Mvc;
using PMSCRM.Models;
using PMSCRM.Services;

namespace PMSCRM.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CustomerController : Controller
    {
        CustomerService _customerService;

        public CustomerController(CustomerService customerService)
        {
            _customerService = customerService;
        }

        [HttpGet("GetCustomers")]
        public ActionResult<List<Customer>> GetCustomers()
        {
            var customers = _customerService.GetCustomers();
            if (customers == null || !customers.Any())
            {
                return NotFound("No customers found");
            }
            return Ok(customers);
        }

        [HttpPost("AddCustomer")]
        public ActionResult AddCustomer([FromBody] Customer customer)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var success = _customerService.AddCustomer(customer);
            if (success)
            {
                return Ok("Customer was added");
            }
            return BadRequest("Failed to add customer");
        }

        [HttpPut("{id}")]
        public ActionResult UpdateCustomer(Guid id, [FromBody] Customer customer)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            bool success = _customerService.UpdateCustomer(id, customer);
            if (success)
            {
                return Ok("Customer was updated");
            }
            return BadRequest("Failed to update customer");
        }

        [HttpDelete("{id}")]
        public ActionResult DeleteCustomer(Guid id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            bool success = _customerService.DeleteCustomer(id);
            if(success)
            {
                return Ok("Customer was deleted");
            }
            return BadRequest("Customer was not deleted");
        }

        public IActionResult Index()
        {
            return View();
        }
    }
}

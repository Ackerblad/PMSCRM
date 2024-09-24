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

        [HttpGet("GetAll")]
        public ActionResult<List<Customer>> GetAll()
        {
            var customers = _customerService.GetAll();
            if (customers == null || !customers.Any())
            {
                return NotFound("No customers found");
            }
            return Ok(customers);
        }

        [HttpPost("Add")]
        public ActionResult Add([FromBody] Customer customer)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var success = _customerService.Add(customer);
            if (success)
            {
                return Ok("Customer was added");
            }
            return BadRequest("Failed to add customer");
        }

        [HttpPut("{id}")]
        public ActionResult Update(Guid guid, [FromBody] Customer customer)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            bool success = _customerService.Update(guid, customer);
            if (success)
            {
                return Ok("Customer was updated");
            }
            return BadRequest("Failed to update customer");
        }

        [HttpDelete("{id}")]
        public ActionResult Delete(Guid guid)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            bool success = _customerService.Delete(guid);
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

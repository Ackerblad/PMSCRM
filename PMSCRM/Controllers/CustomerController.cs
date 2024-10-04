using Microsoft.AspNetCore.Mvc;
using PMSCRM.Models;
using PMSCRM.Services;

namespace PMSCRM.Controllers
{
    [Route("[controller]")]
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

        //[HttpPost("Add")]
        //public ActionResult Add([FromBody] Customer customer)
        //{
        //    if (!ModelState.IsValid)
        //    {
        //        return BadRequest(ModelState);
        //    }

        //    var success = _customerService.Add(customer);
        //    if (success)
        //    {
        //        return Ok("Customer was added");
        //    }
        //    return BadRequest("Failed to add customer");
        //}

        [HttpPost]
        public ActionResult AddCustomer(Customer customer)
        {
            if (!ModelState.IsValid)
            {
                return View(customer);
            }

            bool success = _customerService.Add(customer);
            if (success)
            {
                return RedirectToAction("Success");
            }

            ModelState.AddModelError(string.Empty, "Failed to add customer");
            return View(customer);
        }

        //[HttpPut("{id}")]
        //public ActionResult Update(Guid guid, [FromBody] Customer customer)
        //{
        //    if (!ModelState.IsValid)
        //    {
        //        return BadRequest(ModelState);
        //    }

        //    bool success = _customerService.Update(guid, customer);
        //    if (success)
        //    {
        //        return Ok("Customer was updated");
        //    }
        //    return BadRequest("Failed to update customer");
        //}

        [HttpGet("EditCustomer/{id}")]
        public IActionResult EditCustomer(Guid id)
        {
            var customer = _customerService.GetById(id);
            if (customer == null)
            {
                return NotFound("Customer not found");
            }
            return View(customer);
        }

        [HttpPost("EditCustomer/{id}")]
        public IActionResult EditCustomer(Guid id, Customer updatedCustomer)
        {
            if (!ModelState.IsValid)
            {
                return View(updatedCustomer);
            }

            bool success = _customerService.Update(id, updatedCustomer);
            if (success)
            {
                return RedirectToAction("ViewCustomers");
            }

            ModelState.AddModelError(string.Empty, "Failed to update customer.");
            return View("updatedCustomer");
        }

        //[HttpDelete("{id}")]
        //public ActionResult Delete(Guid guid)
        //{
        //    if (!ModelState.IsValid)
        //    {
        //        return BadRequest(ModelState);
        //    }

        //    bool success = _customerService.Delete(guid);
        //    if(success)
        //    {
        //        return Ok("Customer was deleted");
        //    }
        //    return BadRequest("Customer was not deleted");
        //}

        // GET: Task/DeleteTask/{id}
        [HttpGet("DeleteCustomer/{id}")]
        public IActionResult DeleteCustomer(Guid id)
        {
            var customer = _customerService.GetById(id);
            if (customer == null)
            {
                return NotFound("Customer not found");
            }

            return View(customer);
        }

        [HttpPost("DeleteConfirmed/{id}")]
        public IActionResult DeleteConfirmed(Guid id)
        {
            var customer = _customerService.GetById(id);
            if (customer == null)
            {
                return NotFound("Customer not found.");
            }

            bool success = _customerService.Delete(id);
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

        [HttpGet("Success")]
        public IActionResult Success()
        {
            return View();
        }

        [HttpGet("ViewCustomers")]
        public IActionResult ViewCustomers()
        {
            var customer = _customerService.GetAll();
            return View(customer);
        }
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

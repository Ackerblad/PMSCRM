using Microsoft.AspNetCore.Mvc;
using PMSCRM.Models;
using PMSCRM.Services;

namespace PMSCRM.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TaskProcessAreaUserCustomerController : Controller
    {
        TaskProcessAreaUserCustomerService _taskProcessAreaUserCustomerService;

        public TaskProcessAreaUserCustomerController(TaskProcessAreaUserCustomerService taskProcessAreaUserCustomerService)
        {
            _taskProcessAreaUserCustomerService = taskProcessAreaUserCustomerService;
        }

        [HttpGet("Get")]
        public ActionResult<List<TaskProcessAreaUserCustomer>> Get()
        {
            var taskProcessAreaUserCustomer = _taskProcessAreaUserCustomerService.GetAll();
            if (taskProcessAreaUserCustomer == null || !taskProcessAreaUserCustomer.Any())
            {
                return NotFound("No taskProcessAreaUserCustomer found");
            }
            return Ok(taskProcessAreaUserCustomer);
        }

        [HttpPost("Add")]
        public ActionResult Add([FromBody] TaskProcessAreaUserCustomer taskProcessAreaUserCustomer)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var success = _taskProcessAreaUserCustomerService.Add(taskProcessAreaUserCustomer);
            if (success)
            {
                return Ok("taskProcessAreaUserCustomer was added");
            }
            return BadRequest("Failed to add taskProcessAreaUserCustomer");
        }

        [HttpPut("{id}")]
        public ActionResult Update(Guid id, [FromBody] TaskProcessAreaUserCustomer taskProcessAreaUserCustomer)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            bool success = _taskProcessAreaUserCustomerService.Update(id, taskProcessAreaUserCustomer);
            if (success)
            {
                return Ok();
            }
            return BadRequest("Failed to update taskProcessAreaUserCustomer");
        }

        [HttpDelete("{id}")]

        public ActionResult Delete(Guid id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            bool success = _taskProcessAreaUserCustomerService.Delete(id);
            if (success)
            {
                return Ok();
            }

            return BadRequest("Failed to delete taskProcessAreaUserCustomer");
        }

        public IActionResult Index()
        {
            return View();
        }
    }
}

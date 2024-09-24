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

        [HttpGet("GetAll")]
        public ActionResult<List<TaskProcessAreaUserCustomer>> GetAll()
        {
            var tpauc = _taskProcessAreaUserCustomerService.GetAll();
            if (tpauc == null || !tpauc.Any())
            {
                return NotFound("Nothing found");
            }
            return Ok(tpauc);
        }

        [HttpPost("Add")]
        public ActionResult Add([FromBody] TaskProcessAreaUserCustomer tpauc)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var success = _taskProcessAreaUserCustomerService.Add(tpauc);
            if (success)
            {
                return Ok("It was added");
            }
            return BadRequest("Failed to add it");
        }

        [HttpPut("{id}")]
        public ActionResult Update(Guid guid, [FromBody] TaskProcessAreaUserCustomer tpauc)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            bool success = _taskProcessAreaUserCustomerService.Update(guid, tpauc);
            if (success)
            {
                return Ok();
            }
            return BadRequest("Failed to update");
        }

        [HttpDelete("{id}")]

        public ActionResult Delete(Guid guid)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            bool success = _taskProcessAreaUserCustomerService.Delete(guid);
            if (success)
            {
                return Ok();
            }

            return BadRequest("Failed to delete");
        }

        public IActionResult Index()
        {
            return View();
        }
    }
}

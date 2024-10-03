using Microsoft.AspNetCore.Mvc;
using PMSCRM.Models;
using PMSCRM.Services;

namespace PMSCRM.Controllers
{
    [Route("[controller]")]
    public class TaskProcessAreaController : Controller
    {
        TaskProcessAreaService _taskProcessAreaService;

        public TaskProcessAreaController(TaskProcessAreaService taskProcessAreaService)
        {
            _taskProcessAreaService = taskProcessAreaService;
        }

        [HttpGet("GetAll")]
        public ActionResult<List<TaskProcessArea>> GetAll()
        {
            var taskProcessArea = _taskProcessAreaService.GetAll();
            if (taskProcessArea == null || !taskProcessArea.Any())
            {
                return NotFound("No taskProcessAreas found");
            }
            return Ok(taskProcessArea);
        }

        [HttpPost("Add")]
        public ActionResult Add([FromBody] TaskProcessArea taskProcessArea)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var success = _taskProcessAreaService.Add(taskProcessArea);
            if (success)
            {
                return Ok("taskProcessArea was added");
            }
            return BadRequest("Failed to add taskProcessArea");
        }

        [HttpPut("{id}")]
        public ActionResult Update(Guid guid, [FromBody] TaskProcessArea taskProcessArea)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            bool success = _taskProcessAreaService.Update(guid, taskProcessArea);
            if (success)
            {
                return Ok();
            }
            return BadRequest("Failed to update taskProcessArea");
        }

        [HttpDelete("{id}")]
        public ActionResult Delete(Guid guid)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            bool success = _taskProcessAreaService.Delete(guid);
            if (success)
            {
                return Ok();
            }

            return BadRequest("Failed to delete taskProcessArea");
        }

        public IActionResult Index()
        {
            return View();
        }
    }
}

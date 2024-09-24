using Microsoft.AspNetCore.Mvc;
using PMSCRM.Models;
using PMSCRM.Services;

namespace PMSCRM.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TaskProcessAreaController : Controller
    {
        TaskProcessAreaService _taskProcessAreaService;

        public TaskProcessAreaController(TaskProcessAreaService taskProcessAreaService)
        {
            _taskProcessAreaService = taskProcessAreaService;
        }

        [HttpGet("GetTaskProcessAreas")]
        public ActionResult<List<TaskProcessArea>> GetTaskProcessAreas()
        {
            var taskProcessArea = _taskProcessAreaService.GetTaskProcessAreas();
            if (taskProcessArea == null || !taskProcessArea.Any())
            {
                return NotFound("No taskProcessAreas found");
            }
            return Ok(taskProcessArea);
        }

        [HttpPost("AddTaskProcessArea")]
        public ActionResult AddTaskProcessArea([FromBody] TaskProcessArea taskProcessArea)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var success = _taskProcessAreaService.AddTaskProcessArea(taskProcessArea);
            if (success)
            {
                return Ok("taskProcessArea was added");
            }
            return BadRequest("Failed to add taskProcessArea");
        }

        [HttpPut("{id}")]
        public ActionResult UpdateTaskProcessArea(Guid id, [FromBody] TaskProcessArea taskProcessArea)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            bool success = _taskProcessAreaService.UpdateTaskProcessArea(id, taskProcessArea);
            if (success)
            {
                return Ok();
            }
            return BadRequest("Failed to update taskProcessArea");
        }

        [HttpDelete("{id}")]

        public ActionResult DeleteTaskProcessArea(Guid id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            bool success = _taskProcessAreaService.DeleteTaskProcessArea(id);
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

using Microsoft.AspNetCore.Mvc;
using PMSCRM.Services;

namespace PMSCRM.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TaskController : Controller
    {
        TaskService _taskService;

        public TaskController(TaskService taskService)
        {
            _taskService = taskService;
        }

        [HttpGet("GetTasks")]
        public ActionResult<List<Models.Task>> GetTasks()
        {
            var tasks = _taskService.GetTasks();
            if (tasks == null || !tasks.Any())
            {
                return NotFound("No tasks found");
            }

            return Ok(tasks);

        }

        [HttpPost("AddTask")] 
        public ActionResult AddTask([FromBody] Models.Task task)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            bool success = _taskService.AddTask(task);
            if (success)
            {
                return Ok("Task was added");
            }
            return BadRequest("Failed to add task");
        }

        [HttpPut("{id}")] 
        public ActionResult UpdateTask (Guid id, [FromBody] Models.Task task)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            bool success = _taskService.UpdateTask(id, task);
            if (success)
            {
                return Ok();
            }
            return BadRequest("Failed to update task");
        }

        [HttpDelete("{id}")]
        public ActionResult DeleteTask(Guid id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            bool success = _taskService.DeleteTask(id);
            if (success)
            {
                return Ok();
            }

            return BadRequest("Failed to delete task");
        }

        public IActionResult Index()
        {
            return View();
        }
    }
}

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

        [HttpGet("GetAll")]
        public ActionResult<List<Models.Task>> GetAll()
        {
            var tasks = _taskService.GetAll();
            if (tasks == null || !tasks.Any())
            {
                return NotFound("No tasks found");
            }

            return Ok(tasks);
        }

        [HttpPost("Add")]
        public ActionResult Add([FromBody] Models.Task task)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            bool success = _taskService.Add(task);
            if (success)
            {
                return Ok("Task was added");
            }
            return BadRequest("Failed to add task");
        }

        [HttpPut("{id}")] 
        public ActionResult Update (Guid guid, [FromBody] Models.Task task)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            bool success = _taskService.Update(guid, task);
            if (success)
            {
                return Ok();
            }
            return BadRequest("Failed to update task");
        }

        [HttpDelete("{id}")]
        public ActionResult Delete(Guid guid)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            bool success = _taskService.Delete(guid);
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

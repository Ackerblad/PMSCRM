using Microsoft.AspNetCore.Mvc;
using PMSCRM.Models;
using PMSCRM.Services;

namespace PMSCRM.Controllers
{
    [Route("[controller]")]
    //[ApiController]
    public class TaskController : Controller
    {
        TaskService _taskService;

        public TaskController(TaskService taskService)
        {
            _taskService = taskService;
        }

        //[HttpGet("GetAll")]
        //public ActionResult<List<Models.Task>> GetAll()
        //{
        //    var tasks = _taskService.GetAll();
        //    if (tasks == null || !tasks.Any())
        //    {
        //        return NotFound("No tasks found");
        //    }

        //    return Ok(tasks);
        //}

        
        //[HttpPost("Add")]
        //public ActionResult Add([FromBody] Models.Task task)
        //{
        //    if (!ModelState.IsValid)
        //    {
        //        return BadRequest(ModelState);
        //    }

        //    bool success = _taskService.Add(task);
        //    if (success)
        //    {

        //        return Ok("Task was added");

        //    }
        //    return BadRequest("Failed to add task");
        //}

        [HttpPost]
        public ActionResult AddTask(Models.Task task)
        {
            if (!ModelState.IsValid)
            {
                return View(task);
            }

            bool success = _taskService.Add(task);
            if (success)
            {
                return RedirectToAction("Success");
            }

            ModelState.AddModelError(string.Empty, "Failed to add task");
            return View(task);
        }

        //[HttpPut("{id}")] 
        //public ActionResult Update (Guid guid, [FromBody] Models.Task task)
        //{
        //    if (!ModelState.IsValid)
        //    {
        //        return BadRequest(ModelState);
        //    }

        //    bool success = _taskService.Update(guid, task);
        //    if (success)
        //    {
        //        return Ok();
        //    }
        //    return BadRequest("Failed to update task");
        //}

        // GET: Task/EditTask/{id}
        [HttpGet("EditTask/{id}")]
        public IActionResult EditTask(Guid id)
        {
            var task = _taskService.GetById(id);
            if (task == null)
            {
                return NotFound("Task not found");
            }
            return View(task);
        }

        // POST: Task/EditTask/{id}
        [HttpPost("EditTask/{id}")]
        public IActionResult EditTask(Guid id, Models.Task updatedTask)
        {
            if(!ModelState.IsValid)
            {
                return View(updatedTask);
            } 

            bool success = _taskService.Update(id, updatedTask);
            if (success)
            {
                return RedirectToAction("ViewTasks");
            }

            ModelState.AddModelError(string.Empty, "Failed to update task.");
            return View("updatedTask");
        }



        //[HttpDelete("{id}")]
        //public ActionResult Delete(Guid guid)
        //{
        //    if (!ModelState.IsValid)
        //    {
        //        return BadRequest(ModelState);
        //    }

        //    bool success = _taskService.Delete(guid);
        //    if (success)
        //    {
        //        return Ok();
        //    }

        //    return BadRequest("Failed to delete task");
        //}

        // GET: Task/DeleteTask/{id}
        [HttpGet("DeleteTask/{id}")]
        public IActionResult DeleteTask(Guid id)
        {
            var task = _taskService.GetById(id);
            if (task == null)
            {
                return NotFound("Task not found");
            }

            return View(task);
        }

        [HttpPost("DeleteConfirmed/{id}")]
        public IActionResult DeleteConfirmed(Guid id)
        {
            var task = _taskService.GetById(id);
            if (task == null)
            {
                return NotFound("Task not found.");
            }

            bool success = _taskService.Delete(id);
            if (success)
            {
                TempData["SuccessMessage"] = "Task deleted successfully!";
                return RedirectToAction("ViewTasks");
            }

            ModelState.AddModelError(string.Empty, "Failed to delete task.");
            return View("DeleteTask", task);
        }


        public IActionResult AddTask()
        {
            return View();
        }

        [HttpGet("Success")]
        public IActionResult Success()
        {
            return View();
        }

        [HttpGet("ViewTasks")]
        public IActionResult ViewTasks()
        {
            var tasks = _taskService.GetAll();
            return View(tasks);
        }

        [HttpGet("EditTask")]
        public IActionResult EditTask()
        {
            return View();
        }

        [HttpGet("DeleteTask")] 
        public IActionResult DeleteTask()
        {
            return View();
        }

    }
}

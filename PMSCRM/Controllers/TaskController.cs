using Microsoft.AspNetCore.Mvc;
using PMSCRM.Models;
using PMSCRM.Services;
using PMSCRM.Utilities;

namespace PMSCRM.Controllers
{
    [Route("[controller]")]
    public class TaskController : Controller
    {
        private readonly TaskService _taskService;
        private readonly CompanyDivider _companyDivider;

        public TaskController(TaskService taskService, CompanyDivider companyDivider)
        {
            _taskService = taskService;
            _companyDivider = companyDivider;
        }

        [HttpPost]
        public ActionResult AddTask(Models.Task task)
        {
            if (!ModelState.IsValid)
            {
                return View(task);
            }

            task.CompanyId = _companyDivider.GetCompanyId();

            bool success = _taskService.Add(task);
            if (success)
            {
                return RedirectToAction("Success");
            }

            ModelState.AddModelError(string.Empty, "Failed to add task");
            return View(task);
        }

        [HttpGet("EditTask/{id}")]
        public IActionResult EditTask(Guid id)
        {
            var companyId = _companyDivider.GetCompanyId();
            var task = _taskService.GetById(id, companyId);

            if (task == null)
            {
                return NotFound("Task not found");
            }
            return View(task);
        }

        [HttpPost("EditTask/{id}")]
        public IActionResult EditTask(Guid id, Models.Task updatedTask)
        {
            if(!ModelState.IsValid)
            {
                return View(updatedTask);
            }

            updatedTask.CompanyId = _companyDivider.GetCompanyId();

            bool success = _taskService.Update(id, updatedTask);
            if (success)
            {
                return RedirectToAction("ViewTasks");
            }

            ModelState.AddModelError(string.Empty, "Failed to update task.");
            return View("updatedTask");
        }

        [HttpGet("DeleteTask/{id}")]
        public IActionResult DeleteTask(Guid id)
        {
            var companyId = _companyDivider.GetCompanyId();
            var task = _taskService.GetById(id, companyId);

            if (task == null)
            {
                return NotFound("Task not found");
            }

            return View(task);
        }

        [HttpPost("DeleteConfirmed/{id}")]
        public IActionResult DeleteConfirmed(Guid id)
        {
            var companyId = _companyDivider.GetCompanyId();
            var task = _taskService.GetById(id, companyId);

            if (task == null)
            {
                return NotFound("Task not found.");
            }

            bool success = _taskService.Delete(id, companyId);
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
            var companyId = _companyDivider.GetCompanyId();
            var tasks = _taskService.GetAll(companyId);
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

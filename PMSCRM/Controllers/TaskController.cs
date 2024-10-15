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
        public async Task<IActionResult> AddTask(Models.Task task)
        {
            if (!ModelState.IsValid)
            {
                return View(task);
            }

            task.CompanyId = _companyDivider.GetCompanyId();

            bool success = await _taskService.AddAsync(task);
            if (success)
            {
                return RedirectToAction("Success");
            }

            ModelState.AddModelError(string.Empty, "Failed to add task");
            return View(task);
        }

        [HttpGet("EditTask/{id}")]
        public async Task<IActionResult> EditTask(Guid id)
        {
            var companyId = _companyDivider.GetCompanyId();
            var task = await _taskService.GetByIdAsync(id, companyId);

            if (task == null)
            {
                return NotFound("Task not found");
            }
            return View(task);
        }

        [HttpPost("EditTask/{id}")]
        public async Task<IActionResult> EditTask(Guid id, Models.Task updatedTask)
        {
            if(!ModelState.IsValid)
            {
                return View(updatedTask);
            }

            updatedTask.CompanyId = _companyDivider.GetCompanyId();

            bool success = await _taskService.UpdateAsync(id, updatedTask);
            if (success)
            {
                return RedirectToAction("ViewTasks");
            }

            ModelState.AddModelError(string.Empty, "Failed to update task.");
            return View("updatedTask");
        }

        [HttpGet("DeleteTask/{id}")]
        public async Task<IActionResult> DeleteTask(Guid id)
        {
            var companyId = _companyDivider.GetCompanyId();
            var task = await _taskService.GetByIdAsync(id, companyId);

            if (task == null)
            {
                return NotFound("Task not found");
            }

            return View(task);
        }

        [HttpPost("DeleteConfirmed/{id}")]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            var companyId = _companyDivider.GetCompanyId();
            var task = await _taskService.GetByIdAsync(id, companyId);

            if (task == null)
            {
                return NotFound("Task not found.");
            }

            bool success = await _taskService.DeleteAsync(id, companyId);
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
        public async Task<IActionResult> ViewTasks()
        {
            var companyId = _companyDivider.GetCompanyId();
            var tasks = await _taskService.GetAllAsync(companyId);
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

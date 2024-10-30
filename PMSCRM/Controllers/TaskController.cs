using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PMSCRM.Models;
using PMSCRM.Services;
using PMSCRM.Utilities;

namespace PMSCRM.Controllers
{
    [Authorize]
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

        [HttpGet]
        public IActionResult AddTask()
        {
            ViewBag.IsEditMode = false;
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> AddTask(Models.Task task)
        {
            ViewBag.IsEditMode = false;
            if (!ModelState.IsValid) return View(task);

            task.CompanyId = _companyDivider.GetCompanyId();
            if (task.CompanyId == Guid.Empty) return View(task);

            bool success = await _taskService.AddAsync(task);
            if (!success)
            {
                ViewBag.Message = "Failed to add task. Please try again.";
                ViewBag.MessageType = "error";
                return View(task);
            }

            ViewBag.Message = "Task added successfully!";
            ViewBag.MessageType = "success";
            return View("AddTask", task);
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
            ViewBag.IsEditMode = true;
            return View("AddTask",task);
        }

        [HttpPost("EditTask/{id}")]
        public async Task<IActionResult> EditTask(Guid id, Models.Task updatedTask)
        {
            if (!ModelState.IsValid)
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

        [HttpGet("Details/{id}")]
        public async Task<IActionResult> Details(Guid id)
        {
            var companyId = _companyDivider.GetCompanyId();
            var task = await _taskService.GetByIdAsync(id, companyId);

            if (task == null)
            {
                ViewBag.Message = "Task not found.";
                return View("ViewTasks", task);
            }

            return View(task);
        }

        [HttpGet("ViewTasks")]
        public async Task<IActionResult> ViewTasks(string sortBy, string sortDirection = "asc")
        {
            var companyId = _companyDivider.GetCompanyId();
            var tasks = await _taskService.GetAllAsync(companyId);

            tasks = tasks.SortBy(sortBy, sortDirection).ToList();

            ViewBag.CurrentSort = sortBy;
            ViewBag.CurrentSortDirection = sortDirection;
            return View(tasks);
        }
    }
}

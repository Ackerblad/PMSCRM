using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using PMSCRM.Models;
using PMSCRM.Services;
using PMSCRM.Utilities;
using PMSCRM.ViewModels;

namespace PMSCRM.Controllers
{
    [Authorize]
    [Route("[controller]")]
    public class TaskProcessAreaController : Controller
    {
        private readonly TaskProcessAreaService _taskProcessAreaService;
        private readonly CompanyDivider _companyDivider;
        private readonly TaskService _taskService;
        private readonly ProcessService _processService;
        private readonly AreaService _areaService;


        public TaskProcessAreaController(
            TaskProcessAreaService taskProcessAreaService,
            TaskService taskService,
            ProcessService processService,
            AreaService areaService,
            CompanyDivider companyDivider)
        {
            _taskProcessAreaService = taskProcessAreaService;
            _taskService = taskService;
            _processService = processService;
            _areaService = areaService;
            _companyDivider = companyDivider;
        }

        [HttpGet("AddTaskProcessArea")]
        public async Task<IActionResult> AddTaskProcessArea()
        {
            var companyId = _companyDivider.GetCompanyId();

            var tasks = await _taskService.GetAllAsync(companyId);
            var processes = await _processService.GetAllAsync(companyId);
            var areas = await _areaService.GetAllAsync(companyId); // Assuming you want to fetch areas too

            var model = new TaskProcessAreaViewModel
            {
                Tasks = tasks.Select(t => new SelectListItem
                {
                    Value = t.TaskId.ToString(),
                    Text = t.Name,
                })
                .OrderBy(t => t.Text) // Sort tasks by name
                .ToList(), // Convert to a List for consistency

                Processes = processes.Select(p => new SelectListItem
                {
                    Value = p.ProcessId.ToString(),
                    Text = p.Name
                })
                .OrderBy(p => p.Text) // Sort processes by name
                .ToList(), // Convert to a List for consistency

                Areas = areas.Select(a => new SelectListItem // Ensure areas are also populated
                {
                    Value = a.AreaId.ToString(),
                    Text = a.Name
                })
                .OrderBy(a => a.Text) // Sort areas by name
                .ToList() // Convert to a List for consistency
            };

            return View(model);
        }


        [HttpPost("AddTaskProcessArea")]
        public async Task<IActionResult> AddTaskProcessArea(TaskProcessAreaViewModel model)
        {
            var companyId = _companyDivider.GetCompanyId();
            var process = await _processService.GetByIdAsync(model.ProcessId, companyId);

            var taskProcessArea = new TaskProcessArea
            {
                TaskId = model.TaskId,
                ProcessId = model.ProcessId,
                AreaId = process.AreaId,
                CompanyId = companyId,
            };

            bool success = await _taskProcessAreaService.AddAsync(taskProcessArea);
            if (success)
            {
                return RedirectToAction("ViewTaskProcessAreas");
            }

            ModelState.AddModelError(string.Empty, "Failed to add task-process-area.");


            model.Tasks = (await _taskService.GetAllAsync(companyId))
        .Select(t => new SelectListItem { Value = t.TaskId.ToString(), Text = t.Name });

            model.Processes = (await _processService.GetAllAsync(companyId))
                .Select(p => new SelectListItem { Value = p.ProcessId.ToString(), Text = p.Name });
            return View(model);
        }

        [HttpGet("EditTaskProcessArea/{id}")]
        public async Task<IActionResult> EditTaskProcessArea(Guid id)
        {
            var companyId = _companyDivider.GetCompanyId();
            var taskProcessArea = await _taskProcessAreaService.GetByIdAsync(id, companyId);

            if (taskProcessArea == null)
            {
                return NotFound("Task Process Area not found");
            }

            var tasks = await _taskService.GetAllAsync(companyId);
            var processes = await _processService.GetAllAsync(companyId);

            var model = new TaskProcessAreaViewModel
            {
                TaskProcessAreaId = taskProcessArea.TaskProcessAreaId,
                TaskId = taskProcessArea.TaskId,
                ProcessId = taskProcessArea.ProcessId,
                Tasks = tasks.Select(t => new SelectListItem
                {
                    Value = t.TaskId.ToString(),
                    Text = t.Name,
                    Selected = t.TaskId == taskProcessArea.TaskId
                }),
                Processes = processes.Select(p => new SelectListItem
                {
                    Value = p.ProcessId.ToString(),
                    Text = p.Name,
                    Selected = p.ProcessId == taskProcessArea.ProcessId
                }),
            };

            return View(model);
        }

        [HttpPost("EditTaskProcessArea/{id}")]
        public async Task<IActionResult> EditTaskProcessArea(TaskProcessAreaViewModel model)
        {
            var taskProcessArea = new TaskProcessArea
            {
                TaskProcessAreaId = model.TaskProcessAreaId,
                TaskId = model.TaskId,
                ProcessId = model.ProcessId,
                CompanyId = _companyDivider.GetCompanyId(),
            };

            bool success = await _taskProcessAreaService.UpdateAsync(taskProcessArea);
            if (success)
            {
                return RedirectToAction("ViewTaskProcessAreas");
            }

            ModelState.AddModelError(string.Empty, "Failed to update Task Process Area.");
            return View(model);
        }

        [HttpGet("DeleteTaskProcessArea/{id}")]
        public async Task<IActionResult> DeleteTaskProcessArea(Guid id)
        {
            var companyId = _companyDivider.GetCompanyId();
            var taskProcessArea = await _taskProcessAreaService.GetByIdAsync(id, companyId);

            if (taskProcessArea == null)
            {
                return NotFound("Task Process Area not found.");
            }

            var model = new TaskProcessAreaDisplayViewModel
            {
                TaskProcessAreaId = taskProcessArea.TaskProcessAreaId,
                TaskName = taskProcessArea.Task?.Name,
                ProcessName = taskProcessArea.Process?.Name,
                Timestamp = taskProcessArea.Timestamp
            };

            return View(model);
        }

        [HttpPost("DeleteConfirmed/{id}")]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            var companyId = _companyDivider.GetCompanyId();
            var taskProcessArea = await _taskProcessAreaService.GetByIdAsync(id, companyId);

            if (taskProcessArea == null)
            {
                return NotFound("Task Process Area not found.");
            }

            bool success = await _taskProcessAreaService.DeleteAsync(id, companyId);
            if (success)
            {
                TempData["SuccessMessage"] = "Task Process Area deleted successfully!";
                return RedirectToAction("ViewTaskProcessAreas");
            }

            ModelState.AddModelError(string.Empty, "Failed to delete Task Process Area.");
            return View("DeleteTaskProcessArea", taskProcessArea);
        }

        [HttpGet("ViewTaskProcessAreas")]
        public IActionResult ViewTaskProcessAreas()
        {
            var taskProcessAreas = _taskProcessAreaService.GetAllWithDetails();
            var model = taskProcessAreas.Select(tpa => new TaskProcessAreaDisplayViewModel
            {
                TaskProcessAreaId = tpa.TaskProcessAreaId,
                TaskName = tpa.Task?.Name,
                ProcessName = tpa.Process?.Name,
                Timestamp = tpa.Timestamp
            }).ToList();

            return View(model);
        }

        public IActionResult AddTaskProcessAreas()
        {
            return View();
        }

        [HttpGet("Success")]
        public IActionResult Success()
        {
            return View();
        }

        [HttpGet("EditTaskProcessArea")]
        public IActionResult EditTaskProcessArea()
        {
            return View();
        }

        [HttpGet("DeleteTaskProcessArea")]
        public IActionResult DeleteTaskProcessArea()
        {
            return View();
        }
    }
}

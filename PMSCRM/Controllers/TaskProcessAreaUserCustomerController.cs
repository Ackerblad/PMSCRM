using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using PMSCRM.Models;
using PMSCRM.Services;
using PMSCRM.Utilities;
using PMSCRM.ViewModels;
using System.Diagnostics;
using System.Security.Claims;

namespace PMSCRM.Controllers
{
    [Route("[controller]")]
    [Authorize]
    public class TaskProcessAreaUserCustomerController : Controller
    {
        TaskProcessAreaUserCustomerService _taskProcessAreaUserCustomerService;
        private readonly CompanyDivider _companyDivider;
        private readonly TaskService _taskService;
        private readonly ProcessService _processService;
        private readonly AreaService _areaService;
        private readonly UserService _userService;
        private readonly CustomerService _customerService;
        private readonly TaskProcessAreaService _taskProcessAreaService;

        public TaskProcessAreaUserCustomerController(TaskProcessAreaUserCustomerService taskProcessAreaUserCustomerService,
                                                     TaskService taskService, ProcessService processService,
                                                     AreaService areaService, UserService userService,
                                                     CustomerService customerService, CompanyDivider companyDivider, TaskProcessAreaService taskProcessAreaService)
        {
            _taskProcessAreaUserCustomerService = taskProcessAreaUserCustomerService;
            _companyDivider = companyDivider;
            _taskService = taskService;
            _processService = processService;
            _areaService = areaService;
            _userService = userService;
            _customerService = customerService;
            _taskProcessAreaService = taskProcessAreaService;
        }

        [HttpGet("AddTaskProcessAreaUserCustomer")]
        public async Task<IActionResult> AddTaskProcessAreaUserCustomer()
        {
            var companyId = _companyDivider.GetCompanyId();

            var allTaskProcessAreas = await _taskProcessAreaUserCustomerService.GetAllWithDetailsAsync();
            var existingConnections = await _taskProcessAreaUserCustomerService.GetAllWithDetailsAsync();

            var users = await _userService.GetAllAsync(companyId);
            var customers = await _customerService.GetAllAsync(companyId);

            var model = new TaskProcessAreaUserCustomerViewModel
            {
                ExistingConnections = existingConnections.ToList(),
                TaskProcessAreas = allTaskProcessAreas,

                Users = users.Select(u => new SelectListItem
                {
                    Value = u.UserId.ToString(),
                    Text = u.FirstName + " " + u.LastName
                }).ToList(),

                Customers = customers.Select(c => new SelectListItem
                {
                    Value = c.CustomerId.ToString(),
                    Text = c.Name
                }).ToList(),

                Statuses = Enum.GetValues(typeof(Utilities.TaskStatus))
                              .Cast<Utilities.TaskStatus>()
                              .Select(s => new SelectListItem
                              {
                                  Value = ((byte)s).ToString(),
                                  Text = s.ToString()
                              })
                              .ToList()
            };

            return View(model);
        }

        [HttpPost("AddTaskProcessAreaUserCustomer")]
        public async Task<IActionResult> AddTaskProcessAreaUserCustomer(Guid taskProcessAreaId, TaskProcessAreaUserCustomerViewModel model)
        {
            var companyId = _companyDivider.GetCompanyId();

            var newConnection = new TaskProcessAreaUserCustomer
            {
                TaskProcessAreaId = taskProcessAreaId,
                UserId = model.UserId,
                CustomerId = model.CustomerId,
                StartDate = model.StartDate,
                EndDate = model.EndDate,
                Status = (byte)model.Status,
                CompanyId = companyId
            };

            bool success = await _taskProcessAreaUserCustomerService.AddAsync(new List<TaskProcessAreaUserCustomer> { newConnection });

            if (success)
            {
                return RedirectToAction("Success");
            }

            ModelState.AddModelError(string.Empty, "Failed to add task-process-area-user-customer.");

            return await AddTaskProcessAreaUserCustomer();
        }

        //     [HttpGet("EditTaskProcessAreaUserCustomer/{id}")]
        //     public async Task<IActionResult> EditTaskProcessAreaUserCustomer(Guid id)
        //     {
        //         var companyId = _companyDivider.GetCompanyId();
        //         var taskProcessAreaUserCustomer = await _taskProcessAreaUserCustomerService.GetByIdAsync(id, companyId);

        //         if (taskProcessAreaUserCustomer == null)
        //         {
        //             return NotFound("Task Process Area User Customer not found");
        //         }

        //var areas = await _areaService.GetAllAsync(companyId);
        //var processes = await _processService.GetAllAsync(companyId);
        //var tasks = await _taskService.GetAllAsync(companyId);
        //var users = await _userService.GetAllAsync(companyId);
        //var customers = await _customerService.GetAllAsync(companyId);

        //var model = new TaskProcessAreaViewModel
        //         {
        //             TaskProcessAreaId = taskProcessArea.TaskProcessAreaId,
        //             TaskId = taskProcessArea.TaskId,
        //             ProcessId = taskProcessArea.ProcessId,
        //             AreaId = taskProcessArea.AreaId,
        //	Areas = areas.Select(a => new SelectListItem
        //	{
        //		Value = a.AreaId.ToString(),
        //		Text = a.Name,
        //		Selected = a.AreaId == taskProcessArea.AreaId
        //	}),
        //	Processes = processes.Select(p => new SelectListItem
        //	{
        //		Value = p.ProcessId.ToString(),
        //		Text = p.Name,
        //		Selected = p.ProcessId == taskProcessArea.ProcessId
        //	}),
        //	Tasks = tasks.Select(t => new SelectListItem
        //             {
        //                 Value = t.TaskId.ToString(),
        //                 Text = t.Name,
        //                 Selected = t.TaskId == taskProcessArea.TaskId
        //             }),


        //         };

        //         return View(model);
        //     }

        //     [HttpPost("EditTaskProcessAreaUserCustomer/{id}")]
        //     public async Task<IActionResult> EditTaskProcessAreaUserCustomer(TaskProcessAreaUserCustomerViewModel model)
        //     {
        //         //if (!ModelState.IsValid)
        //         //{
        //         //    return View(model);
        //         //}

        //         var taskProcessArea = new TaskProcessArea
        //         {
        //             TaskProcessAreaId = model.TaskProcessAreaId,
        //             TaskId = model.TaskId,
        //             ProcessId = model.ProcessId,
        //             AreaId = model.AreaId,
        //             CompanyId = _companyDivider.GetCompanyId(),
        //         };

        //         bool success = await _taskProcessAreaService.UpdateAsync(taskProcessArea);
        //         if (success)
        //         {
        //             return RedirectToAction("ViewTaskProcessAreas");
        //         }

        //         ModelState.AddModelError(string.Empty, "Failed to update Task Process Area.");
        //         return View(model);
        //     }

        //[HttpGet("DeleteTaskProcessArea/{id}")]
        //public async Task<IActionResult> DeleteTaskProcessArea(Guid id)
        //{
        //    var companyId = _companyDivider.GetCompanyId();
        //    var taskProcessArea = await _taskProcessAreaService.GetByIdAsync(id, companyId);

        //    if (taskProcessArea == null)
        //    {
        //        return NotFound("Task Process Area not found.");
        //    }

        //    var model = new TaskProcessAreaDisplayViewModel
        //    {
        //        TaskProcessAreaId = taskProcessArea.TaskProcessAreaId,
        //        TaskName = taskProcessArea.Task?.Name,
        //        ProcessName = taskProcessArea.Process?.Name,
        //        AreaName = taskProcessArea.Area?.Name,
        //        Timestamp = taskProcessArea.Timestamp
        //    };

        //    return View(model);
        //}

        //[HttpPost("DeleteConfirmed/{id}")]
        //public async Task<IActionResult> DeleteConfirmed(Guid id)
        //{
        //    var companyId = _companyDivider.GetCompanyId();
        //    var taskProcessArea = await _taskProcessAreaService.GetByIdAsync(id, companyId);

        //    if (taskProcessArea == null)
        //    {
        //        return NotFound("Task Process Area not found.");
        //    }

        //    bool success = await _taskProcessAreaService.DeleteAsync(id, companyId);
        //    if (success)
        //    {
        //        TempData["SuccessMessage"] = "Task Process Area deleted successfully!";
        //        return RedirectToAction("ViewTaskProcessAreas");
        //    }

        //    ModelState.AddModelError(string.Empty, "Failed to delete Task Process Area.");
        //    return View("DeleteTaskProcessArea", taskProcessArea);
        //}

        //[HttpGet("ViewTaskProcessAreaUserCustomer")]
        //public async Task<IActionResult> ViewTaskProcessAreaUserCustomer()
        //{
        //    // Await the asynchronous method to get the task process area user customer details
        //    var taskProcessAreaUserCustomer = await _taskProcessAreaUserCustomerService.GetAllWithDetailsAsync();

        //    // Ensure you are selecting from the correct type; 
        //    // assuming taskProcessAreaUserCustomer is a collection of a specific type
        //    var model = taskProcessAreaUserCustomer.Select(tpauc => new TaskProcessAreaUserCustomerDisplayViewModel
        //    {
        //        TaskName = tpauc.TaskProcessArea.Task.Name,
        //        ProcessName = tpauc.TaskProcessArea.Process.Name,
        //        AreaName = tpauc.TaskProcessArea.Area.Name,
        //        UserName = tpauc.User.FirstName,
        //        CustomerName = tpauc.Customer.Name,
        //        StartDate = tpauc.StartDate,
        //        EndDate = tpauc.EndDate,
        //        Status = tpauc.Status
        //    }).ToList();

        //    return View(model);
        //}

        [HttpGet("ViewTaskProcessAreaUserCustomerForUser")]
        [Authorize(Roles = "User")]
        public async Task<IActionResult> ViewTaskProcessAreaUserCustomerForUser()
        {
            // Get the logged-in user's ID from the claims
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (string.IsNullOrEmpty(userId))
            {
                // If for some reason the user ID is null or empty, return an error or redirect.
                return Unauthorized();
            }

            // Call the service function with the current user's ID
            var tpaucRecords = await _taskProcessAreaUserCustomerService.GetAllWithDetailsToDisplayForUserAsync(Guid.Parse(userId));

            // Pass the tpaucRecords to the view for rendering
            return View(tpaucRecords);
        }


        [HttpGet("ViewTaskProcessAreaUserCustomer")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> ViewTaskProcessAreaUserCustomer()
        {
            var tpauc = await _taskProcessAreaUserCustomerService.GetAllWithDetailsToDisplayAsync();

            return View("ViewTaskProcessAreaUserCustomer", tpauc);
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

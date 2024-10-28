using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
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
        private readonly UserService _userService;
        private readonly CustomerService _customerService;

        public TaskProcessAreaUserCustomerController(TaskProcessAreaUserCustomerService taskProcessAreaUserCustomerService,
                                                     UserService userService,CustomerService customerService, CompanyDivider companyDivider)
        {
            _taskProcessAreaUserCustomerService = taskProcessAreaUserCustomerService;
            _companyDivider = companyDivider;
            _userService = userService;
            _customerService = customerService;
        }

        [HttpGet("AddTaskProcessAreaUserCustomer")]
        public async Task<IActionResult> AddTaskProcessAreaUserCustomer(string sortBy = "Area", string sortDirection = "asc")
        {
            var companyId = _companyDivider.GetCompanyId();

            var allTaskProcessAreas = await _taskProcessAreaUserCustomerService.GetAllWithDetailsAsync();
            var existingConnections = await _taskProcessAreaUserCustomerService.GetAllWithDetailsAsync();

            switch (sortBy)
            {
                case "Area":
                    existingConnections = sortDirection == "asc"
                        ? existingConnections.OrderBy(t => t.AreaName).ToList()
                        : existingConnections.OrderByDescending(t => t.AreaName).ToList();
                    break;
                case "Process":
                    existingConnections = sortDirection == "asc"
                        ? existingConnections.OrderBy(t => t.ProcessName).ToList()
                        : existingConnections.OrderByDescending(t => t.ProcessName).ToList();
                    break;
                case "Task":
                    existingConnections = sortDirection == "asc"
                        ? existingConnections.OrderBy(t => t.TaskName).ToList()
                        : existingConnections.OrderByDescending(t => t.TaskName).ToList();
                    break;
                default:
                    existingConnections = existingConnections.OrderBy(t => t.AreaName).ToList();
                    break;
            }

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
                })
                .OrderBy(u => u.Text)
                .ToList(),
                Customers = customers.Select(c => new SelectListItem
                {
                    Value = c.CustomerId.ToString(),
                    Text = c.Name
                })
                .OrderBy(c => c.Text)
                .ToList(),
                Statuses = Enum.GetValues(typeof(Utilities.TaskStatus))
                              .Cast<Utilities.TaskStatus>()
                              .Select(s => new SelectListItem
                              {
                                  Value = ((byte)s).ToString(),
                                  Text = s.ToString()
                              })
                              .OrderBy(s => s.Text)
                              .ToList()
            };

            ViewBag.CurrentSort = sortBy;
            ViewBag.CurrentSortDirection = sortDirection;

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
                return RedirectToAction("ViewTaskProcessAreaUserCustomer");
            }

            ModelState.AddModelError(string.Empty, "Failed to add task-process-area-user-customer.");

            return await AddTaskProcessAreaUserCustomer();
        }

        [HttpGet("EditTaskProcessAreaUserCustomer/{id}")]
        public async Task<IActionResult> EditTaskProcessAreaUserCustomer(Guid id)
        {
            var companyId = _companyDivider.GetCompanyId();
            var tpauc = await _taskProcessAreaUserCustomerService.GetByIdAsync(id, companyId);

            if (tpauc == null)
            {
                return NotFound("Task Process Area User Customer not found");
            }

            var users = await _userService.GetAllAsync(companyId);
            var customers = await _customerService.GetAllAsync(companyId);
            var allTaskProcessAreas = await _taskProcessAreaUserCustomerService.GetAllWithDetailsAsync();

            var model = new TaskProcessAreaUserCustomerViewModel
            {
                TaskProcessAreaId = tpauc.TaskProcessAreaId,
                UserId = tpauc.UserId,
                CustomerId = tpauc.CustomerId,
                StartDate = tpauc.StartDate,
                EndDate = tpauc.EndDate,
                Status = (System.Threading.Tasks.TaskStatus)(Utilities.TaskStatus)tpauc.Status,
                Users = users.Select(u => new SelectListItem
                {
                    Value = u.UserId.ToString(),
                    Text = u.FirstName + " " + u.LastName,
                    Selected = (u.UserId == tpauc.UserId)
                }).ToList(),
                Customers = customers.Select(c => new SelectListItem
                {
                    Value = c.CustomerId.ToString(),
                    Text = c.Name,
                    Selected = (c.CustomerId == tpauc.CustomerId)
                }).ToList(),
                Statuses = Enum.GetValues(typeof(PMSCRM.Utilities.TaskStatus))
                               .Cast<PMSCRM.Utilities.TaskStatus>()
                               .Select(s => new SelectListItem
                               {
                                   Value = ((byte)s).ToString(),
                                   Text = s.ToString(),
                                   Selected = (s == (PMSCRM.Utilities.TaskStatus)tpauc.Status)
                               }).ToList(),

                ExistingConnections = allTaskProcessAreas,
                IsEditMode = true
            };

            return View("AddTaskProcessAreaUserCustomer", model);
        }

        [HttpPost("EditTaskProcessAreaUserCustomer/{id}")]
        public async Task<IActionResult> EditTaskProcessAreaUserCustomer(Guid id, TaskProcessAreaUserCustomerViewModel model)
        {
            var taskProcessAreaUserCustomer = new TaskProcessAreaUserCustomer
            {
                TaskProcessAreaUserCustomerId = id,
                TaskProcessAreaId = model.TaskProcessAreaId,
                UserId = model.UserId,
                CustomerId = model.CustomerId,
                StartDate = model.StartDate,
                EndDate = model.EndDate,
                Status = (byte)model.Status
            };

            bool success = await _taskProcessAreaUserCustomerService.UpdateAsync(taskProcessAreaUserCustomer);

            if (success)
            {
                return RedirectToAction("ViewTaskProcessAreaUserCustomer");
            }

            ModelState.AddModelError(string.Empty, "Failed to update Task Process Area User Customer.");
            return View("AddTaskProcessAreaUserCustomer", model);
        }

        [HttpPost("DeleteTaskProcessAreaUserCustomer/{id}")]
        public async Task<IActionResult> DeleteTaskProcessAreaUserCustomer(Guid id)
        {
            var companyId = _companyDivider.GetCompanyId();
            var tpauc = await _taskProcessAreaUserCustomerService.GetByIdAsync(id, companyId);

            if (tpauc == null)
            {
                return RedirectToAction("ViewTaskProcessAreaUserCustomer"); 
            }

            bool success = await _taskProcessAreaUserCustomerService.DeleteAsync(id, companyId);
            return RedirectToAction("ViewTaskProcessAreaUserCustomer"); 
        }

        //[HttpGet("ViewTaskProcessAreaUserCustomerForUser")]
        //[Authorize(Roles = "User")]
        //public async Task<IActionResult> ViewTaskProcessAreaUserCustomerForUser()
        //{
        //    // Get the logged-in user's ID from the claims
        //    var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

        //    if (string.IsNullOrEmpty(userId))
        //    {
        //        // If for some reason the user ID is null or empty, return an error or redirect.
        //        return Unauthorized();
        //    }

        //    // Call the service function with the current user's ID
        //    var tpaucRecords = await _taskProcessAreaUserCustomerService.GetAllWithDetailsToDisplayForUserAsync(Guid.Parse(userId));

        //    // Pass the tpaucRecords to the view for rendering
        //    return View(tpaucRecords);
        //}


        //[HttpGet("ViewTaskProcessAreaUserCustomer")]
        //[Authorize(Roles = "Admin")]
        //public async Task<IActionResult> ViewTaskProcessAreaUserCustomer()
        //{
        //    var tpauc = await _taskProcessAreaUserCustomerService.GetAllWithDetailsToDisplayAsync();

        //    return View("ViewTaskProcessAreaUserCustomer", tpauc);
        //}

        [HttpGet("ViewTaskProcessAreaUserCustomerForUser")]
        [Authorize(Roles = "User")]
        public async Task<IActionResult> ViewTaskProcessAreaUserCustomerForUser(string sortBy, string sortDirection)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (string.IsNullOrEmpty(userId))
            {
                return Unauthorized();
            }

            var tpaucRecords = await _taskProcessAreaUserCustomerService.GetAllWithDetailsToDisplayForUserAsync(Guid.Parse(userId));

            ViewBag.CurrentSort = sortBy ?? "Area";
            ViewBag.CurrentSortDirection = sortDirection ?? "asc";

            switch (ViewBag.CurrentSort)
            {
                case "Area":
                    tpaucRecords = ViewBag.CurrentSortDirection == "asc"
                        ? tpaucRecords.OrderBy(r => r.AreaName).ToList()
                        : tpaucRecords.OrderByDescending(r => r.AreaName).ToList();
                    break;
                case "Process":
                    tpaucRecords = ViewBag.CurrentSortDirection == "asc"
                        ? tpaucRecords.OrderBy(r => r.ProcessName).ToList()
                        : tpaucRecords.OrderByDescending(r => r.ProcessName).ToList();
                    break;
                case "Task":
                    tpaucRecords = ViewBag.CurrentSortDirection == "asc"
                        ? tpaucRecords.OrderBy(r => r.TaskName).ToList()
                        : tpaucRecords.OrderByDescending(r => r.TaskName).ToList();
                    break;
                case "Customer":
                    tpaucRecords = ViewBag.CurrentSortDirection == "asc"
                        ? tpaucRecords.OrderBy(r => r.CustomerName).ToList()
                        : tpaucRecords.OrderByDescending(r => r.CustomerName).ToList();
                    break;
                case "StartDate":
                    tpaucRecords = ViewBag.CurrentSortDirection == "asc"
                        ? tpaucRecords.OrderBy(r => r.StartDate).ToList()
                        : tpaucRecords.OrderByDescending(r => r.StartDate).ToList();
                    break;
                case "EndDate":
                    tpaucRecords = ViewBag.CurrentSortDirection == "asc"
                        ? tpaucRecords.OrderBy(r => r.EndDate).ToList()
                        : tpaucRecords.OrderByDescending(r => r.EndDate).ToList();
                    break;
                case "Status":
                    tpaucRecords = ViewBag.CurrentSortDirection == "asc"
                        ? tpaucRecords.OrderBy(r => r.Status).ToList()
                        : tpaucRecords.OrderByDescending(r => r.Status).ToList();
                    break;
                case "Timestamp":
                    tpaucRecords = ViewBag.CurrentSortDirection == "asc"
                        ? tpaucRecords.OrderBy(r => r.Timestamp).ToList()
                        : tpaucRecords.OrderByDescending(r => r.Timestamp).ToList();
                    break;
                default:
                    tpaucRecords = tpaucRecords.OrderBy(r => r.AreaName).ToList();
                    break;
            }

            return View(tpaucRecords);
        }

        [HttpGet("ViewTaskProcessAreaUserCustomer")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> ViewTaskProcessAreaUserCustomer(string sortBy, string sortDirection)
        {
            var tpauc = await _taskProcessAreaUserCustomerService.GetAllWithDetailsToDisplayAsync();

            ViewBag.CurrentSort = sortBy ?? "Area";
            ViewBag.CurrentSortDirection = sortDirection ?? "asc";

            switch (ViewBag.CurrentSort)
            {
                case "Area":
                    tpauc = ViewBag.CurrentSortDirection == "asc"
                        ? tpauc.OrderBy(r => r.AreaName).ToList()
                        : tpauc.OrderByDescending(r => r.AreaName).ToList();
                    break;
                case "Process":
                    tpauc = ViewBag.CurrentSortDirection == "asc"
                        ? tpauc.OrderBy(r => r.ProcessName).ToList()
                        : tpauc.OrderByDescending(r => r.ProcessName).ToList();
                    break;
                case "Task":
                    tpauc = ViewBag.CurrentSortDirection == "asc"
                        ? tpauc.OrderBy(r => r.TaskName).ToList()
                        : tpauc.OrderByDescending(r => r.TaskName).ToList();
                    break;
                case "User":
                    tpauc = ViewBag.CurrentSortDirection == "asc"
                        ? tpauc.OrderBy(r => r.UserName).ToList()
                        : tpauc.OrderByDescending(r => r.UserName).ToList();
                    break;
                case "Customer":
                    tpauc = ViewBag.CurrentSortDirection == "asc"
                        ? tpauc.OrderBy(r => r.CustomerName).ToList()
                        : tpauc.OrderByDescending(r => r.CustomerName).ToList();
                    break;
                case "StartDate":
                    tpauc = ViewBag.CurrentSortDirection == "asc"
                        ? tpauc.OrderBy(r => r.StartDate).ToList()
                        : tpauc.OrderByDescending(r => r.StartDate).ToList();
                    break;
                case "EndDate":
                    tpauc = ViewBag.CurrentSortDirection == "asc"
                        ? tpauc.OrderBy(r => r.EndDate).ToList()
                        : tpauc.OrderByDescending(r => r.EndDate).ToList();
                    break;
                case "Status":
                    tpauc = ViewBag.CurrentSortDirection == "asc"
                        ? tpauc.OrderBy(r => r.Status).ToList()
                        : tpauc.OrderByDescending(r => r.Status).ToList();
                    break;
                case "Timestamp":
                    tpauc = ViewBag.CurrentSortDirection == "asc"
                        ? tpauc.OrderBy(r => r.Timestamp).ToList()
                        : tpauc.OrderByDescending(r => r.Timestamp).ToList();
                    break;
                default:
                    tpauc = tpauc.OrderBy(r => r.AreaName).ToList();
                    break;
            }

            return View("ViewTaskProcessAreaUserCustomer", tpauc);
        }


        public IActionResult AddTaskProcessAreas()
        {
            return View();
        }
    }
}

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
    }
}

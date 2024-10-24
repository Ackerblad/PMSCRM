using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PMSCRM.Models;
using PMSCRM.Services;
using PMSCRM.Utilities;
using PMSCRM.ViewModels;

namespace PMSCRM.Controllers
{
    [Authorize]
    [Route("[controller]")]
    public class ResultsController : Controller
    {
        private readonly CompanyDivider _companyDivider;
        private readonly AreaService _areaService;
        private readonly CustomerService _customerService;
        private readonly ProcessService _processService;
        private readonly TaskService _taskService;
        private readonly UserService _userService;

        public ResultsController(CompanyDivider companyDivider, 
            AreaService areaService, 
            CustomerService customerService, 
            ProcessService processService, 
            TaskService taskService, 
            UserService userService)
        {
            _companyDivider = companyDivider;
            _areaService = areaService;
            _customerService = customerService;
            _processService = processService;
            _taskService = taskService;
            _userService = userService;
        }

        [HttpGet]
        public async Task<IActionResult> Index(string query, string filters, string sortBy, string sortDirection = "asc")
        {
            var companyId = _companyDivider.GetCompanyId();

            if (companyId == Guid.Empty)
            {
                return BadRequest("Company not found.");
            }

            // Split the filters into an array (if any filters were provided)
            var filterArray = !string
                .IsNullOrEmpty(filters) ? filters
                .Split(',')
                .Select(f => f
                .ToLower())
                .ToList() : new List<string>();



            // Initialize filtered results variables
            List<Area> filteredAreas = new List<Area>();
            List<Customer> filteredCustomers = new List<Customer>();
            List<Process> filteredProcesses = new List<Process>();
            List<Models.Task> filteredTasks = new List<Models.Task>();
            List<User> filteredUsers = new List<User>();


            if (string.IsNullOrWhiteSpace(query))
            {
                var emptyViewModel = new SearchResultsViewModel
                {
                    Areas = filteredAreas,
                    Customers = filteredCustomers,
                    Processes = filteredProcesses,
                    Tasks = filteredTasks,
                    Users = filteredUsers,
                    FilterArray = filterArray,
                    Query = query
                };

                return View(emptyViewModel); // Return an empty view model
            }

            bool searchAll = !filterArray.Any();

            // Filtering logic based on selected types (Area, Customer, Process, Task, User)
            if (searchAll || filterArray.Contains("area"))
            {
                filteredAreas = await _areaService.SearchAreasAsync(companyId, query);
            }

            if (searchAll || filterArray.Contains("customer"))
            {
                filteredCustomers = await _customerService.SearchCustomersAsync(companyId, query);
            }

            if (searchAll || filterArray.Contains("process"))
            {
                filteredProcesses = await _processService.SearchProcessesAsync(companyId, query);
            }

            if (searchAll || filterArray.Contains("task"))
            {
                filteredTasks = await _taskService.SearchTasksAsync(companyId, query);
            }

            if (searchAll || filterArray.Contains("user"))
            {
                filteredUsers = await _userService.SearchUsersAsync(companyId, query);
            }


            // Create view model with the filtered results
            var viewModel = new SearchResultsViewModel
            {
                Areas = filteredAreas,
                Customers = filteredCustomers,
                Processes = filteredProcesses,
                Tasks = filteredTasks,
                Users = filteredUsers,
                FilterArray = filterArray,
                Query = query
            };

            return View(viewModel);
        }




        [HttpPost]
        public async Task<IActionResult> SortResults(SearchResultsViewModel model, string sortBy, string sortDirection = "asc")
        {
            //if (string.IsNullOrEmpty(model.Query))
            //{
            //    // Handle the scenario where the query is null
            //    return BadRequest("Query is null or empty. Unable to sort results.");
            //}


            if (model.Areas == null || !model.Areas.Any())
            {
                var companyId = _companyDivider.GetCompanyId();
                if (companyId == Guid.Empty)
                {
                    return BadRequest("Company not found.");
                }

                //// Re-fetch the data based on the current filters and query
                //var query = model.Query; // Assuming you store the query in the model
                //var filters = model.FilterArray; // Assuming you store filters in the model

                //// Initialize filtered results
                //var filteredAreas = await _areaService.SearchAreasAsync(companyId, model.Query);

                //// Update the model with the fetched data
                //model.Areas = filteredAreas;
            }
            // Sort Areas
            if (model.Areas.Any())
            {
                model.Areas = sortDirection == "asc"
                    ? model.Areas.OrderBy(a => EF.Property<string>(a, sortBy)).ToList()
                    : model.Areas.OrderByDescending(a => EF.Property<string>(a, sortBy)).ToList();
            }

            // Sort Customers
            if (model.Customers.Any())
            {
                model.Customers = sortDirection == "asc"
                    ? model.Customers.OrderBy(c => EF.Property<string>(c, sortBy)).ToList()
                    : model.Customers.OrderByDescending(c => EF.Property<string>(c, sortBy)).ToList();
            }

            // Sort Processes
            if (model.Processes.Any())
            {
                model.Processes = sortDirection == "asc"
                    ? model.Processes.OrderBy(p => EF.Property<string>(p, sortBy)).ToList()
                    : model.Processes.OrderByDescending(p => EF.Property<string>(p, sortBy)).ToList();
            }

            // Sort Tasks
            if (model.Tasks.Any())
            {
                model.Tasks = sortDirection == "asc"
                    ? model.Tasks.OrderBy(t => EF.Property<string>(t, sortBy)).ToList()
                    : model.Tasks.OrderByDescending(t => EF.Property<string>(t, sortBy)).ToList();
            }

            // Sort Users
            if (model.Users.Any())
            {
                model.Users = sortDirection == "asc"
                    ? model.Users.OrderBy(u => EF.Property<string>(u, sortBy)).ToList()
                    : model.Users.OrderByDescending(u => EF.Property<string>(u, sortBy)).ToList();
            }
            
            // Pass current sorting info to the view (used for toggling sort direction)
            ViewBag.CurrentSort = sortBy;
            ViewBag.CurrentSortDirection = sortDirection;

            return View("Index", model);
        }


    }
}


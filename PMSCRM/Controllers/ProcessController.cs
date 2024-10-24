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
    public class ProcessController : Controller
    {
        private readonly ProcessService _processService;
        private readonly CompanyDivider _companyDivider;
        private readonly AreaService _areaService;

        public ProcessController(ProcessService processService, CompanyDivider companyDivider, AreaService areaService)
        {
            _processService = processService;
            _companyDivider = companyDivider;
            _areaService = areaService;
        }

        [HttpGet]
        public async Task<IActionResult> AddProcess()
        {
            var companyId = _companyDivider.GetCompanyId();

            var areas = await _areaService.GetAllAsync(companyId);

            var viewModel = new ProcessViewModel
            {
                Areas = areas.Select(a => new SelectListItem
                {
                    Value = a.AreaId.ToString(),
                    Text = a.Name
                }).ToList()
            };

            return View(viewModel);
        }

        [HttpPost]
        public async Task<IActionResult> AddProcess(ProcessViewModel viewModel)
        {
            if (!ModelState.IsValid)
            {
                var areas = await _areaService.GetAllAsync(_companyDivider.GetCompanyId());
                viewModel.Areas = areas.Select(a => new SelectListItem
                {
                    Value = a.AreaId.ToString(),
                    Text = a.Name
                }).ToList();

                return View(viewModel);
            }

            var process = viewModel.Process;
            process.CompanyId = _companyDivider.GetCompanyId();
            process.Duration = 0;

            bool success = await _processService.AddAsync(process);
            if (success)
            {
                return RedirectToAction("ViewProcesses");
            }

            ModelState.AddModelError(string.Empty, "Failed to add process.");
            return View(viewModel); 
        }

        [HttpGet("EditProcess/{id}")]
        public async Task<IActionResult> EditProcess(Guid id)
        {
            var companyId = _companyDivider.GetCompanyId();
            var process = await _processService.GetByIdAsync(id, companyId);

            if (process == null)
            {
                return NotFound("Process not found.");
            }

            var areas = await _areaService.GetAllAsync(companyId);
            var areaSelectList = areas.Select(a => new SelectListItem
            {
                Value = a.AreaId.ToString(),
                Text = a.Name
            }).ToList();

            var viewModel = new ProcessViewModel
            {
                Process = process,
                Areas = areaSelectList
            };

            return View(viewModel);
        }

        [HttpPost("EditProcess/{id}")]
        public async Task<IActionResult> EditProcess(Guid id, ProcessViewModel viewModel)
        {
            if (!ModelState.IsValid)
            {
                var areas = await _areaService.GetAllAsync(_companyDivider.GetCompanyId());
                viewModel.Areas = areas.Select(a => new SelectListItem
                {
                    Value = a.AreaId.ToString(),
                    Text = a.Name
                }).ToList();

                return View(viewModel);
            }

            viewModel.Process.CompanyId = _companyDivider.GetCompanyId();

            bool success = await _processService.UpdateAsync(id, viewModel.Process);
            if (success)
            {
                return RedirectToAction("ViewProcesses");
            }

            ModelState.AddModelError(string.Empty, "Failed to update process.");
            var areasFailed = await _areaService.GetAllAsync(_companyDivider.GetCompanyId());
            viewModel.Areas = areasFailed.Select(a => new SelectListItem
            {
                Value = a.AreaId.ToString(),
                Text = a.Name
            }).ToList();

            return View(viewModel);
        }

        [HttpGet("DeleteProcess/{id}")]
        public async Task<IActionResult> DeleteProcess(Guid id)
        {
            var companyId = _companyDivider.GetCompanyId();
            var process = await _processService.GetByIdAsync(id, companyId);
            if (process == null)
            {
                return NotFound("Process not found.");
            }

            return View(process);
        }

        [HttpPost("DeleteConfirmed/{id}")]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            var companyId = _companyDivider.GetCompanyId();
            var process = await _processService.GetByIdAsync(id, companyId);

            if (process == null)
            {
                return NotFound("Process not found.");
            }

            bool success = await _processService.DeleteAsync(id, companyId);
            if (success)
            {
                TempData["SuccessMessage"] = "Process deleted successfully!";
                return RedirectToAction("ViewProcesses");
            }

            ModelState.AddModelError(string.Empty, "Failed to delete process.");
            return View("DeleteProcess", process);
        }

        [HttpGet("ViewProcesses")]
        public async Task<IActionResult> ViewProcesses()
        {
            var companyId = _companyDivider.GetCompanyId();
            var processes = await _processService.GetAllAsync(companyId);
            return View(processes);
        }

        [HttpGet("Success")]
        public IActionResult Success()
        {
            return View();
        }

        [HttpGet("EditProcess")]
        public IActionResult EditProcess()
        {
            return View();
        }

        [HttpGet("DeleteProcess")]
        public IActionResult DeleteProcess()
        {
            return View();
        }

        [HttpGet("Details/{id}")]
        public async Task<IActionResult> Details(Guid id)
        {
            var companyId = _companyDivider.GetCompanyId();
            var process = await _processService.GetByIdAsync(id, companyId);

            if (process == null)
            {
                return NotFound("Process not found.");
            }

            return View(process);
        }
    }
}

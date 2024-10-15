using Microsoft.AspNetCore.Mvc;
using PMSCRM.Models;
using PMSCRM.Services;
using PMSCRM.Utilities;
using System.Data;

namespace PMSCRM.Controllers
{
    [Route("[controller]")]
    public class ProcessController : Controller
    {
        ProcessService _processService;
        private readonly CompanyDivider _companyDivider;

        public ProcessController(ProcessService processService, CompanyDivider companyDivider)
        {
            _processService = processService;
            _companyDivider = companyDivider;
        }

        [HttpPost]
        public async Task<IActionResult> AddProcess(Process process)
        {
            if (!ModelState.IsValid)
            {
                return View(process);
            }
            process.CompanyId = _companyDivider.GetCompanyId();


            bool success = await _processService.AddAsync(process);
            if (success)
            {
                return RedirectToAction("ViewProcesses");
            }

            ModelState.AddModelError(string.Empty, "Failed to add process.");
            return View(process);
        }

        [HttpGet("EditProcess/{id}")]
        public async Task<IActionResult> EditProcess(Guid id)
        {
            var companyId = _companyDivider.GetCompanyId();
            var process = _processService.GetByIdAsync(id, companyId);
            if (process == null)
            {
                return NotFound("Process not found.");
            }
            return View(process);
        }

        // POST: /Process/EditProcess/{id}
        [HttpPost("EditProcess/{id}")]
        public async Task<IActionResult> EditProcess(Guid id, Process updatedProcess)
        {
            if (!ModelState.IsValid)
            {
                return View(updatedProcess);
            }
            updatedProcess.CompanyId = _companyDivider.GetCompanyId();

            bool success = await _processService.UpdateAsync(id, updatedProcess);
            if (success)
            {
                return RedirectToAction("ViewProcesses");
            }

            ModelState.AddModelError(string.Empty, "Failed to update process.");
            return View(updatedProcess);
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

        // POST: /Process/DeleteConfirmed/{id}
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

        public IActionResult AddProcess()
        {
            return View();
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
            var process = await _processService.GetByIdAsync(id, companyId); // Fetch task by ID and company

            if (process == null)
            {
                return NotFound("Process not found.");
            }

            return View(process); // Return the task to the view
        }


    }
}

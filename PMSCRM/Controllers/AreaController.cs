using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PMSCRM.Models;
using PMSCRM.Services;
using PMSCRM.Utilities;

namespace PMSCRM.Controllers
{
    [Authorize]
    [Route("[controller]")]
    public class AreaController : Controller
    {
        private readonly AreaService _areaService;
        private readonly CompanyDivider _companyDivider;

        public AreaController(AreaService areaService, CompanyDivider companyDivider)
        {
            _areaService = areaService;
            _companyDivider = companyDivider;
        }


        [HttpPost]
        public async Task<IActionResult> AddAreaAsync(Area area)
        {
            if (!ModelState.IsValid)
            {
                return View(area);
            }

            area.CompanyId = _companyDivider.GetCompanyId();

            if (area.CompanyId == Guid.Empty)
            {
                ModelState.AddModelError(string.Empty, "Unable to find company information for the logged-in user.");
                return View(area);
            }

            bool success = await _areaService.AddAsync(area);
            if (success)
            {
                return RedirectToAction("Success");
            }

            ModelState.AddModelError(string.Empty, "Failed to add area");
            return View(area);
        }

        [HttpGet("EditArea/{id}")]
        public async Task<ActionResult> EditArea(Guid id)
        {
            var companyId = _companyDivider.GetCompanyId();
            var area = await _areaService.GetByIdAsync(id, companyId);
            if (area == null)
            {
                return NotFound("Area not found");
            }
            return View(area);
        }

        [HttpPost("EditArea/{id}")]
        public async Task<IActionResult> EditAreaAsync(Guid id, Area updatedArea)
        {
            if (!ModelState.IsValid)
            {
                return View(updatedArea);
            }

            updatedArea.CompanyId = _companyDivider.GetCompanyId();

            bool success = await _areaService.UpdateAsync(id, updatedArea);
            if (success)
            {
                return RedirectToAction("ViewAreas");
            }

            ModelState.AddModelError(string.Empty, "Failed to update area.");
            return View("updatedArea");
        }

        [HttpGet("DeleteArea/{id}")]
        public async Task<IActionResult> DeleteArea(Guid id)
        {
            var companyId = _companyDivider.GetCompanyId();
            var area = await _areaService.GetByIdAsync(id, companyId);

            if (area == null)
            {
                return NotFound("Area not found");
            }

            return View(area);
        }

        [HttpPost("DeleteConfirmed/{id}")]
        public async Task<IActionResult> DeleteConfirmedAsync(Guid id)
        {
            var companyId = _companyDivider.GetCompanyId();
            var area = await _areaService.GetByIdAsync(id, companyId);
            if (area == null)
            {
                return NotFound("Area not found.");
            }

            bool success = await _areaService.DeleteAsync(id, companyId);
            if (success)
            {
                TempData["SuccessMessage"] = "Area deleted successfully!";
                return RedirectToAction("ViewAreas");
            }

            ModelState.AddModelError(string.Empty, "Failed to delete area.");
            return View("DeleteArea", area);
        }

        public IActionResult AddArea()
        {
            return View();
        }

        [HttpGet("Success")]
        public IActionResult Success()
        {
            return View();
        }

        [HttpGet("ViewAreas")]
        public async Task<IActionResult> ViewAreasAsync(string sortBy, string sortDirection = "asc")
        {
            var companyId = _companyDivider.GetCompanyId();
            var areas = await _areaService.GetAllAsync(companyId);

            areas = areas.SortBy(sortBy, sortDirection).ToList();

            // Pass current sorting info to the view (used for toggling sort direction)
            ViewBag.CurrentSort = sortBy;
            ViewBag.CurrentSortDirection = sortDirection;

            return View(areas);
        }
        [HttpGet("EditArea")]
        public IActionResult EditArea()
        {
            return View();
        }
        [HttpGet("DeleteArea")]
        public IActionResult DeleteArea()
        {
            return View();
        }

    }
}

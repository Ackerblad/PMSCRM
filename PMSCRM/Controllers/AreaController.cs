﻿using Microsoft.AspNetCore.Authorization;
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

        [HttpGet]
        public IActionResult AddArea()
        {
            ViewBag.IsEditMode = false;
            return View(new Area());
        }

        private async Task<Area?> GetAreaAsync(Guid id)
        {
            var companyId = GetCompanyId();
            return companyId == Guid.Empty ? null : await _areaService.GetByIdAsync(id, companyId); 
        }

        [HttpPost]
        public async Task<IActionResult> AddAreaAsync(Area area)
        {
            if (!ModelState.IsValid) return View(area);

            area.CompanyId = GetCompanyId();
            if (area.CompanyId == Guid.Empty) return View(area);

            var success = await _areaService.AddAsync(area);
            if (!success)
            {
                ModelState.AddModelError("", "Failed to add area.");
                return View(area);
            }

            return RedirectToAction("Success");
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
            ViewBag.IsEditMode = true;
            return View("AddArea", area);
        }

        [HttpPost("EditArea/{id}")]
        public async Task<IActionResult> EditAreaAsync(Guid id, Area updatedArea)
        {
            if (!ModelState.IsValid) return View(updatedArea);

            updatedArea.CompanyId = GetCompanyId();
            if (updatedArea.CompanyId == Guid.Empty) return View(updatedArea);

            var success = await _areaService.UpdateAsync(id, updatedArea);
            if (!success)
            {
                ModelState.AddModelError(string.Empty, "Failed to update area.");
                return View("updatedArea");
            }
            return RedirectToAction("ViewAreas");
        }

        [HttpGet("DeleteArea/{id}")]
        public async Task<IActionResult> DeleteArea(Guid id)
        {
            var area = await GetAreaAsync(id);
            return area == null ? NotFound("Area not found") : View(area);
        }

        [HttpPost("DeleteConfirmed/{id}")]
        public async Task<IActionResult> DeleteConfirmedAsync(Guid id)
        {
            var companyId = _companyDivider.GetCompanyId();
            if (companyId == Guid.Empty) return RedirectToAction("DeleteArea", new { id });

            var area = await _areaService.GetByIdAsync(id, companyId);
            if (area == null)
            {
                ViewBag.Message = "Area not found.";
                return View("DeleteArea", area);
            }

            var success = await _areaService.DeleteAsync(id, companyId);
            ViewBag.Message = success ? "Area deleted successfully!" : "Failed to delete area. The area contains a process.";
            return success ? RedirectToAction("ViewAreas") : View("DeleteArea", area);
        }

        [HttpGet("Success")]
        public IActionResult Success() => View();

        [HttpGet("ViewAreas")]
        public async Task<IActionResult> ViewAreasAsync(string sortBy, string sortDirection = "asc")
        {
            var companyId = GetCompanyId();
            if (companyId == Guid.Empty) return View(new List<Area>());

            var areas = await _areaService.GetAllAsync(companyId);
            areas = areas.SortBy(sortBy, sortDirection).ToList();

            ViewBag.CurrentSort = sortBy;
            ViewBag.CurrentSortDirection = sortDirection;

            return View(areas);
        }
    }
}

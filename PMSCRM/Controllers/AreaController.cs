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

        private Guid GetCompanyId()
        {
            return _companyDivider.GetCompanyId();
        }

        private async Task<Area?> GetAreaAsync(Guid id)
        {
            var companyId = GetCompanyId();
            return companyId == Guid.Empty ? null : await _areaService.GetByIdAsync(id, companyId); 
        }

        [HttpGet]
        public IActionResult AddArea()
        {
            ViewBag.IsEditMode = false;
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> AddAreaAsync(Area area)
        {
            ViewBag.IsEditMode = false;
            if (!ModelState.IsValid) return View(area);

            area.CompanyId = GetCompanyId();
            if (area.CompanyId == Guid.Empty) return View(area);

            var success = await _areaService.AddAsync(area);
            if (!success)
            {
                ViewBag.Message = "Failed to add area. Please try again.";
                ViewBag.MessageType = "error";
                return View(area);
            }

            ViewBag.Message = "Area added successfully!";
            ViewBag.MessageType = "success";
            return View("AddArea", area);
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
            ViewBag.Message = success ? "Area deleted successfully!" : "This area contains one or more processes. Go to processes and change area of the connected process.";
            return success ? RedirectToAction("ViewAreas") : View("DeleteArea", area);
        }

        [HttpGet("Details/{id}")]
        public async Task<IActionResult> Details(Guid id)
        {
            var companyId = GetCompanyId();
            var area = await _areaService.GetByIdAsync(id, companyId);

            if (area == null)
            {
                ViewBag.Message = "Area not found.";
                return View("ViewAreas", area);
            }

            return View(area);
        }

        [HttpGet("ViewAreas")]
        public async Task<IActionResult> ViewAreasAsync(string sortBy = "TimeStamp", string sortDirection = "desc")
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

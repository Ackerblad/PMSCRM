using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PMSCRM.Models;
using PMSCRM.Services;
using System.Threading.Tasks;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace PMSCRM.Controllers
{
    [Route("[controller]")]
    public class AreaController : Controller
    {
        AreaService _areaService;


        public AreaController(AreaService areaService)
        {
            _areaService = areaService;
        }

        // Helper to retrieve CompanyId from logged-in user's claims
        private Guid GetUserCompanyId()
        {
            var claim = User.FindFirst("CompanyId");
            return claim != null ? Guid.Parse(claim.Value) : Guid.Empty;
        }

        [HttpGet("GetAll")]
        public async Task<IActionResult> GetAllAsync()
        {
            var areas = await _areaService.GetAllAsync();
            if (areas == null || !areas.Any())
            {
                return NotFound("No areas found");
            } 
            return Ok(areas);
        }

        [HttpPost]
        public async Task<IActionResult> AddAreaAsync(Area area)
        {
            if(!ModelState.IsValid)
            {
                return View(area);
            }

            var companyId = GetUserCompanyId(); 
            if (companyId == Guid.Empty)
            {
                ModelState.AddModelError(string.Empty, "Unable to find company information for the logged-in user.");
                return View(area);
            }

            area.CompanyId = companyId;

            bool success = await _areaService.AddAsync(area);
            if (success)
            {
                return RedirectToAction("Success");
            }

            ModelState.AddModelError(string.Empty, "Failed to add area");
            return View(area);
        }

        [HttpGet("EditArea/{id}")]
        public async Task<IActionResult> EditAreaAsync(Guid id)
        {
            var area = await _areaService.GetByIdAsync(id);
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

            bool success = await _areaService.UpdateAsync(id, updatedArea);
            if (success)
            {
                return RedirectToAction("ViewAreas");
            }

            ModelState.AddModelError(string.Empty, "Failed to update area.");
            return View("updatedArea");
        }

        [HttpGet("DeleteArea/{id}")]
        public async Task<IActionResult> DeleteAreaAsync(Guid id)
        {
            var area = await _areaService.GetByIdAsync(id);
            if (area == null)
            {
                return NotFound("Area not found");
            }

            return View(area);
        }

        [HttpPost("DeleteConfirmed/{id}")]
        public async Task<IActionResult> DeleteConfirmedAsync(Guid id)
        {
            var area = await _areaService.GetByIdAsync(id);
            if (area == null)
            {
                return NotFound("Area not found.");
            }

            bool success = await _areaService.DeleteAsync(id);
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
        public async Task<IActionResult> ViewAreasAsync()
        {
            var areas = await _areaService.GetAllAsync();
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

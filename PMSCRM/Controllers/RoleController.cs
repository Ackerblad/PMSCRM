using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PMSCRM.Models;
using PMSCRM.Services;
using PMSCRM.Utilities;

namespace PMSCRM.Controllers
{
    [Authorize]
    [Route("[controller]")]
    public class RoleController : Controller
    {
        private readonly RoleService _roleService;
        private readonly CompanyDivider _companyDivider;

        public RoleController(RoleService roleService, CompanyDivider companyDivider)
        {
            _roleService = roleService;
            _companyDivider = companyDivider;
        }

        [HttpPost]
        public async Task<IActionResult> AddRole(Role role)
        {
            if (!ModelState.IsValid)
            {
                return View(role);
            }
            role.CompanyId = _companyDivider.GetCompanyId();

            bool success = await _roleService.AddAsync(role);
            if (success)
            {
                return RedirectToAction("ViewRoles");
            }

            ModelState.AddModelError(string.Empty, "Failed to add role.");
            return View(role);
        }

        [HttpGet("EditRole/{id}")]
        public async Task<IActionResult> EditRole(Guid id)
        {
            var companyId = _companyDivider.GetCompanyId();
            var role = await _roleService.GetByIdAsync(id, companyId);
            if (role == null)
            {
                return NotFound("Role not found.");
            }
            return View(role);
        }

        // POST: /Process/EditProcess/{id}
        [HttpPost("EditRole/{id}")]
        public async Task<IActionResult> EditRole(Guid id, Role updatedRole)
        {
            if (!ModelState.IsValid)
            {
                return View(updatedRole);
            }

            updatedRole.CompanyId = _companyDivider.GetCompanyId();

            bool success = await _roleService.UpdateAsync(id, updatedRole);
            if (success)
            {
                return RedirectToAction("ViewRoles");
            }

            ModelState.AddModelError(string.Empty, "Failed to update role.");
            return View(updatedRole);
        }

        [HttpGet("DeleteRole/{id}")]
        public async Task<IActionResult> DeleteRole(Guid id)
        {
            var companyId = _companyDivider.GetCompanyId();
            var role = await _roleService.GetByIdAsync(id, companyId);
            if (role == null)
            {
                return NotFound("Role not found.");
            }

            return View(role);
        }

        // POST: /Process/DeleteConfirmed/{id}
        [HttpPost("DeleteConfirmed/{id}")]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            var companyId = _companyDivider.GetCompanyId();
            var role = await _roleService.GetByIdAsync(id, companyId);
            if (role == null)
            {
                return NotFound("Role not found.");
            }

            bool success = await _roleService.DeleteAsync(id, companyId);
            if (success)
            {
                TempData["SuccessMessage"] = "Role deleted successfully!";
                return RedirectToAction("ViewRoles");
            }

            ModelState.AddModelError(string.Empty, "Failed to delete role.");
            return View("DeleteRole", role);
        }

        [HttpGet("ViewRoles")]
        public async Task<IActionResult> ViewRoles()
        {
            var companyId = _companyDivider.GetCompanyId();
            var roles = await _roleService.GetAllAsync(companyId);
            return View(roles);
        }

        public IActionResult AddRole()
        {
            return View();
        }

        [HttpGet("Success")]
        public IActionResult Success()
        {
            return View();
        }

        [HttpGet("EditRole")]
        public IActionResult EditRole()
        {
            return View();
        }

        [HttpGet("DeleteRole")]
        public IActionResult DeleteRole()
        {
            return View();
        }
    }
}

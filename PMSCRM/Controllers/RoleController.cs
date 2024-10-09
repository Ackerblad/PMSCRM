using Microsoft.AspNetCore.Mvc;
using PMSCRM.Models;
using PMSCRM.Services;
using System.Data;

namespace PMSCRM.Controllers
{
    [Route("[controller]")]
    public class RoleController : Controller
    {
        RoleService _roleService;

        public RoleController(RoleService roleService)
        {
            _roleService = roleService;
        }

        [HttpGet("GetAll")]
        public ActionResult<List<Role>> GetAll()
        {
            var roles = _roleService.GetAll();
            if (roles == null || !roles.Any())
            {
                return NotFound("No roles found");
            }
            return Ok(roles);
        }

        //[HttpPost("Add")]
        //public ActionResult Add([FromBody] Role role)
        //{
        //    if (!ModelState.IsValid)
        //    {
        //        return BadRequest(ModelState);
        //    }

        //    var success = _roleService.Add(role);
        //    if (success)
        //    {
        //        return Ok("Role was added");
        //    }
        //    return BadRequest("Failed to add role");
        //}

        [HttpPost("AddRole")]
        public IActionResult AddRole(Role role)
        {
            if (!ModelState.IsValid)
            {
                return View(role);
            }

            bool success = _roleService.Add(role);
            if (success)
            {
                return RedirectToAction("ViewRoles");
            }

            ModelState.AddModelError(string.Empty, "Failed to add role.");
            return View(role);
        }

        //[HttpPut("{id}")]
        //public ActionResult Update(Guid guid, [FromBody] Role role)
        //{
        //    if (!ModelState.IsValid)
        //    {
        //        return BadRequest(ModelState);
        //    }

        //    bool success = _roleService.Update(guid, role);
        //    if (success)
        //    {
        //        return Ok();
        //    }
        //    return BadRequest("Failed to update role");
        //}

        [HttpGet("EditRole/{id}")]
        public IActionResult EditRole(Guid id)
        {
            var role = _roleService.GetById(id);
            if (role == null)
            {
                return NotFound("Role not found.");
            }
            return View(role);
        }

        // POST: /Process/EditProcess/{id}
        [HttpPost("EditRole/{id}")]
        public IActionResult EditRole(Guid id, Role updatedRole)
        {
            if (!ModelState.IsValid)
            {
                return View(updatedRole);
            }

            bool success = _roleService.Update(id, updatedRole);
            if (success)
            {
                return RedirectToAction("ViewRoles");
            }

            ModelState.AddModelError(string.Empty, "Failed to update role.");
            return View(updatedRole);
        }

        //[HttpDelete("{id}")]

        //public ActionResult Delete(Guid guid)
        //{
        //    if (!ModelState.IsValid)
        //    {
        //        return BadRequest(ModelState);
        //    }

        //    bool success = _roleService.Delete(guid);
        //    if (success)
        //    {
        //        return Ok();
        //    }

        //    return BadRequest("Failed to delete role");
        //}

        [HttpGet("DeleteRole/{id}")]
        public IActionResult DeleteRole(Guid id)
        {
            var role = _roleService.GetById(id);
            if (role == null)
            {
                return NotFound("Role not found.");
            }

            return View(role);
        }

        // POST: /Process/DeleteConfirmed/{id}
        [HttpPost("DeleteConfirmed/{id}")]
        public IActionResult DeleteConfirmed(Guid id)
        {
            var role = _roleService.GetById(id);
            if (role == null)
            {
                return NotFound("Role not found.");
            }

            bool success = _roleService.Delete(id);
            if (success)
            {
                TempData["SuccessMessage"] = "Role deleted successfully!";
                return RedirectToAction("ViewRoles");
            }

            ModelState.AddModelError(string.Empty, "Failed to delete role.");
            return View("DeleteRole", role);
        }

        [HttpGet("ViewRoles")]
        public IActionResult ViewRoles()
        {
            var roles = _roleService.GetAll();
            if (roles == null || roles.Count == 0)
            {
                TempData["InfoMessage"] = "No roles available.";
            }
            return View(roles);
        }

        [HttpGet("AddRole")]
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

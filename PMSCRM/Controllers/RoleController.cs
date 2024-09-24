using Microsoft.AspNetCore.Mvc;
using PMSCRM.Models;
using PMSCRM.Services;
using System.Data;

namespace PMSCRM.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RoleController : Controller
    {
        RoleService _roleService;

        public RoleController(RoleService roleService)
        {
            _roleService = roleService;
        }

        [HttpGet("GetRoles")]
        public ActionResult<List<Role>> GetRoles()
        {
            var roles = _roleService.GetRoles();
            if (roles == null || !roles.Any())
            {
                return NotFound("No roles found");
            }
            return Ok(roles);
        }

        [HttpPost("AddRole")]
        public ActionResult AddRole([FromBody] Role role)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var success = _roleService.AddRole(role);
            if (success)
            {
                return Ok("Role was added");
            }
            return BadRequest("Failed to add role");
        }

        [HttpPut("{id}")]
        public ActionResult UpdateRole(Guid id, [FromBody] Role role)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            bool success = _roleService.UpdateRole(id, role);
            if (success)
            {
                return Ok();
            }
            return BadRequest("Failed to update role");
        }

        [HttpDelete("{id}")]

        public ActionResult DeleteRole(Guid id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            bool success = _roleService.DeleteRole(id);
            if (success)
            {
                return Ok();
            }

            return BadRequest("Failed to delete role");
        }

        public IActionResult Index()
        {
            return View();
        }
    }
}

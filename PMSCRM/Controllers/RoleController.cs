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

        [HttpPost("Add")]
        public ActionResult Add([FromBody] Role role)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var success = _roleService.Add(role);
            if (success)
            {
                return Ok("Role was added");
            }
            return BadRequest("Failed to add role");
        }

        [HttpPut("{id}")]
        public ActionResult Update(Guid guid, [FromBody] Role role)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            bool success = _roleService.Update(guid, role);
            if (success)
            {
                return Ok();
            }
            return BadRequest("Failed to update role");
        }

        [HttpDelete("{id}")]

        public ActionResult Delete(Guid guid)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            bool success = _roleService.Delete(guid);
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

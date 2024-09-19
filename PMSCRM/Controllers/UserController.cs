using Microsoft.AspNetCore.Mvc;
using PMSCRM.Models;
using PMSCRM.Services;

namespace PMSCRM.Controllers
{


    [Route("api/[controller]")]
    [ApiController]
    public class UserController : Controller
    {
        UserService userService;

        public UserController(UserService userService)
        {
            this.userService = userService;
        }
        
        [HttpPost]
        public ActionResult AddUser([FromBody] User user)
        {
            if(!ModelState.IsValid)
            {
                return BadRequest(ModelState);

            }
            bool success = userService.AddUser(user);
            if (success)
            {
                return Ok("User added successfully");
            }
            return BadRequest("Failed to add user");

        }

        [HttpGet]
        public ActionResult<List<User>> GetUsers() 
        {
            var users = userService.GetUsers();
            if (users == null || !users.Any())
            {
                return NotFound("No users found");
            }
            return Ok(users);
        }

        [HttpPut("{id}")]
        public ActionResult UpdateUser(Guid id, [FromBody] User user)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);

            }

            bool sucess = userService.UpdateUser(id, user);
            if (sucess)
            {
                return Ok();
            }

            return BadRequest("Failed to update user");
        }

        [HttpDelete("{id}")]
        public ActionResult DeleteUser(Guid id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            bool success = userService.DeleteUser(id);
            if (success)
            {
                return Ok();
            }

            return BadRequest("Failed to delete user");
        }

        public IActionResult Index()
        {
            return View();
        }
    }
}

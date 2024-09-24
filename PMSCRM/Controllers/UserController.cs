using Microsoft.AspNetCore.Mvc;
using PMSCRM.Models;
using PMSCRM.Services;
using PMSCRM.Utilities;

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

        [HttpPost("add")]
        public ActionResult AddUser([FromBody] UserRegistration registration)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);

            }
            bool success = userService.AddUser(registration.CompanyId, registration.RoleId, registration.Username, registration.FirstName, registration.LastName,
                                               registration.PhoneNumber, registration.EmailAddress, registration.Password);

            if (success)
            {
                return Ok("User added successfully");
            }
            return BadRequest("Failed to add user");
        }

        [HttpPost("login")]
        public ActionResult Login(string username, string password)
        {
            var user = userService.AuthenticateUser(username, password);

            if (user != null)
            {
                return Ok("Login successful");
            }
            return Unauthorized("Invalid credentials");
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

        //[HttpPut("{id}")]
        //public ActionResult UpdateUser(Guid id, [FromBody] User user)
        //{
        //    if (!ModelState.IsValid)
        //    {
        //        return BadRequest(ModelState);

        //    }

        //    bool sucess = userService.UpdateUser(id, user);
        //    if (sucess)
        //    {
        //        return Ok();
        //    }

        //    return BadRequest("Failed to update user");
        //}

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

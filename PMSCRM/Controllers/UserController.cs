using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using PMSCRM.Models;
using PMSCRM.Services;
using PMSCRM.Utilities;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using System.Security.Claims;

namespace PMSCRM.Controllers
{
    [Route("[controller]")]
    public class UserController : Controller
    {
        UserService userService;

        public UserController(UserService userService)
        {
            this.userService = userService;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginRequest loginRequest)
        {
            var user = userService.AuthenticateUser(loginRequest.EmailAddress, loginRequest.Password);
            if (user != null)
            {        
                var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.Email, user.EmailAddress),
               
                };

                var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);

                var authProperties = new AuthenticationProperties
                {
                    IsPersistent = loginRequest.RememberMe, 
                };

                if (loginRequest.RememberMe)
                {
                    authProperties.ExpiresUtc = DateTime.UtcNow.AddDays(20);
                }
                else
                {
                    authProperties.ExpiresUtc = DateTime.UtcNow.AddMinutes(20);
                }

                await HttpContext.SignInAsync(
                    CookieAuthenticationDefaults.AuthenticationScheme,
                    new ClaimsPrincipal(claimsIdentity),
                    authProperties);

                return RedirectToAction("Index", "Home");
            }
            ViewBag.Message = "Invalid credentials";
            return View("~/Views/Login/Login.cshtml");
        }

        [HttpPost("add")]
        public ActionResult AddUser([FromBody] UserRegistration registration)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);

            }
            bool success = userService.AddUser(registration.CompanyId, registration.RoleId, registration.EmailAddress, registration.FirstName, registration.LastName,
                                               registration.PhoneNumber, registration.Password);

            if (success)
            {
                return Ok("User added successfully");
            }
            return BadRequest("Failed to add user");
        }

        [HttpPost("request-password-reset")]
        public ActionResult RequestPasswordReset([FromBody] PasswordResetRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            bool success = userService.GeneratePasswordToken(request.EmailAddress);

            if (success)
            {
                return Ok("Password reset link sent to your email.");
            }

            return BadRequest("Email not found");
        }

        [HttpPost("reset-password")]
        public ActionResult ResetPassword([FromBody] PasswordReset passwordReset)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            bool success = userService.ResetPassword(passwordReset.Token, passwordReset.NewPassword);

            if (success)
            {
                return Ok("Password has been reset successfully.");
            }

            return BadRequest("Invalid or expired token.");
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
        public ActionResult UpdateUser(Guid id, [FromBody] UserUpdate updatedUser)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);

            }

            bool sucess = userService.UpdateUser(id, updatedUser);
            if (sucess)
            {
                return Ok("User updated successfully");
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
    }
}

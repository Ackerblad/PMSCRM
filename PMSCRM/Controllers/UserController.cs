using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using PMSCRM.Models;
using PMSCRM.Services;
using PMSCRM.Utilities;
using System.Security.Claims;

namespace PMSCRM.Controllers
{
    [Route("[controller]")]
    public class UserController : Controller
    {
        private readonly UserService _userService;
        private readonly CompanyDivider _companyDivider;

        public UserController(UserService userService, CompanyDivider companyDivider)
        {
            _userService = userService;
            _companyDivider = companyDivider;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginRequest loginRequest)
        {
            var user = _userService.AuthenticateUser(loginRequest.EmailAddress, loginRequest.Password);
            if (user != null)
            {        
                var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.Email, user.EmailAddress),
                    new Claim("CompanyId", user.CompanyId.ToString())
               
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

        [HttpPost("AddUser")]
        public ActionResult AddUser(UserRegistration registration)
        {
            if (!ModelState.IsValid)
            {
                return View("AddUser");

            }

            var tempPassword = _userService.GenerateTemporaryPassword();

            var companyId = _companyDivider.GetCompanyId();

            bool success = _userService.AddUser(companyId, registration.RoleId, registration.EmailAddress, registration.FirstName, registration.LastName,
                                               registration.PhoneNumber, tempPassword);

            if (success)
            {
                bool tokenGenerated = _userService.GeneratePasswordToken(registration.EmailAddress);
                if (tokenGenerated)
                {
                    return RedirectToAction("ViewUsers");
                }
            }
            ModelState.AddModelError("", "Failed to add user.");
            return View("AddUser");
        }

        [HttpPost("request-password-reset")]
        public ActionResult RequestPasswordReset([FromBody] PasswordResetRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            bool success = _userService.GeneratePasswordToken(request.EmailAddress);

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

            bool success = _userService.ResetPassword(passwordReset.Token, passwordReset.NewPassword);

            if (success)
            {
                return Ok("Password has been reset successfully.");
            }

            return BadRequest("Invalid or expired token.");
        }

        [HttpGet]
        public ActionResult<List<User>> GetUsers() 
        {
            var companyId = _companyDivider.GetCompanyId();
            var users = _userService.GetUsers(companyId);

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

            var companyId = _companyDivider.GetCompanyId(); 

            bool success = _userService.UpdateUser(id, updatedUser, companyId);
            if (success)
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

            var companyId = _companyDivider.GetCompanyId(); 

            bool success = _userService.DeleteUser(id, companyId);
            if (success)
            {
                return Ok();
            }

            return BadRequest("Failed to delete user");
        }

        [HttpGet("AddUser")]
        public IActionResult AddUser()
        {
            return View();
        }

        [HttpGet("EditUser")]
        public IActionResult EditUser()
        {
            return View();
        }

        [HttpGet("DeleteUser")]
        public IActionResult DeleteUser()
        {
            return View();
        }
    }
}

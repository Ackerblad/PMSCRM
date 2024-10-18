using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
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
            var user = await _userService.AuthenticateUser(loginRequest.EmailAddress, loginRequest.Password);
            if (user != null)
            {
                var companyId = user.CompanyId;
                var claims = new List<Claim>
                {
                    new(ClaimTypes.Email, user.EmailAddress),
                    new("CompanyId", companyId.ToString())
                };

                var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);

                var authProperties = new AuthenticationProperties
                {
                    IsPersistent = loginRequest.RememberMe,
                };

                if (loginRequest.RememberMe)
                {
                    authProperties.ExpiresUtc = DateTime.UtcNow.AddDays(30);
                }
                else
                {
                    authProperties.ExpiresUtc = DateTime.UtcNow.AddHours(10);
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
        [Authorize]
        [HttpPost("AddUser")]
        public async Task<IActionResult> AddUser(UserRegistration registration)
        {
            if (!ModelState.IsValid)
            {
                return View("AddUser");
            }

            var tempPassword = _userService.GenerateTemporaryPassword();

            var companyId = _companyDivider.GetCompanyId();

            bool success = await _userService.AddUser(companyId, registration.RoleId, registration.EmailAddress, registration.FirstName, registration.LastName,
                                               registration.PhoneNumber, tempPassword);

            if (success)
            {
                bool tokenGenerated = await _userService.GeneratePasswordToken(registration.EmailAddress);
                if (tokenGenerated)
                {
                    return RedirectToAction("Success");
                }
            }
            ModelState.AddModelError("", "Failed to add user.");
            return View("AddUser");
        }

        [HttpPost("request-password-reset")]
        public async Task<IActionResult> RequestPasswordReset(PasswordResetRequest request)
        {
            if (!ModelState.IsValid)
            {
                return View("ForgotPassword");
            }

            bool success = await _userService.GeneratePasswordToken(request.EmailAddress);

            if (success)
            {
                ViewBag.Message = "Reset link sent to your email, You can now close this page.";
                ViewBag.IsSuccess = true;
            }
            else
            {
                ViewBag.Message = "Email not found, try again";
                ViewBag.IsSuccess = false;
            }

            return View("ForgotPassword");
        }

        [HttpGet("reset-password")]
        public IActionResult ResetPassword(Guid token)
        {
            var model = new PasswordReset { Token = token };
            return View(model);
        }

        [HttpPost("reset-password")]
        public async Task<IActionResult> ResetPassword(PasswordReset passwordReset, string confirmPassword)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.Message = "Password is invalid.";
                ViewBag.IsSuccess = false;
                return View(passwordReset);
            }

            if (passwordReset.NewPassword != confirmPassword)
            {
                ViewBag.Message = "Passwords do not match.";
                ViewBag.IsSuccess = false;
                return View(passwordReset);
            }

            var success = await _userService.ResetPassword(passwordReset.Token, passwordReset.NewPassword);

            if (success)
            {
                TempData["ResetSuccessMessage"] = "Your password has been reset.";
                return RedirectToAction("Login", "Login");
            }

            ViewBag.Message = "This reset link has expired.";
            ViewBag.IsSuccess = false;
            return View(passwordReset);
        }
        [Authorize]
        [HttpGet("ViewUsers")]
        public async Task<IActionResult> ViewUsers()
        {
            var companyId = _companyDivider.GetCompanyId();
            var users = await _userService.GetUsers(companyId);

            if (users == null || users.Count == 0)
            {
                TempData["InfoMessage"] = "No users available.";
            }
            return View(users);
        }
        [Authorize]
        [HttpGet("EditUser/{id}")]
        public async Task<IActionResult> EditUser(Guid id)
        {
            var companyId = _companyDivider.GetCompanyId();
            var user = await _userService.GetById(id, companyId);
            if (user == null)
            {
                return NotFound("User not found.");
            }
            return View(user);
        }
        [Authorize]
        [HttpPost("EditUser/{id}")]
        public async Task<IActionResult> UpdateUser(Guid id, UserUpdate updatedUser)
        {
            if (!ModelState.IsValid)
            {
                return View(updatedUser);
            }

            var companyId = _companyDivider.GetCompanyId();

            bool success = await _userService.UpdateUser(id, updatedUser, companyId);
            if (success)
            {
                return RedirectToAction("ViewUsers");
            }

            ModelState.AddModelError(string.Empty, "Failed to update user.");
            return View(updatedUser);
        }
        [Authorize]
        [HttpGet("DeleteUser/{id}")]
        public async Task<IActionResult> DeleteUser(Guid id)
        {
            var companyId = _companyDivider.GetCompanyId();
            var user = await _userService.GetById(id, companyId);
            if (user == null)
            {
                return NotFound("User not found.");
            }
            return View(user);
        }
        [Authorize]
        [HttpPost("DeleteConfirmed/{id}")]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            var companyId = _companyDivider.GetCompanyId();
            var user = await _userService.GetById(id, companyId);
            if (user == null)
            {
                return NotFound("User not found.");
            }

            bool success = await _userService.DeleteUser(id, companyId);
            if (success)
            {
                TempData["SuccessMessage"] = "User deleted successfully!";
                return RedirectToAction("ViewUsers");
            }

            ModelState.AddModelError(string.Empty, "Failed to delete user.");
            return View("DeleteUser", user);
        }
        [Authorize]
        [HttpGet("AddUser")]
        public IActionResult AddUser()
        {
            return View();
        }
        [Authorize]
        [HttpGet("Success")]
        public IActionResult Success()
        {
            return View();
        }
        [Authorize]
        [HttpGet("EditUser")]
        public IActionResult EditUser()
        {
            return View();
        }
        [Authorize]
        [HttpGet("DeleteUser")]
        public IActionResult DeleteUser()
        {
            return View();
        }

        [HttpGet("ForgotPassword")]
        public IActionResult ForgotPassword()
        {
            return View();
        }
    }
}

using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
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
            loginRequest.EmailAddress = loginRequest.EmailAddress.Trim();
            loginRequest.Password = loginRequest.Password.Trim();

            var user = await _userService.AuthenticateUser(loginRequest.EmailAddress, loginRequest.Password);
            if (user != null)
            {
                var companyId = user.CompanyId;
                var roleName = user.Role?.Name ?? "User";

                var claims = new List<Claim>
        {
            new(ClaimTypes.Email, user.EmailAddress),
            new("CompanyId", companyId.ToString()),
            new(ClaimTypes.Role, roleName),
            new(ClaimTypes.NameIdentifier, user.UserId.ToString())
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
        [HttpGet("AddUser")]
        public async Task<IActionResult> AddUser()
        {
            var companyId = _companyDivider.GetCompanyId();
            var roles = await _userService.GetRolesByCompanyIdAsync(companyId);

            ViewBag.Roles = roles?.Select(r => new SelectListItem
            {
                Value = r.RoleId.ToString(),
                Text = r.Name
            }).ToList() ?? new List<SelectListItem>();

            ViewBag.IsEditMode = false;
            return View(new UserRegistration { RoleId = Guid.Empty });
        }

        [Authorize]
        [HttpPost("AddUser")]
        public async Task<IActionResult> AddUser(UserRegistration registration)
        {
            var companyId = _companyDivider.GetCompanyId();
            var roles = await _userService.GetRolesByCompanyIdAsync(companyId);

            ViewBag.IsEditMode = false; 
            ViewBag.Roles = roles.Select(r => new SelectListItem
            {
                Value = r.RoleId.ToString(),
                Text = r.Name
            }).ToList();

            if (!ModelState.IsValid)
            {
                return View(registration); 
            }

            registration.EmailAddress = registration.EmailAddress.Trim();
            registration.FirstName = registration.FirstName.Trim();
            registration.LastName = registration.LastName.Trim();
            registration.PhoneNumber = registration.PhoneNumber.Trim();

            var tempPassword = _userService.GenerateTemporaryPassword();

            bool success = await _userService.AddUser(companyId, registration.RoleId, registration.EmailAddress, registration.FirstName, registration.LastName, registration.PhoneNumber, tempPassword);

            if (success)
            {
                bool tokenGenerated = await _userService.GeneratePasswordToken(registration.EmailAddress);
                if (tokenGenerated)
                {
                    ViewBag.Message = "Success! An email has been sent to the user.";
                    ViewBag.MessageType = "success";
                    return View("AddUser", registration);
                }
            }

            ViewBag.Message = "Failed to add user. Please try again.";
            ViewBag.MessageType = "error";
            return View(registration);
        }

        [HttpPost("request-password-reset")]
        public async Task<IActionResult> RequestPasswordReset(PasswordResetRequest request)
        {
            if (!ModelState.IsValid)
            {
                return View("ForgotPassword");
            }

            request.EmailAddress = request.EmailAddress.Trim();

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

            passwordReset.NewPassword = passwordReset.NewPassword.Trim();
            confirmPassword = confirmPassword.Trim();

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
        public async Task<IActionResult> ViewUsers(string sortBy, string sortDirection = "asc")
        {
            var companyId = _companyDivider.GetCompanyId();
            var users = await _userService.GetAllAsync(companyId);

            ViewBag.CurrentUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            users = users.SortBy(sortBy, sortDirection).ToList();

            ViewBag.CurrentSort = sortBy;
            ViewBag.CurrentSortDirection = sortDirection;

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

            var userRegistration = new UserRegistration
            {
                RoleId = user.RoleId,
                EmailAddress = user.EmailAddress,
                FirstName = user.FirstName,
                LastName = user.LastName,
                PhoneNumber = user.PhoneNumber
            };

            var roles = await _userService.GetRolesByCompanyIdAsync(companyId);
            ViewBag.Roles = roles.Select(r => new SelectListItem
            {
                Value = r.RoleId.ToString(),
                Text = r.Name
            }).ToList();

            ViewBag.IsEditMode = true;
            return View("AddUser", userRegistration);
        }

        [Authorize]
        [HttpPost("EditUser/{id}")]
        public async Task<IActionResult> UpdateUser(Guid id, UserUpdate updatedUser)
        {
            var companyId = _companyDivider.GetCompanyId();
            ViewBag.IsEditMode = true;
            var roles = await _userService.GetRolesByCompanyIdAsync(companyId);
            ViewBag.Roles = roles.Select(r => new SelectListItem
            {
                Value = r.RoleId.ToString(),
                Text = r.Name
            }).ToList();

            if (!ModelState.IsValid)
            {
                return View("AddUser", updatedUser);
            }

            updatedUser.EmailAddress = updatedUser.EmailAddress.Trim();
            updatedUser.FirstName = updatedUser.FirstName.Trim();
            updatedUser.LastName = updatedUser.LastName.Trim();
            updatedUser.PhoneNumber = updatedUser.PhoneNumber.Trim();

            bool success = await _userService.UpdateUser(id, updatedUser, companyId);
            if (success)
            {
                return RedirectToAction("ViewUsers");
            }

            ViewBag.Message = "Failed to update user. Please try again.";
            ViewBag.MessageType = "error";
            return View("AddUser", updatedUser); 
        }

        [Authorize]
        [HttpGet("DeleteUser/{id}")]
        public async Task<IActionResult> DeleteUser(Guid id)
        {
            var currentUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (currentUserId == id.ToString())
            {
                TempData["ErrorMessage"] = "You cannot delete your own account.";
                return RedirectToAction("ViewUsers"); 
            }

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
            var currentUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (currentUserId == id.ToString())
            {
                TempData["ErrorMessage"] = "You cannot delete your own account.";
                return RedirectToAction("ViewUsers");
            }

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

        [HttpGet("Details/{id}")]
        public async Task<IActionResult> Details(Guid id)
        {
            var companyId = _companyDivider.GetCompanyId();
            var user = await _userService.GetById(id, companyId);

            if (user == null)
            {
                ViewBag.Message = "User not found.";
                return View("ViewUsers", user);
            }

            return View(user);
        }

        [HttpGet("ForgotPassword")]
        public IActionResult ForgotPassword()
        {
            return View();
        }
    }
}

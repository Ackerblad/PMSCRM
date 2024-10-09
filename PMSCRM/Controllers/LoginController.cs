using Microsoft.AspNetCore.Mvc;

namespace PMSCRM.Controllers
{
    public class LoginController : Controller
    {
        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }
    }
}

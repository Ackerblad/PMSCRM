using Microsoft.AspNetCore.Mvc;

namespace PMSCRM.Controllers
{
    public class LoginController : Controller
    {
        [HttpGet]
        public IActionResult Index()
        {
            return View("Login");
        }
    }
}

using Microsoft.AspNetCore.Mvc;

namespace PMSCRM.Controllers
{
    public class ProfileController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}

﻿using Microsoft.AspNetCore.Mvc;

namespace PMSCRM.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}

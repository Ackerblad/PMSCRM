﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace PMSCRM.Controllers
{
    //[Authorize] // Restrict access to all actions in this controller
    public class HomeController : Controller
    {
        

        public IActionResult Index()
        {
            return View();
        }

       
    }
}

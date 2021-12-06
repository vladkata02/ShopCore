using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;

namespace ShopCore.Controllers
{
    public class HomeController : Controller
    {
        // GET: Home
        [Authorize]
        public IActionResult Index()
        {
            string userName = HttpContext.User.Identity.Name;

            if (HttpContext.User.IsInRole("Administrator"))
            {
                TempData["adminMessage"] = "You are an Administrator!";
            }

            if (HttpContext.User.IsInRole("Manager"))
            {
                TempData["managerMessage"] = "You are a Manager!";
            }

            TempData["username"] = userName;

            return View();
        }
    }
}
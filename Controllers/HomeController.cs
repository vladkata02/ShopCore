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
                ViewData["adminMessage"] = "You are an Administrator!";
            }

            if (HttpContext.User.IsInRole("Manager"))
            {
                ViewData["managerMessage"] = "You are a Manager!";
            }

            ViewData["username"] = userName;

            return View();
        }
        [Authorize]
        public IActionResult ConfidentialData()
        {
            string userName = HttpContext.User.Identity.Name;
            ViewData["username"] = userName;
            return View();
        }
        public IActionResult Shopping()
        {
            string userName = HttpContext.User.Identity.Name;
            ViewData["username"] = userName;
            return View();
        }
        public IActionResult About()
        {
            string userName = HttpContext.User.Identity.Name;
            ViewData["username"] = userName;
            return View();
        }
        public IActionResult Service()
        {
            string userName = HttpContext.User.Identity.Name;
            ViewData["username"] = userName;
            return View();
        }

    }
}
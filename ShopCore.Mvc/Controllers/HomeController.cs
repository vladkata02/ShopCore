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
        [Authorize]
        public IActionResult Index()
        {
            string userName = this.HttpContext.User.Identity.Name;

            if (this.HttpContext.User.IsInRole("Administrator"))
            {
                this.TempData["adminMessage"] = "You are an Administrator!";
            }

            if (this.HttpContext.User.IsInRole("Manager"))
            {
                this.TempData["managerMessage"] = "You are a Manager!";
            }

            this.TempData["username"] = userName;

            return this.View();
        }
    }
}
using ShopCore.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using ShopCore.Data;

namespace ShopCore.Controllers
{
    public class LayoutController : Controller
    { 
        public IActionResult Index()
        {
            string userName = HttpContext.User.Identity.Name;
            TempData["username"] = userName;
            return View();
        }
    }
}
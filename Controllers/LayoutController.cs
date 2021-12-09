using ShopCore.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using ShopCore.Data;
using ShopCore.ViewModel;

namespace ShopCore.Controllers
{
    public class LayoutController : Controller
    { 
        public IActionResult _Layout()
        {
            var userName = HttpContext.User.Identity.Name;
            TempData["username"] = userName;
            return View(userName);
        }
    }
}
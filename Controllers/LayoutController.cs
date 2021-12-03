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
        // GET: Layout

        private ShopDBEntities3 db;
        public IActionResult Index()
        {
            return View();
        }
        private bool UserVerified(user log)
        {
            var user = db.users.Where(a => a.Username.Equals(log.Username) && a.Password.Equals(log.Password)).FirstOrDefault();
            if (user != null)
            {
                return true;
            }
            return false;
        }
    }
}
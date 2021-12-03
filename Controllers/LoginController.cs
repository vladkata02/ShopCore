using ShopCore.Models;
using ShopCore.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using ShopCore.Data;
namespace ShopCore.Controllers
{
    public class LoginController : Controller
    {
        ShopDBEntities3 db;
                
        
        public IActionResult Index()
        {
         
                return View();
        }
        [HttpPost]
        public IActionResult Index(user log)
        {
            var user = db.users.Where(x => x.Username == log.Username && x.Password == log.Password).FirstOrDefault();
            if (user != null)
            {
                ViewData["Username"] = log.Username.ToString();
                return Redirect(url: "Shopping");
            }
            else
            {
                return View();
            }
        }
        }
    }


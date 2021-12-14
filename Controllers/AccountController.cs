using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using ShopCore.Data;
using ShopCore.Data.Context;
using ShopCore.Models;
using ShopCore.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace ShopCore.Controllers
{
    public class AccountController : Controller
    {
        private ShopDBContext db;

        public AccountController(ShopDBContext db)
        {
            this.db = db;
        }
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Register(RegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
                User user = new User();
                user.Username = model.UserName;
                user.Password = model.Password;
                user.Roles = "Manager,Admin";

                db.Users.Add(user);
                db.SaveChanges();

                TempData["message"] = "User created successfully!";
            }
            return View();
        }
        public IActionResult Login(string returnUrl)
        {
            return View();
        }
        [HttpPost]
        public IActionResult Login(LoginViewModel model,
        string returnUrl)
        {
            bool isUservalid = false;

            User user = db.Users.Where(usr =>
        usr.Username == model.UserName &&
        usr.Password == model.Password).SingleOrDefault();

            if (user != null)
            {
                isUservalid = true;
            }


            if (ModelState.IsValid && isUservalid)
            {
                var claims = new List<Claim>();

                claims.Add(new Claim(ClaimTypes.Name, user.Username));

                string[] roles = user.Roles.Split(",");

                foreach (string role in roles)
                {
                    claims.Add(new Claim(ClaimTypes.Role, role));
                }

                var identity = new ClaimsIdentity(
                    claims, CookieAuthenticationDefaults.
        AuthenticationScheme);

                var principal = new ClaimsPrincipal(identity);

                var props = new AuthenticationProperties();
                props.IsPersistent = model.RememberMe;

                HttpContext.SignInAsync(
                    CookieAuthenticationDefaults.
        AuthenticationScheme,
                    principal, props).Wait();

                return RedirectToAction("Index", "Home");
            }
            else
            {
                TempData["message"] = "Invalid UserName or Password!";
            }

            return View();
        }
        [HttpPost]
        public IActionResult Logout()
        {
            HttpContext.SignOutAsync(
        CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Login", "Account");
        }
    }
}

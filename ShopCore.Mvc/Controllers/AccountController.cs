using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using ShopCore.Data.Context;
using ShopCore.Models;
using ShopCore.ViewModel;

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
            return this.View();
        }

        [HttpPost]
        public IActionResult Register(RegisterViewModel model)
        {
            if (this.ModelState.IsValid)
            {
                User user = new User();
                user.Username = model.UserName;
                user.Password = model.Password;
                user.Roles = "Manager,Admin";

                this.db.Users.Add(user);
                this.db.SaveChanges();

                this.TempData["message"] = "User created successfully!";
            }

            return this.View();
        }

        public IActionResult Login(string returnUrl)
        {
            return this.View();
        }

        [HttpPost]
        public IActionResult Login(LoginViewModel model, string returnUrl)
        {
            bool isUservalid = false;

            User user = this.db.Users.Where(usr => usr.Username == model.UserName && usr.Password == model.Password).SingleOrDefault();

            if (user != null)
            {
                isUservalid = true;
            }

            if (this.ModelState.IsValid && isUservalid)
            {
                var claims = new List<Claim>();

                claims.Add(new Claim(ClaimTypes.Name, user.Username));

                string[] roles = user.Roles.Split(",");

                foreach (string role in roles)
                {
                    claims.Add(new Claim(ClaimTypes.Role, role));
                }

                var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);

                var principal = new ClaimsPrincipal(identity);

                var props = new AuthenticationProperties();
                props.IsPersistent = model.RememberMe;

                this.HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal, props).Wait();

                return this.RedirectToAction("Index", "Home");
            }
            else
            {
                this.TempData["message"] = "Invalid UserName or Password!";
            }

            return this.View();
        }

        [HttpPost]
        public IActionResult Logout()
        {
            this.HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);

            return this.RedirectToAction("Login", "Account");
        }
    }
}

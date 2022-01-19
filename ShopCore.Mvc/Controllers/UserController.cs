namespace ShopCore.Controllers
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Security.Claims;
    using Microsoft.AspNetCore.Authentication;
    using Microsoft.AspNetCore.Authentication.Cookies;
    using Microsoft.AspNetCore.Mvc;
    using ShopCore.Services.Interfaces;
    using ShopCore.Services.ViewModel;

    public class UserController : Controller
    {
        private IUserRepository userRepository;
        private IUnitOfWork unitOfWork;

        public UserController(IUserRepository userRepository, IUnitOfWork unitOfWork)
        {
            this.userRepository = userRepository;
            this.unitOfWork = unitOfWork;
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
                this.userRepository.Add(model);
                this.unitOfWork.SaveChanges();

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

            UserViewModel user = this.userRepository.LoginVerification(model);

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

                return this.RedirectToAction("Index", "Shopping");
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

            return this.RedirectToAction("Login", "User");
        }
    }
}

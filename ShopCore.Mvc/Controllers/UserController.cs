namespace ShopCore.Controllers
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Security.Claims;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Authentication;
    using Microsoft.AspNetCore.Authentication.Cookies;
    using Microsoft.AspNetCore.Authentication.Facebook;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.Logging;
    using ShopCore.Services.Interfaces;
    using ShopCore.Services.ViewModel;

    public class UserController : Controller
    {
        private readonly ILogger<UserController> logger;
        private IUserRepository userRepository;
        private IUnitOfWork unitOfWork;

        public UserController(IUserRepository userRepository, IUnitOfWork unitOfWork, ILogger<UserController> logger)
        {
            this.logger = logger;
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

        public IActionResult FacebookLogin()
        {
            var properties = new AuthenticationProperties { RedirectUri = this.Url.Action("FacebookLoginResponse") };

            return this.Challenge(properties, FacebookDefaults.AuthenticationScheme);
        }

        public IActionResult FacebookRegister()
        {
            var properties = new AuthenticationProperties { RedirectUri = this.Url.Action("FacebookRegisterResponse") };

            return this.Challenge(properties, FacebookDefaults.AuthenticationScheme);
        }

        public IActionResult FacebookLoginResponse()
        {
            string userName = this.HttpContext.User.Identity.Name.ToString();
            string email = this.HttpContext.User.FindFirstValue(ClaimTypes.Email);
            if (!this.userRepository.UserExists(email))
            {
                this.HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
                this.TempData["message"] = "This user don't have Facebook registered account!";
                return this.RedirectToAction("Register", "User");
            }

            var claim = new List<Claim>();
            claim.Add(new Claim("FullName", userName));
            claim.Add(new Claim(ClaimTypes.Name, userName));

            this.Claims(claim);

            return this.RedirectToAction("Index", "Shopping");
        }

        // TODO:
        // Remove email input in request cart and places where needed,

        public IActionResult FacebookRegisterResponse()
        {
            string email = this.HttpContext.User.FindFirstValue(ClaimTypes.Email);

            if (this.userRepository.UserExists(email))
            {
                this.HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
                this.TempData["message"] = "This user already have Facebook registered account!";
                return this.RedirectToAction("Register", "User");
            }

            string userName = this.HttpContext.User.Identity.Name.ToString();

            this.userRepository.FacebookAdd(userName, email);
            var claim = new List<Claim>();
            claim.Add(new Claim("FullName", userName));
            claim.Add(new Claim(ClaimTypes.Name, userName));

            this.Claims(claim);

            return this.RedirectToAction("Index", "Shopping");
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
                claims.Add(new Claim("FullName", user.FullName));

                this.Claims(claims);

                return this.RedirectToAction("Index", "Shopping");
            }
            else
            {
                this.TempData["message"] = "Invalid username or password!";
            }

            return this.View();
        }

        [HttpPost]
        public IActionResult Logout()
        {
            this.HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);

            return this.RedirectToAction("Login", "User");
        }

        private void Claims(List<Claim> claim)
        {
            var identity = new ClaimsIdentity(claim, CookieAuthenticationDefaults.AuthenticationScheme);
            var principal = new ClaimsPrincipal(identity);
            var props = new AuthenticationProperties();
            props.IsPersistent = true;
            this.HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal).Wait();
        }
    }
}

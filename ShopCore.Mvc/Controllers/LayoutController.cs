using Microsoft.AspNetCore.Mvc;

namespace ShopCore.Controllers
{
    public class LayoutController : Controller
    {
        public IActionResult _Layout()
        {
            var userName = this.HttpContext.User.Identity.Name;
            this.TempData["username"] = userName;
            return this.View(userName);
        }
    }
}
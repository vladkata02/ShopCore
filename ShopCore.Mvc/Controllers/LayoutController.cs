using Microsoft.AspNetCore.Mvc;

namespace ShopCore.Controllers
{
    public class LayoutController : Controller
    {
        public IActionResult _Layout()
        {
            return this.View();
        }
    }
}
namespace ShopCore.Controllers
{
    using Microsoft.AspNetCore.Mvc;

    public class LayoutController : Controller
    {
        public IActionResult _Layout()
        {
            return this.View();
        }
    }
}
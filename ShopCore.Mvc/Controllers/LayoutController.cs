namespace ShopCore.Controllers
{
    using Microsoft.AspNetCore.Mvc;

    // TODO има ли смисъл от този контролер?
    public class LayoutController : Controller
    {
        public IActionResult _Layout()
        {
            return this.View();
        }
    }
}
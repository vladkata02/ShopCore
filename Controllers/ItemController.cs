using ShopCore.Models;
using ShopCore.ViewModel;
using System;
using System.IO;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using ShopCore.Data;

namespace ShopCore.Controllers
{
    public class ItemController : Controller
    {
        private readonly ShopDBContext _context;

        public ItemController(ShopDBContext context)
        {
            _context = context;
        }
        public IActionResult Index()
        {
            string userName = HttpContext.User.Identity.Name;
            TempData["username"] = userName;
            ItemViewModel objItemViewModel = new ItemViewModel();
            objItemViewModel.CategorySelectListItem = (from objCat in _context.Categories

                                                       select new Microsoft.AspNetCore.Mvc.Rendering.SelectListItem()
                                                       {
                                                           Text = objCat.CategoryName,
                                                           Value = objCat.CategoryId.ToString(),
                                                           Selected = true
                                                       });
            return View(objItemViewModel);
        }
        [HttpPost]
        public IActionResult Index(ItemViewModel objItemViewModel)
        {
            string userName = HttpContext.User.Identity.Name;
            TempData["username"] = userName;
            string NewImage = Guid.NewGuid() + Path.GetExtension(objItemViewModel.ImagePath.FileName);

            Item objItem = new Item();
            objItem.ImagePath = "~/Images/" + NewImage;
            objItem.CategoryId = objItemViewModel.CategoryId;
            objItem.Description = objItemViewModel.Description;
            objItem.ItemCode = objItemViewModel.ItemCode;
            objItem.ItemId = Guid.NewGuid();
            objItem.ItemName = objItemViewModel.ItemName;
            objItem.ItemBrand = objItemViewModel.ItemBrand;
            objItem.ItemPrice = objItemViewModel.ItemPrice;
            _context.Items.Add(objItem);
            _context.SaveChanges();

            return Json(new { Success = true, Message = "Item is added Succesfully." });
        }
    }
}
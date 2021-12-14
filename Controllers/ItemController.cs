using ShopCore.Models;
using ShopCore.ViewModel;
using System;
using System.IO;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using ShopCore.Data;
using Microsoft.AspNetCore.Http;
using ShopCore.Data.Context;

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
        public IActionResult Index(ItemViewModel objItemViewModel, IFormFile files)
        {
            string userName = HttpContext.User.Identity.Name;
            TempData["username"] = userName;

            var fileName = Path.GetFileName(files.FileName);
  
            var fileExtension = Path.GetExtension(fileName);
            var newFileName = String.Concat(Convert.ToString(Guid.NewGuid()), fileExtension);

            Item objItem = new Item();
            objItem.ImageName = newFileName;
            objItem.CategoryId = objItemViewModel.CategoryId;
            objItem.Description = objItemViewModel.Description;
            objItem.ItemCode = objItemViewModel.ItemCode;
            objItem.ItemId = Guid.NewGuid();
            objItem.ItemName = objItemViewModel.ItemName;
            objItem.ItemBrand = objItemViewModel.ItemBrand;
            objItem.ItemPrice = objItemViewModel.ItemPrice;

            using (var target = new MemoryStream())
            {
                files.CopyTo(target);
                objItem.Image = target.ToArray();
            }
            _context.Items.Add(objItem);
            _context.SaveChanges();

            return RedirectToAction("Index");
        }
    }
}
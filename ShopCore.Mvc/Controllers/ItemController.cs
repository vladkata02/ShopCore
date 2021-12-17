namespace ShopCore.Controllers
{
    using System;
    using System.IO;
    using System.Linq;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.Configuration;
    using ShopCore.Data;
    using ShopCore.Data.Context;
    using ShopCore.Models;
    using ShopCore.ViewModel;

    public class ItemController : Controller
    {
        private readonly ShopDBContext context;

        public ItemController(ShopDBContext context)
        {
            this.context = context;
        }

        public IActionResult Index()
        {
            string userName = this.HttpContext.User.Identity.Name;
            this.TempData["username"] = userName;
            ItemViewModel objItemViewModel = new ItemViewModel();
            objItemViewModel.CategorySelectListItem = from objCat in this.context.Categories
                select new Microsoft.AspNetCore.Mvc.Rendering.SelectListItem()
                                                       {
                                                           Text = objCat.CategoryName,
                                                           Value = objCat.CategoryId.ToString(),
                                                           Selected = true,
                                                       };

            return this.View(objItemViewModel);
        }

        [HttpPost]
        public IActionResult Index(ItemViewModel objItemViewModel, IFormFile files)
        {
            string userName = this.HttpContext.User.Identity.Name;
            this.TempData["username"] = userName;

            var fileName = Path.GetFileName(files.FileName);
            var fileExtension = Path.GetExtension(fileName);
            var newFileName = string.Concat(Convert.ToString(Guid.NewGuid()), fileExtension);

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

            this.context.Items.Add(objItem);
            this.context.SaveChanges();

            return this.RedirectToAction("Index");
        }
    }
}
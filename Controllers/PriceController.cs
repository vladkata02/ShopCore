using Microsoft.AspNetCore.Mvc;
using ShopCore.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ShopCore.Models;
using ShopCore.ViewModel;

namespace ShopCore.Controllers
{
    public class PriceController : Controller
    {
        private readonly ShopDBContext _context;
        public PriceController(ShopDBContext context)
        {
            _context = context;
        }
        public IActionResult Index(string ItemId)
        {
            string userName = HttpContext.User.Identity.Name;
            TempData["username"] = userName;
            var itemCheckId = _context.Items.SingleOrDefault(model => model.ItemId.ToString() == ItemId);
            //var categoryCheck = _context.Categories.SingleOrDefault(model => model.CategoryId == CategoryId);
            ShoppingViewModel objItem = new ShoppingViewModel();
            objItem.ItemName = itemCheckId.ItemName;
            objItem.Image = itemCheckId.Image;
            objItem.Description = itemCheckId.Description;
            objItem.ItemPrice = itemCheckId.ItemPrice;
            objItem.ItemBrand = itemCheckId.ItemBrand;
            objItem.ItemId = itemCheckId.ItemId;
            objItem.ItemCode = itemCheckId.ItemCode;
                                                                 
            return View(objItem);
        }
        public IActionResult Index(string ItemId,decimal ItemPrice, PriceEditorViewModel objPriceViewModel)
        {
            var ifCheckId = _context.Prices.SingleOrDefault(model => model.ItemId == ItemId);
            if (ifCheckId == null)
            {
                Price objFirstPrice = new Price();
                objFirstPrice.PriceId = _context.Prices.Count()+1;
                objFirstPrice.ItemId = ItemId;
                objFirstPrice.PriceOfItem = ItemPrice;
                objFirstPrice.DateOfPrice = DateTime.Now;
                _context.Prices.Add(objFirstPrice);
            }
            
                Price objPrice = new Price();
                objPrice.PriceId = _context.Prices.Count() + 1;
                objPrice.ItemId = ItemId;
                objPrice.PriceOfItem = objPriceViewModel.CurrentPrice;
                objPrice.DateOfPrice = DateTime.Now;
                var entity = _context.Items.FirstOrDefault(item => item.ItemId.ToString() == ItemId);
                entity.ItemPrice = objPriceViewModel.CurrentPrice;
                _context.Items.Update(entity);
                _context.Prices.Add(objPrice);
                _context.SaveChanges();
            return View();
        }
    }
}

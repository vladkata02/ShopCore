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
        [HttpPost]
        public IActionResult Index(Guid button)
        {
            string userName = HttpContext.User.Identity.Name;
            TempData["username"] = userName;
            Guid ItemId = button;
            var itemCheckId = _context.Items.SingleOrDefault(model => model.ItemId == ItemId);
            PriceEditorViewModel objItem = new PriceEditorViewModel();
            objItem.ItemName = itemCheckId.ItemName;
            objItem.Image = itemCheckId.Image;
            objItem.ItemPrice = itemCheckId.ItemPrice;
            objItem.ItemBrand = itemCheckId.ItemBrand;
            objItem.ItemId = ItemId;
                                                                 
            return View(objItem);
            //item price cant get though
        }
        [HttpPost]
        public IActionResult ChangePrice(PriceEditorViewModel objItem, Guid button)
        {
            Guid ItemId = button;
            var ifCheckId = _context.Prices.Any(model => model.ItemId == ItemId.ToString());
            if (ifCheckId == false)
            {
                var entityForPrice = _context.Items.FirstOrDefault(item => item.ItemId == ItemId);
                Price objFirstPrice = new Price();
                objFirstPrice.PriceId = _context.Prices.Count()+1;
                objFirstPrice.ItemId = ItemId.ToString();
                objFirstPrice.PriceOfItem = entityForPrice.ItemPrice;
                objFirstPrice.DateOfPrice = DateTime.Now;
                _context.Prices.Add(objFirstPrice);
                _context.SaveChanges();
            }
            
                Price objPrice = new Price();
                objPrice.PriceId = _context.Prices.Count() + 1;
                objPrice.ItemId = ItemId.ToString();
                objPrice.PriceOfItem = objItem.CurrentPrice;
                objPrice.DateOfPrice = DateTime.Now;
                var entity = _context.Items.FirstOrDefault(item => item.ItemId == ItemId);
                entity.ItemPrice = objItem.CurrentPrice;
                _context.Items.Update(entity);
                _context.Prices.Add(objPrice);
                _context.SaveChanges();
            return RedirectToAction("PriceHistory", new {  button });
        }
        public IActionResult PriceHistory(Guid button)
        {
            Guid ItemId = button;
            string userName = HttpContext.User.Identity.Name;
            TempData["username"] = userName;
            List<PriceHistoryViewModel> list = new List<PriceHistoryViewModel>();
            foreach (var order in _context.Prices.Where(element => element.ItemId == ItemId.ToString()))
            {

                PriceHistoryViewModel objPriceHistoryModel = new PriceHistoryViewModel();
                objPriceHistoryModel.ItemId = order.ItemId;
                objPriceHistoryModel.CurrentPrice = order.PriceOfItem;
                objPriceHistoryModel.DateOfPrice = order.DateOfPrice;

                var findElementById = _context.Items.Where(check => check.ItemId.ToString() == order.ItemId).FirstOrDefault();
                objPriceHistoryModel.Image = findElementById.Image;
                objPriceHistoryModel.ItemBrand = findElementById.ItemBrand;
                objPriceHistoryModel.ItemName = findElementById.ItemName;

                list.Add(objPriceHistoryModel);

            }
            return View(list);

        }
    }
}

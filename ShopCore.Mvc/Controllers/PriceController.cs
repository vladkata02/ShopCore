namespace ShopCore.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Mvc;
    using ShopCore.Data;
    using ShopCore.Data.Context;
    using ShopCore.Models;
    using ShopCore.ViewModel;

    public class PriceController : Controller
    {
        private readonly ShopDBContext context;

        public PriceController(ShopDBContext context)
        {
            this.context = context;
        }

        [HttpPost]
        public IActionResult Index(Guid button)
        {
            string userName = this.HttpContext.User.Identity.Name;
            this.TempData["username"] = userName;
            Guid itemId = button;
            var itemCheckId = this.context.Items.SingleOrDefault(model => model.ItemId == itemId);

            PriceEditorViewModel objItem = new PriceEditorViewModel();
            objItem.ItemName = itemCheckId.ItemName;
            objItem.Image = itemCheckId.Image;
            objItem.ItemPrice = itemCheckId.ItemPrice;
            objItem.ItemBrand = itemCheckId.ItemBrand;
            objItem.ItemId = itemId;

            return this.View(objItem);
        }

        [HttpPost]
        public IActionResult ChangePrice(PriceEditorViewModel objItem, Guid button)
        {
            Guid itemId = button;
            var ifCheckId = this.context.Prices.Any(model => model.ItemId == itemId.ToString());
            if (ifCheckId == false)
            {
                var entityForPrice = this.context.Items.FirstOrDefault(item => item.ItemId == itemId);
                Price objFirstPrice = new Price();
                objFirstPrice.PriceId = this.context.Prices.Count() + 1;
                objFirstPrice.ItemId = itemId.ToString();
                objFirstPrice.PriceOfItem = entityForPrice.ItemPrice;
                objFirstPrice.DateOfPrice = DateTime.Now;

                this.context.Prices.Add(objFirstPrice);
                this.context.SaveChanges();
            }

            Price objPrice = new Price();
            objPrice.PriceId = this.context.Prices.Count() + 1;
            objPrice.ItemId = itemId.ToString();
            objPrice.PriceOfItem = objItem.CurrentPrice;
            objPrice.DateOfPrice = DateTime.Now;

            var entity = this.context.Items.FirstOrDefault(item => item.ItemId == itemId);
            entity.ItemPrice = objItem.CurrentPrice;

            this.context.Items.Update(entity);
            this.context.Prices.Add(objPrice);
            this.context.SaveChanges();

            return this.RedirectToAction("PriceHistory", new { button });
        }

        public IActionResult PriceHistory(Guid button)
        {
            Guid itemId = button;
            string userName = this.HttpContext.User.Identity.Name;
            this.TempData["username"] = userName;
            List<PriceHistoryViewModel> list = new List<PriceHistoryViewModel>();

            foreach (var order in this.context.Prices.Where(element => element.ItemId == itemId.ToString()))
            {
                PriceHistoryViewModel objPriceHistoryModel = new PriceHistoryViewModel();
                objPriceHistoryModel.ItemId = order.ItemId;
                objPriceHistoryModel.CurrentPrice = order.PriceOfItem;
                objPriceHistoryModel.DateOfPrice = order.DateOfPrice;

                var findElementById = this.context.Items.Where(check => check.ItemId.ToString() == order.ItemId).FirstOrDefault();
                objPriceHistoryModel.Image = findElementById.Image;
                objPriceHistoryModel.ItemBrand = findElementById.ItemBrand;
                objPriceHistoryModel.ItemName = findElementById.ItemName;

                list.Add(objPriceHistoryModel);
            }

            return this.View(list);
        }
    }
}

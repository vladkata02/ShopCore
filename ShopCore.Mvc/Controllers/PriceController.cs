namespace ShopCore.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Mvc;
    using ShopCore.Data;
    using ShopCore.Data.Context;
    using ShopCore.Data.Models;
    using ShopCore.Services.Interfaces;
    using ShopCore.Services.ViewModel;

    public class PriceController : Controller
    {
        private IPriceRepository priceRepository;

        public PriceController(IPriceRepository priceRepository)
        {
            this.priceRepository = priceRepository;
        }

        [HttpPost]
        public IActionResult Index(Guid button)
        {
            string userName = this.HttpContext.User.Identity.Name;
            this.TempData["username"] = userName;
            Guid itemId = button;
            var itemCheckId = this.priceRepository.CheckId(itemId);

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
            bool ifCheckId = this.priceRepository.IfAnyCheckId(itemId);
            if (ifCheckId == false)
            {
                var originalPrice = this.priceRepository.CheckOriginalPrice(itemId);
                Price objFirstPrice = new Price();
                objFirstPrice.PriceId = this.priceRepository.TableCount();
                objFirstPrice.ItemId = itemId.ToString();
                objFirstPrice.PriceOfItem = originalPrice.ItemPrice;
                objFirstPrice.DateOfPrice = DateTime.Now;

                this.priceRepository.AddFirstPrice(objFirstPrice);
                this.priceRepository.Save();
            }

            Price objPrice = new Price();
            objPrice.PriceId = this.priceRepository.TableCount();
            objPrice.ItemId = itemId.ToString();
            objPrice.PriceOfItem = objItem.CurrentPrice;
            objPrice.DateOfPrice = DateTime.Now;

            var entity = this.priceRepository.CheckOriginalPrice(itemId);
            entity.ItemPrice = objItem.CurrentPrice;

            this.priceRepository.UpdatePrice(entity);
            this.priceRepository.AddChangedPrice(objPrice);
            this.priceRepository.Save();

            return this.RedirectToAction("PriceHistory", new { button });
        }

        public IActionResult PriceHistory(Guid button)
        {
            Guid itemId = button;
            string userName = this.HttpContext.User.Identity.Name;
            this.TempData["username"] = userName;
            List<PriceHistoryViewModel> list = new List<PriceHistoryViewModel>();

            foreach (var order in this.priceRepository.WhereId(itemId))
            {
                PriceHistoryViewModel objPriceHistoryModel = new PriceHistoryViewModel();
                objPriceHistoryModel.ItemId = order.ItemId;
                objPriceHistoryModel.CurrentPrice = order.PriceOfItem;
                objPriceHistoryModel.DateOfPrice = order.DateOfPrice;

                var findElementById = this.priceRepository.FindItemById(itemId, objPriceHistoryModel);
                objPriceHistoryModel.Image = findElementById.Image;
                objPriceHistoryModel.ItemBrand = findElementById.ItemBrand;
                objPriceHistoryModel.ItemName = findElementById.ItemName;

                list.Add(objPriceHistoryModel);
            }

            return this.View(list);
        }
    }
}

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
            Guid itemId = button;
            var itemCheckId = this.priceRepository.FindElementById(itemId);

            PriceEditorViewModel objectItem = new PriceEditorViewModel();
            objectItem.ItemName = itemCheckId.Name;
            objectItem.ImageContent = itemCheckId.ImageContent;
            objectItem.ItemPrice = itemCheckId.Price;
            objectItem.ItemBrand = itemCheckId.Brand;
            objectItem.ItemId = itemId;

            return this.View(objectItem);
        }

        [HttpPost]
        public IActionResult ChangePrice(PriceEditorViewModel objItem, Guid button)
        {
            Guid itemId = button;
            bool ifCheckId = this.priceRepository.IfAnyPricesInDatabase(itemId);
            if (ifCheckId == false)
            {
                var originalPrice = this.priceRepository.CheckOriginalPrice(itemId);
                Price objFirstPrice = new Price();
                objFirstPrice.Id = this.priceRepository.TableCount();
                objFirstPrice.ItemId = itemId.ToString();
                objFirstPrice.PriceValue = originalPrice.Price;
                objFirstPrice.Date = DateTime.Now;

                this.priceRepository.AddFirstPrice(objFirstPrice);
                this.priceRepository.Save();
            }

            Price objPrice = new Price();
            objPrice.Id = this.priceRepository.TableCount();
            objPrice.ItemId = itemId.ToString();
            objPrice.PriceValue = objItem.CurrentPrice;
            objPrice.Date = DateTime.Now;

            var entity = this.priceRepository.CheckOriginalPrice(itemId);
            entity.Price = objItem.CurrentPrice;

            this.priceRepository.UpdatePrice(entity);
            this.priceRepository.AddChangedPrice(objPrice);
            this.priceRepository.Save();

            return this.RedirectToAction("PriceHistory", new { button });
        }

        public IActionResult PriceHistory(Guid button)
        {
            Guid itemId = button;
            List<PriceHistoryViewModel> list = new List<PriceHistoryViewModel>();

            foreach (var order in this.priceRepository.FindPriceHistoryById(itemId))
            {
                PriceHistoryViewModel objPriceHistoryModel = new PriceHistoryViewModel();
                objPriceHistoryModel.ItemId = order.ItemId;
                objPriceHistoryModel.CurrentPrice = order.PriceValue;
                objPriceHistoryModel.Date = order.Date;

                var findElementById = this.priceRepository.FindItemById(itemId, objPriceHistoryModel);
                objPriceHistoryModel.ImageContent = findElementById.ImageContent;
                objPriceHistoryModel.ItemBrand = findElementById.Brand;
                objPriceHistoryModel.ItemName = findElementById.Name;

                list.Add(objPriceHistoryModel);
            }

            return this.View(list);
        }
    }
}

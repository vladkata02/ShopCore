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
            var elementWithData = this.priceRepository.FindElementById(itemId);

            PriceEditorViewModel objectItem = new PriceEditorViewModel();
            objectItem.ItemName = elementWithData.Name;
            objectItem.ImageContent = elementWithData.ImageContent;
            objectItem.ItemPrice = elementWithData.Price;
            objectItem.ItemBrand = elementWithData.Brand;
            objectItem.ItemId = itemId;

            return this.View(objectItem);
        }

        [HttpPost]
        public IActionResult ChangePrice(PriceEditorViewModel objectItem, Guid button)
        {
            Guid itemId = button;
            bool ifAnyPricesExist = this.priceRepository.IfAnyPricesInDatabase(itemId);
            if (ifAnyPricesExist == false)
            {
                var originalPrice = this.priceRepository.CheckOriginalPrice(itemId);
                Price objectFirstPrice = new Price();
                objectFirstPrice.Id = this.priceRepository.TableCount();
                objectFirstPrice.ItemId = itemId.ToString();
                objectFirstPrice.PriceValue = originalPrice.Price;
                objectFirstPrice.Date = DateTime.Now;

                this.priceRepository.AddFirstPrice(objectFirstPrice);
                this.priceRepository.Save();
            }

            Price objectPrice = new Price();
            objectPrice.Id = this.priceRepository.TableCount();
            objectPrice.ItemId = itemId.ToString();
            objectPrice.PriceValue = objectItem.CurrentPrice;
            objectPrice.Date = DateTime.Now;

            var entityWithOriginalPrice = this.priceRepository.CheckOriginalPrice(itemId);
            entityWithOriginalPrice.Price = objectItem.CurrentPrice;

            this.priceRepository.UpdatePrice(entityWithOriginalPrice);
            this.priceRepository.AddChangedPrice(objectPrice);
            this.priceRepository.Save();

            return this.RedirectToAction("PriceHistory", new { button });
        }

        public IActionResult PriceHistory(Guid button)
        {
            Guid itemId = button;
            List<PriceHistoryViewModel> listOfItemsHistory = new List<PriceHistoryViewModel>();

            foreach (var order in this.priceRepository.FindPriceHistoryById(itemId))
            {
                PriceHistoryViewModel objectPriceHistoryModel = new PriceHistoryViewModel();
                objectPriceHistoryModel.ItemId = order.ItemId;
                objectPriceHistoryModel.CurrentPrice = order.PriceValue;
                objectPriceHistoryModel.Date = order.Date;

                var findElementById = this.priceRepository.FindItemById(itemId, objectPriceHistoryModel);
                objectPriceHistoryModel.ImageContent = findElementById.ImageContent;
                objectPriceHistoryModel.ItemBrand = findElementById.Brand;
                objectPriceHistoryModel.ItemName = findElementById.Name;

                listOfItemsHistory.Add(objectPriceHistoryModel);
            }

            return this.View(listOfItemsHistory);
        }
    }
}

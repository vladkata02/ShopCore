namespace ShopCore.Services.Repositories
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Linq.Expressions;
    using System.Threading.Tasks;
    using Microsoft.EntityFrameworkCore;
    using ShopCore.Data.Context;
    using ShopCore.Data.Models;
    using ShopCore.Services.Interfaces;
    using ShopCore.Services.ViewModel;

    public class PriceRepository : IPriceRepository
    {
        private ShopDBContext context;

        public PriceRepository(ShopDBContext context)
        {
            this.context = context;
        }

        public List<PriceHistoryViewModel> GetPriceHistory(List<PriceHistoryViewModel> listOfItemsHistory, Guid itemGuid)
        {
            foreach (var order in this.GetPriceHistoryById(itemGuid))
            {
                PriceHistoryViewModel objectPriceHistoryModel = new PriceHistoryViewModel();
                objectPriceHistoryModel.ItemId = order.ItemId;
                objectPriceHistoryModel.CurrentPrice = order.PriceValue;
                objectPriceHistoryModel.Date = order.Date;

                var item = this.FindItemById(itemGuid, objectPriceHistoryModel);
                objectPriceHistoryModel.ImageContent = item.ImageContent;
                objectPriceHistoryModel.ItemBrand = item.Brand;
                objectPriceHistoryModel.ItemName = item.Name;

                listOfItemsHistory.Add(objectPriceHistoryModel);
            }

            return listOfItemsHistory;
        }

        public PriceEditorViewModel GetPriceEditor(Guid itemGuid)
        {
            var lastPrice = this.GetCurrentPrice(itemGuid);
            PriceEditorViewModel priceEditor = new PriceEditorViewModel();
            priceEditor.ItemName = lastPrice.Name;
            priceEditor.ImageContent = lastPrice.ImageContent;
            priceEditor.ItemPrice = lastPrice.Price;
            priceEditor.ItemBrand = lastPrice.Brand;
            priceEditor.ItemId = itemGuid;

            return priceEditor;
        }

        public void ChangePrice(PriceEditorViewModel priceEditor, Guid itemGuid)
        {
            bool hasAnyPrice = this.IfAnyPricesInDatabase(itemGuid);
            if (!hasAnyPrice)
            {
                this.AddFirstPrice(itemGuid);
                this.context.SaveChanges();
            }

            this.UpdatePrice(itemGuid, priceEditor);
            this.AddChangedPrice(itemGuid, priceEditor);
            this.context.SaveChanges();
        }

        private Item GetCurrentPrice(Guid itemGuid)
        {
            return this.context.Items
                .SingleOrDefault(model => model.Id == itemGuid);
        }

        private bool IfAnyPricesInDatabase(Guid itemGuid)
        {
            return this.context.Prices
                .Any(model => model.ItemId == itemGuid);
        }

        private Item FindOriginalPrice(Guid itemGuid)
        {
            return this.context.Items
                .FirstOrDefault(item => item.Id == itemGuid);
        }

        private int TableCount()
        {
            return this.context.Prices.Count() + 1;
        }

        private void AddFirstPrice(Guid itemGuid)
        {
            Price firstPrice = new Price();
            firstPrice.Id = this.TableCount();
            firstPrice.ItemId = itemGuid;
            firstPrice.PriceValue = this.FindOriginalPrice(itemGuid).Price;
            firstPrice.Date = DateTime.Now;
            this.context.Prices.Add(firstPrice);
        }

        private void UpdatePrice(Guid itemGuid, PriceEditorViewModel priceEditor)
        {
            Item entity = this.FindOriginalPrice(itemGuid);
            entity.Price = priceEditor.CurrentPrice;
            this.context.Entry(entity).State = EntityState.Modified;
        }

        private IEnumerable<Price> GetPriceHistoryById(Guid itemGuid)
        {
            return this.context.Prices
                .Where(element => element.ItemId == itemGuid);
        }

        private Item FindItemById(Guid itemGuid, PriceHistoryViewModel objPriceHistoryModel)
        {
            return this.context.Items
                .Where(check => check.Id == objPriceHistoryModel.ItemId)
                .FirstOrDefault();
        }

        private void AddChangedPrice(Guid itemGuid, PriceEditorViewModel priceEditor)
        {
            Price objectPrice = new Price();
            objectPrice.Id = this.TableCount();
            objectPrice.ItemId = itemGuid;
            objectPrice.PriceValue = priceEditor.CurrentPrice;
            objectPrice.Date = DateTime.Now;
            this.context.Prices.Add(objectPrice);
        }
    }
}

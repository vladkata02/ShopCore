namespace ShopCore.Services.Repositories
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Linq.Expressions;
    using System.Threading.Tasks;
    using Microsoft.EntityFrameworkCore;
    using ShopCore.Data.Models;
    using ShopCore.Services.Context;
    using ShopCore.Services.Interfaces;
    using ShopCore.Services.ViewModel;

    internal class PriceRepository : IPriceRepository
    {
        private ShopDBContext context;

        public PriceRepository(ShopDBContext context)
        {
            this.context = context;
        }

        public List<PriceHistoryViewModel> GetPriceHistory(Guid itemGuid)
        {
            List<PriceHistoryViewModel> listOfItemsHistory = new List<PriceHistoryViewModel>();
            foreach (var price in this.GetPriceHistoryById(itemGuid))
            {
                var item = this.FindItemById(price.ItemId);
                PriceHistoryViewModel priceHistoryModel = new PriceHistoryViewModel(
                        price.PriceValue,
                        price.Date,
                        item.ImageContent,
                        item.Brand,
                        item.Name,
                        price.ItemId);
                listOfItemsHistory.Add(priceHistoryModel);
            }

            return listOfItemsHistory;
        }

        public PriceEditorViewModel GetPriceEditor(Guid itemGuid)
        {
            var lastPrice = this.GetItemWithCurrentPrice(itemGuid);

            PriceEditorViewModel priceEditor = new PriceEditorViewModel(
                lastPrice.Name,
                lastPrice.ImageContent,
                lastPrice.Price,
                lastPrice.Brand,
                itemGuid);

            return priceEditor;
        }

        public void AddFirstPrice(Guid itemGuid)
        {
            Price firstPrice = new Price(
                this.TableCountPlusOne(),
                this.FindItemWithOriginalPrice(itemGuid).Price,
                itemGuid);

            this.context.Prices.Add(firstPrice);
        }

        public void AddChangedPrice(Guid itemGuid, PriceEditorViewModel priceEditor)
        {
            Price price = new Price(
                   this.TableCountPlusOne(),
                   priceEditor.CurrentPrice,
                   itemGuid);

            this.context.Prices.Add(price);
        }

        public void UpdatePrice(Guid itemGuid, PriceEditorViewModel priceEditor)
        {
            Item entity = this.FindItemWithOriginalPrice(itemGuid);
            entity.Price = priceEditor.CurrentPrice;
            this.context.Entry(entity).State = EntityState.Modified;
        }

        public bool IfAnyPricesInDatabase(Guid itemGuid)
        {
            return this.context.Prices
                .Any(model => model.ItemId == itemGuid);
        }

        // TODO Method actualy gets item from items by Guid
        private Item GetItemWithCurrentPrice(Guid itemGuid)
        {
            return this.context.Items
                .SingleOrDefault(model => model.Id == itemGuid);
        }

        // TODO Method actualy gets item from items by Guid
        private Item FindItemWithOriginalPrice(Guid itemGuid)
        {
            return this.context.Items
                .FirstOrDefault(item => item.Id == itemGuid);
        }

        private int TableCountPlusOne()
        {
            return this.context.Prices.Count() + 1;
        }

        private IEnumerable<Price> GetPriceHistoryById(Guid itemGuid)
        {
            return this.context.Prices
                .Where(element => element.ItemId == itemGuid);
        }

        // TODO Method actualy gets item from items by Guid
        private Item FindItemById(Guid itemId)
        {
            return this.context.Items
                .Where(check => check.Id == itemId)
                .FirstOrDefault();
        }
    }
}

﻿namespace ShopCore.Services.Repositories
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

        public List<PriceHistoryViewModel> GetPriceHistory(List<PriceHistoryViewModel> listOfItemsHistory, Guid itemGuid)
        {
            foreach (var order in this.GetPriceHistoryById(itemGuid))
            {
                // TODO Extract creation logic in constructor
                // TODO remove word object from variable name
                PriceHistoryViewModel objectPriceHistoryModel = new PriceHistoryViewModel();
                objectPriceHistoryModel.ItemId = order.ItemId;
                objectPriceHistoryModel.CurrentPrice = order.PriceValue;
                objectPriceHistoryModel.Date = order.Date;

                var item = this.FindItemById(objectPriceHistoryModel);
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

            // TODO Extract creation logic in constructor
            PriceEditorViewModel priceEditor = new PriceEditorViewModel();
            priceEditor.ItemName = lastPrice.Name;
            priceEditor.ImageContent = lastPrice.ImageContent;
            priceEditor.ItemPrice = lastPrice.Price;
            priceEditor.ItemBrand = lastPrice.Brand;
            priceEditor.ItemId = itemGuid;

            return priceEditor;
        }

        public void AddFirstPrice(Guid itemGuid)
        {
            // TODO Extract creation logic in constructor
            Price firstPrice = new Price();
            firstPrice.Id = this.TableCount();
            firstPrice.ItemId = itemGuid;
            firstPrice.PriceValue = this.FindItemWithOriginalPrice(itemGuid).Price;
            firstPrice.Date = DateTime.Now;
            this.context.Prices.Add(firstPrice);
        }

        public void AddChangedPrice(Guid itemGuid, PriceEditorViewModel priceEditor)
        {
            // TODO Extract creation logic in constructor
            // TODO remove word object from variable name
            Price objectPrice = new Price();
            objectPrice.Id = this.TableCount();
            objectPrice.ItemId = itemGuid;
            objectPrice.PriceValue = priceEditor.CurrentPrice;
            objectPrice.Date = DateTime.Now;
            this.context.Prices.Add(objectPrice);
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

        private Item GetCurrentPrice(Guid itemGuid)
        {
            return this.context.Items
                .SingleOrDefault(model => model.Id == itemGuid);
        }

        private Item FindItemWithOriginalPrice(Guid itemGuid)
        {
            return this.context.Items
                .FirstOrDefault(item => item.Id == itemGuid);
        }

        // TODO Method name is lying
        private int TableCount()
        {
            return this.context.Prices.Count() + 1;
        }

        private IEnumerable<Price> GetPriceHistoryById(Guid itemGuid)
        {
            return this.context.Prices
                .Where(element => element.ItemId == itemGuid);
        }

        private Item FindItemById(PriceHistoryViewModel objPriceHistoryModel)
        {
            return this.context.Items
                .Where(check => check.Id == objPriceHistoryModel.ItemId)
                .FirstOrDefault();
        }
    }
}

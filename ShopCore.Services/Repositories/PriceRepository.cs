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

        public Item FindElementById(Guid itemId)
        {
            return this.context.Items
                .SingleOrDefault(model => model.Id == itemId);
        }

        public bool IfAnyPricesInDatabase(Guid itemId)
        {
            return this.context.Prices
                .Any(model => model.ItemId == itemId
                .ToString());
        }

        public Item CheckOriginalPrice(Guid itemId)
        {
            return this.context.Items
                .FirstOrDefault(item => item.Id == itemId);
        }

        public int TableCount()
        {
            return this.context.Prices.Count() + 1;
        }

        public void AddFirstPrice(Price objFirstPrice)
        {
            this.context.Prices.Add(objFirstPrice);
        }

        public void UpdatePrice(Item entity)
        {
            this.context
                .Entry(entity).State = EntityState.Modified;
        }

        public IEnumerable<Price> FindPriceHistoryById(Guid itemId)
        {
            return this.context.Prices
                .Where(element => element.ItemId == itemId
                .ToString());
        }

        public Item FindItemById(Guid itemId, PriceHistoryViewModel objPriceHistoryModel)
        {
            return this.context.Items
                .Where(check => check.Id.ToString() == objPriceHistoryModel.ItemId)
                .FirstOrDefault();
        }

        public void AddChangedPrice(Price objPrice)
        {
            this.context.Prices.Add(objPrice);
        }

        public void Save()
        {
            this.context.SaveChanges();
        }
    }
}

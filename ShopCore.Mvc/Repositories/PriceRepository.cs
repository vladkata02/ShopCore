using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using ShopCore.Data.Context;
using ShopCore.Models;
using ShopCore.Mvc.Interfaces;
using ShopCore.ViewModel;

namespace ShopCore.Mvc.Repositories
{
    public class PriceRepository : IPriceRepository
    {
        private ShopDBContext context;

        public PriceRepository(ShopDBContext context)
        {
            this.context = context;
        }

        public Item CheckId(Guid itemId)
        {
            return this.context.Items.SingleOrDefault(model => model.ItemId == itemId);
        }

        public bool IfAnyCheckId(Guid itemId)
        {
            return this.context.Prices.Any(model => model.ItemId == itemId.ToString());
        }

        public Item CheckOriginalPrice(Guid itemId)
        {
            return this.context.Items.FirstOrDefault(item => item.ItemId == itemId);
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
            this.context.Entry(entity).State = EntityState.Modified;
        }

        public IEnumerable<Price> WhereId(Guid itemId)
        {
            return this.context.Prices.Where(element => element.ItemId == itemId.ToString());
        }

        public Item FindItemById(Guid itemId, PriceHistoryViewModel objPriceHistoryModel)
        {
            return this.context.Items.Where(check => check.ItemId.ToString() == objPriceHistoryModel.ItemId).FirstOrDefault();
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

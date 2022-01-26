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
        private IUnitOfWork unitOfWork;

        public PriceRepository(ShopDBContext context, IUnitOfWork unitOfWork)
        {
            this.context = context;
            this.unitOfWork = unitOfWork;
        }

        public void ChangePrice(PriceEditorViewModel priceEditor, Guid itemGuid)
        {
            if (priceEditor.CurrentPrice != 0)
            {
                bool hasAnyPrice = this.AnyPricesById(itemGuid);
                if (!hasAnyPrice)
                {
                    this.AddFirstPrice(itemGuid);
                    this.context.SaveChanges();
                }

                this.UpdatePrice(itemGuid, priceEditor);
                this.AddChangedPrice(itemGuid, priceEditor);
                this.context.SaveChanges();
            }
        }

        public List<PriceHistoryViewModel> GetPriceHistory(Guid itemGuid)
        {
            List<PriceHistoryViewModel> listOfItemsHistory = new List<PriceHistoryViewModel>();
            foreach (var price in this.GetPriceHistoryById(itemGuid))
            {
                var item = this.context.Items.FindItemByGuid(price.ItemId);
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
            var lastPrice = this.context.Items.FindItemByGuid(itemGuid);

            PriceEditorViewModel priceEditor = new PriceEditorViewModel(
                lastPrice.Name,
                lastPrice.ImageContent,
                lastPrice.Price,
                lastPrice.Brand,
                itemGuid);

            return priceEditor;
        }

        private void AddFirstPrice(Guid itemGuid)
        {
            Price firstPrice = new Price(
                this.TableCountPlusOne(),
                this.context.Items.FindItemByGuid(itemGuid).Price,
                itemGuid);

            this.context.Prices.Add(firstPrice);
        }

        private void AddChangedPrice(Guid itemGuid, PriceEditorViewModel priceEditor)
        {
            Price price = new Price(
                   this.TableCountPlusOne(),
                   priceEditor.CurrentPrice,
                   itemGuid);

            this.context.Prices.Add(price);
        }

        private void UpdatePrice(Guid itemGuid, PriceEditorViewModel priceEditor)
        {
            Item entity = this.context.Items.FindItemByGuid(itemGuid);
            entity.Price = priceEditor.CurrentPrice;
            this.context.Entry(entity).State = EntityState.Modified;
        }

        private bool AnyPricesById(Guid itemGuid)
        {
            return this.context.Prices
                .Any(model => model.ItemId == itemGuid);
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
    }
}

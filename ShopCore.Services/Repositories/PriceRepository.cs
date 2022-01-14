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

        public PriceEditorViewModel GetPriceEditor(Guid buttonPassingId)
        {
            var elementWithData = this.FindElementById(buttonPassingId);
            PriceEditorViewModel objectPriceEditor = new PriceEditorViewModel();
            objectPriceEditor.ItemName = elementWithData.Name;
            objectPriceEditor.ImageContent = elementWithData.ImageContent;
            objectPriceEditor.ItemPrice = elementWithData.Price;
            objectPriceEditor.ItemBrand = elementWithData.Brand;
            objectPriceEditor.ItemId = buttonPassingId;

            return objectPriceEditor;
        }

        public bool IfAnyPricesInDatabase(Guid itemId)
        {
            return this.context.Prices
                .Any(model => model.ItemId == itemId
                .ToString());
        }

        public Item FindOriginalPrice(Guid itemId)
        {
            return this.context.Items
                .FirstOrDefault(item => item.Id == itemId);
        }

        public int TableCount()
        {
            return this.context.Prices.Count() + 1;
        }

        public void AddFirstPrice(Guid itemId)
        {
            Price objectFirstPrice = new Price();
            objectFirstPrice.Id = this.TableCount();
            objectFirstPrice.ItemId = itemId.ToString();
            objectFirstPrice.PriceValue = this.FindOriginalPrice(itemId).Price;
            objectFirstPrice.Date = DateTime.Now;
            this.context.Prices.Add(objectFirstPrice);
        }

        public void UpdatePrice(Guid buttonPassingId, PriceEditorViewModel objectItem)
        {
            Item entity = this.FindOriginalPrice(buttonPassingId);
            entity.Price = objectItem.CurrentPrice;
            this.context.Entry(entity).State = EntityState.Modified;
        }

        public IEnumerable<Price> GetPriceHistoryById(Guid itemId)
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

        public void AddChangedPrice(Guid buttonPassingId, PriceEditorViewModel objectItem)
        {
            Price objectPrice = new Price();
            objectPrice.Id = this.TableCount();
            objectPrice.ItemId = buttonPassingId.ToString();
            objectPrice.PriceValue = objectItem.CurrentPrice;
            objectPrice.Date = DateTime.Now;
            this.context.Prices.Add(objectPrice);
        }

        public void ChangePrice(PriceEditorViewModel objectItem, Guid buttonPassingId)
        {
            bool hasAnyPrice = this.IfAnyPricesInDatabase(buttonPassingId);
            if (!hasAnyPrice)
            {
                this.AddFirstPrice(buttonPassingId);
                this.context.SaveChanges();
            }

            this.UpdatePrice(buttonPassingId, objectItem);
            this.AddChangedPrice(buttonPassingId, objectItem);
            this.context.SaveChanges();
        }

        public List<PriceHistoryViewModel> GetPriceHistory(List<PriceHistoryViewModel> listOfItemsHistory, Guid buttonPassingId)
        {
            foreach (var order in this.GetPriceHistoryById(buttonPassingId))
            {
                PriceHistoryViewModel objectPriceHistoryModel = new PriceHistoryViewModel();
                objectPriceHistoryModel.ItemId = order.ItemId;
                objectPriceHistoryModel.CurrentPrice = order.PriceValue;
                objectPriceHistoryModel.Date = order.Date;

                var findElementById = this.FindItemById(buttonPassingId, objectPriceHistoryModel);
                objectPriceHistoryModel.ImageContent = findElementById.ImageContent;
                objectPriceHistoryModel.ItemBrand = findElementById.Brand;
                objectPriceHistoryModel.ItemName = findElementById.Name;

                listOfItemsHistory.Add(objectPriceHistoryModel);
            }

            return listOfItemsHistory;
        }
    }
}

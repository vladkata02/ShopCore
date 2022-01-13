namespace ShopCore.Services.Interfaces
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using ShopCore.Data.Models;
    using ShopCore.Services.ViewModel;

    public interface IPriceRepository
    {
        Item FindElementById(Guid itemId);

        bool IfAnyPricesInDatabase(Guid itemId);

        Item CheckOriginalPrice(Guid itemId);

        void Save();

        int TableCount();

        void AddFirstPrice(Price objFirstPrice);

        void AddChangedPrice(Price objPrice);

        void UpdatePrice(Item entity);

        IEnumerable<Price> GetPriceHistoryById(Guid itemId);

        Item FindItemById(Guid itemId, PriceHistoryViewModel objPriceHistoryModel);
    }
}

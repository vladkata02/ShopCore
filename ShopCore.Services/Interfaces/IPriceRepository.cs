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

        PriceEditorViewModel GetPriceEditor(Guid buttonPassingId);

        bool IfAnyPricesInDatabase(Guid itemId);

        Item FindOriginalPrice(Guid itemId);

        int TableCount();

        void AddFirstPrice(Guid itemId);

        void AddChangedPrice(Guid itemId, PriceEditorViewModel objectItem);

        void UpdatePrice(Guid buttonPassingId, PriceEditorViewModel objectItem);

        IEnumerable<Price> GetPriceHistoryById(Guid itemId);

        Item FindItemById(Guid itemId, PriceHistoryViewModel objPriceHistoryModel);

        void ChangePrice(PriceEditorViewModel objectItem, Guid buttonPassingId);

        List<PriceHistoryViewModel> GetPriceHistory(List<PriceHistoryViewModel> listOfItemsHistory, Guid buttonPassingId);
    }
}

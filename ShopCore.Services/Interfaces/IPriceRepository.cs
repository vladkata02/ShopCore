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
        PriceEditorViewModel GetPriceEditor(Guid itemGuid);

        List<PriceHistoryViewModel> GetPriceHistory(List<PriceHistoryViewModel> listOfItemsHistory, Guid itemGuid);

        void ChangePrice(PriceEditorViewModel priceEditor, Guid itemGuid);
    }
}

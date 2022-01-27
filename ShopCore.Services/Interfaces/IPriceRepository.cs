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
        void ChangePrice(PriceEditorViewModel priceEditor, Guid itemGuid);

        PriceEditorViewModel GetPriceEditor(Guid itemGuid);

        List<PriceHistoryViewModel> GetPriceHistory(Guid itemGuid);
    }
}

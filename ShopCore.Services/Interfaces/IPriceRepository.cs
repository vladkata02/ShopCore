﻿namespace ShopCore.Services.Interfaces
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

        void AddFirstPrice(Guid itemGuid);

        bool IfAnyPricesInDatabase(Guid itemGuid);

        void AddChangedPrice(Guid itemGuid, PriceEditorViewModel priceEditor);

        void UpdatePrice(Guid itemGuid, PriceEditorViewModel priceEditor);
    }
}

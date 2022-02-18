namespace ShopCore.Services.Interfaces
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using ShopCore.Data.Models;
    using ShopCore.Services.ViewModel;

    public interface IShoppingHistoryRepository
    {
        List<ShoppingHistoryViewModel> GetShoppingHistory(string email, string typeLogin);
    }
}

namespace ShopCore.Services.Interfaces
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using ShopCore.Data.Models;
    using ShopCore.Services.ViewModel;

    public interface IShoppingRepository
    {
        IEnumerable<ShoppingViewModel> GetItems();

        void AddItemToCart(Guid itemId, string userName);

        List<ShoppingCartViewModel> DisplayShoppingCart(string userName);

        int AddOrderTime();

        void AddOrder(string userName, int orderId, List<ShoppingCartViewModel> receiptForMail);

        void ClearCart(string userName);

        List<ShoppingHistoryViewModel> GetShoppingHistory(string userName);
    }
}

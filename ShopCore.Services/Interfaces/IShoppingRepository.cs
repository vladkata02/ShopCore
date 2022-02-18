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

        void AddItemToCart(Guid itemId, string email, string typeLogin);

        List<ShoppingCartViewModel> DisplayShoppingCart(string email, string typeLogin);

        int AddOrderTime();

        void AddOrder(int orderId, List<ShoppingCartViewModel> receiptForMail, string email, string typeLogin);

        void ClearCart(string email, string typeLogin);

        List<ShoppingHistoryViewModel> GetShoppingHistory(string email, string typeLogin);
    }
}

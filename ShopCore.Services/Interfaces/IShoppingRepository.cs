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

        void AddItemToCart(string itemId, string userName);

        void DisplayShoppingCart(List<ShoppingCartViewModel> list, string userName);

        Item FindItemById(string itemId);

        Cart IfItemExistInCartById(string itemId, string userName);

        void AddToCartItem(Cart objShoppingCartModel);

        Cart FindItemQuantityById(string itemId, string userName);

        IEnumerable<Cart> GetWhichAccoutCartIs(string userName);

        Item FindElementById(Cart cart);

        int AddOrderTime();

        void AddOrderDetails(OrderDetail objOrderDetail);

        Order FindDateById(OrderDetail order);

        Item FindItemByIdForOrders(OrderDetail order);

        IEnumerable<OrderDetail> FindAccOrders(string userName);

        void ClearCart(string userName);

        int TableCount();

        void AddOrder(string userName, int orderId, List<ShoppingCartViewModel> receiptForMail);

        public List<ShoppingHistoryModel> GetShoppingHistory(string userName, List<ShoppingHistoryModel> listOfShoppingHistory);

        void Save();
    }
}

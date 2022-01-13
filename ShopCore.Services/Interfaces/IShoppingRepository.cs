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
        IEnumerable<Category> GetCategories();

        IEnumerable<Item> GetItems();

        Item FindItemById(string itemId);

        Cart IfItemExistInCartById(string itemId, string userName);

        void AddToCartItem(Cart objShoppingCartModel);

        Cart FindItemQuantityById(string itemId, string userName);

        IEnumerable<Cart> GetWhichAccoutCartIs(string userName);

        Item FindElementById(Cart cart);

        void AddOrderTime(Order orderObj);

        void AddOrderDetails(OrderDetail objOrderDetail);

        Order FindDateById(OrderDetail order);

        Item FindItemByIdForOrders(OrderDetail order);

        IEnumerable<OrderDetail> FindAccOrders(string userName);

        void RemoveItem(Cart item);

        int TableCount();

        void Save();
    }
}

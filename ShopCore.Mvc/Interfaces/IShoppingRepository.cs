using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ShopCore.Models;
using ShopCore.ViewModel;

namespace ShopCore.Mvc.Interfaces
{
    public interface IShoppingRepository
    {
        IEnumerable<Category> GetCategories();

        IEnumerable<Item> GetItems();

        Item CheckId(string itemId);

        Cart IfCheckId(string itemId, string userName);

        void AddCartItem(Cart objShoppingCartModel);

        Cart CheckIdForQuantity(string itemId, string userName);

        IEnumerable<Cart> CheckWhichAccCartIs(string userName);

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

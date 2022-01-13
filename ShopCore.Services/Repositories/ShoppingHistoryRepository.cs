namespace ShopCore.Services.Repositories
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Linq.Expressions;
    using System.Threading.Tasks;
    using Microsoft.EntityFrameworkCore;
    using ShopCore.Data.Context;
    using ShopCore.Data.Models;
    using ShopCore.Services.Interfaces;
    using ShopCore.Services.ViewModel;

    public class ShoppingHistoryRepository : IShoppingHistoryRepository
    {
        private ShopDBContext context;

        public ShoppingHistoryRepository(ShopDBContext context)
        {
            this.context = context;
        }

        public IEnumerable<OrderDetail> FindAccOrders(string userName)
        {
            return this.context.OrderDetails
                .Where(element => element.Account == userName);
        }

        public Order FindDateById(OrderDetail order)
        {
            return this.context.Orders
                .Where(check => check.Id == order.OrderId)
                .FirstOrDefault();
        }

        public Item FindItemByIdForOrders(OrderDetail order)
        {
            return this.context.Items
                .Where(check => check.Id.ToString() == order.ItemId)
                .FirstOrDefault();
        }
    }
}

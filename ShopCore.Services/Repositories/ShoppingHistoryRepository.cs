namespace ShopCore.Services.Repositories
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Linq.Expressions;
    using System.Threading.Tasks;
    using Microsoft.EntityFrameworkCore;
    using ShopCore.Data.Models;
    using ShopCore.Services.Context;
    using ShopCore.Services.Interfaces;
    using ShopCore.Services.ViewModel;

    internal class ShoppingHistoryRepository : IShoppingHistoryRepository
    {
        private ShopDBContext context;

        public ShoppingHistoryRepository(ShopDBContext context)
        {
            this.context = context;
        }

        public List<ShoppingHistoryViewModel> GetShoppingHistory(string userName)
        {
            List<ShoppingHistoryViewModel> listOfShoppingHistory = new List<ShoppingHistoryViewModel>();
            foreach (var order in this.FindAccountOrders(userName))
            {
                var foundDate = this.FindDateById(order.OrderId);
                var findElementById = this.FindItemByIdForOrders(order.ItemId);
                ShoppingHistoryViewModel objectShoppingHistoryModel = new ShoppingHistoryViewModel(
                    order.Id,
                    order.OrderId,
                    order.ItemId,
                    order.UnitPrice,
                    order.Total,
                    foundDate.Date,
                    findElementById.ImageContent,
                    findElementById.Brand,
                    findElementById.Name,
                    order.Quantity,
                    userName);

                listOfShoppingHistory.Add(objectShoppingHistoryModel);
            }

            return listOfShoppingHistory;
        }

        private IEnumerable<OrderDetail> FindAccountOrders(string userName)
        {
            return this.context.OrderDetails
                .Where(element => element.Account == userName);
        }

        private Order FindDateById(int orderId)
        {
            return this.context.Orders
                .Where(check => check.Id == orderId)
                .FirstOrDefault();
        }

        private Item FindItemByIdForOrders(Guid itemId)
        {
            return this.context.Items
                .Where(check => check.Id == itemId)
                .FirstOrDefault();
        }
    }
}

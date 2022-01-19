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

        public List<ShoppingHistoryViewModel> GetShoppingHistory(string userName, List<ShoppingHistoryViewModel> listOfShoppingHistory)
        {
            foreach (var order in this.FindAccOrders(userName))
            {
                ShoppingHistoryViewModel objectShoppingHistoryModel = new ShoppingHistoryViewModel();
                objectShoppingHistoryModel.OrderDetailId = order.Id;
                objectShoppingHistoryModel.OrderNumber = order.OrderId;
                objectShoppingHistoryModel.ItemId = order.ItemId;
                objectShoppingHistoryModel.UnitPrice = order.UnitPrice;
                objectShoppingHistoryModel.Total = order.Total;

                var foundDate = this.FindDateById(order);

                objectShoppingHistoryModel.OrderDate = foundDate.Date;

                var findElementById = this.FindItemByIdForOrders(order);
                objectShoppingHistoryModel.ItemBrand = findElementById.Brand;
                objectShoppingHistoryModel.ItemName = findElementById.Name;
                objectShoppingHistoryModel.Quantity = order.Quantity;
                objectShoppingHistoryModel.Account = userName;

                listOfShoppingHistory.Add(objectShoppingHistoryModel);
            }

            return listOfShoppingHistory;
        }

        private IEnumerable<OrderDetail> FindAccOrders(string userName)
        {
            return this.context.OrderDetails
                .Where(element => element.Account == userName);
        }

        private Order FindDateById(OrderDetail order)
        {
            return this.context.Orders
                .Where(check => check.Id == order.OrderId)
                .FirstOrDefault();
        }

        private Item FindItemByIdForOrders(OrderDetail order)
        {
            return this.context.Items
                .Where(check => check.Id.ToString() == order.ItemId)
                .FirstOrDefault();
        }
    }
}

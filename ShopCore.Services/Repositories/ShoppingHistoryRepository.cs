namespace ShopCore.Services.Repositories
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Linq.Expressions;
    using System.Threading.Tasks;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.Logging;
    using ShopCore.Data.Models;
    using ShopCore.Services.Context;
    using ShopCore.Services.Interfaces;
    using ShopCore.Services.ViewModel;

    internal class ShoppingHistoryRepository : IShoppingHistoryRepository
    {
        private readonly ILogger<ShoppingHistoryRepository> logger;
        private ShopDBContext context;
        private IUnitOfWork unitOfWork;

        public ShoppingHistoryRepository(ShopDBContext context, IUnitOfWork unitOfWork, ILogger<ShoppingHistoryRepository> logger)
        {
            this.logger = logger;
            this.context = context;
            this.unitOfWork = unitOfWork;
        }

        public List<ShoppingHistoryViewModel> GetShoppingHistory(string email, string typeLogin)
        {
            List<ShoppingHistoryViewModel> listOfShoppingHistory = new List<ShoppingHistoryViewModel>();
            foreach (var order in this.FindAccountOrders(email, typeLogin))
            {
                var foundDate = this.FindOrderById(order.OrderId);
                var findElementById = this.context.Items.FindItemByGuid(order.ItemId);
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
                    typeLogin,
                    email);

                listOfShoppingHistory.Add(objectShoppingHistoryModel);
            }

            return listOfShoppingHistory;
        }

        private IEnumerable<OrderDetail> FindAccountOrders(string email, string typeLogin)
        {
            return this.context.OrderDetails
                .Where(element => element.Email == email && element.TypeLogin == typeLogin);
        }

        private Order FindOrderById(int orderId)
        {
            return this.context.Orders
                .Where(check => check.Id == orderId)
                .FirstOrDefault();
        }
    }
}

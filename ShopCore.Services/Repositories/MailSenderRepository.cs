namespace ShopCore.Services.Repositories
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using Microsoft.Extensions.Logging;
    using ShopCore.Data.Models;
    using ShopCore.Services.Context;
    using ShopCore.Services.Interfaces;
    using ShopCore.Services.ViewModel;

    internal class MailSenderRepository : IMailSenderRepository
    {
        private readonly ILogger<MailSenderRepository> logger;
        private ShopDBContext context;

        public MailSenderRepository(ShopDBContext context, ILogger<MailSenderRepository> logger)
        {
            this.logger = logger;
            this.context = context;
        }

        public void GetTodaysOrders(List<ShoppingHistoryViewModel> itemsSoldForTheDay)
        {
            foreach (var order in this.context.OrderDetails)
            {
                var foundDate = this.FindOrderById(order.OrderId);
                if (foundDate.Date < DateTime.Today)
                {
                    continue;
                }
                else
                {
                    var foundElementByGuid = this.context.Items.FindItemByGuid(order.ItemId);
                    ShoppingHistoryViewModel todayShoppingHistory = new ShoppingHistoryViewModel(
                    order.Id,
                    order.OrderId,
                    order.ItemId,
                    order.UnitPrice,
                    order.Total,
                    foundDate.Date,
                    foundElementByGuid.ImageContent,
                    foundElementByGuid.Brand,
                    foundElementByGuid.Name,
                    order.Quantity,
                    order.Account);

                    itemsSoldForTheDay.Add(todayShoppingHistory);
                }
            }
        }

        private Order FindOrderById(int orderId)
        {
            return this.context.Orders
                .Where(check => check.Id == orderId)
                .FirstOrDefault();
        }
    }
}

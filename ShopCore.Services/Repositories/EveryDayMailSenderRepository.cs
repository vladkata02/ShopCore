namespace ShopCore.Services.Repositories
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using ShopCore.Data.Models;
    using ShopCore.Services.Context;
    using ShopCore.Services.Interfaces;
    using ShopCore.Services.ViewModel;

    internal class EveryDayMailSenderRepository : IEveryDayMailSenderRepository
    {
        private ShopDBContext context;

        public EveryDayMailSenderRepository(ShopDBContext context)
        {
            this.context = context;
        }

        public List<ShoppingHistoryViewModel> GetTodaysOrders()
        {
            List<ShoppingHistoryViewModel> listOfTodayShoppingHistory = new List<ShoppingHistoryViewModel>();
            foreach (var order in this.GetTodayOrderDetailsById())
            {
                var foundDate = this.FindOrderById(order.OrderId);
                var findElementById = this.context.Items.FindItemByGuid(order.ItemId);
                ShoppingHistoryViewModel todayShoppingHistoryModel = new ShoppingHistoryViewModel(
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
                    order.Account);

                listOfTodayShoppingHistory.Add(todayShoppingHistoryModel);
            }

            return listOfTodayShoppingHistory;
        }

        private IEnumerable<OrderDetail> GetTodayOrderDetailsById()
        {
            IEnumerable<Order> order = this.GetTodayOrdersByDate();
            return this.context.OrderDetails.Where(x => order.Any(y => y.Id == x.OrderId));
        }

        private IEnumerable<Order> GetTodayOrdersByDate()
        {
            return this.context.Orders
                .Where(element => element.Date == DateTime.Today);
        }

        private Order FindOrderById(int orderId)
        {
            return this.context.Orders
                .Where(check => check.Id == orderId)
                .FirstOrDefault();
        }
    }
}

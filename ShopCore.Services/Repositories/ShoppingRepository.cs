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

    internal class ShoppingRepository : IShoppingRepository
    {
        private readonly ILogger<ShoppingRepository> logger;
        private ShopDBContext context;
        private IUnitOfWork unitOfWork;

        public ShoppingRepository(ShopDBContext context, IUnitOfWork unitOfWork, ILogger<ShoppingRepository> logger)
        {
            this.logger = logger;
            this.context = context;
            this.unitOfWork = unitOfWork;
        }

        public IEnumerable<ShoppingViewModel> GetItems()
        {
            return (from item in this.context.Items
                    join
                     category in this.context.Categories
            on item.CategoryId equals category.Id
                    select new ShoppingViewModel()
                    {
                        ItemName = item.Name,
                        ImageContent = item.ImageContent,
                        Description = item.Description,
                        ItemPrice = item.Price,
                        ItemBrand = item.Brand,
                        ItemId = item.Id,
                        Category = category.Name,
                        ItemCode = item.Code,
                    }).ToList();
        }

        public void AddItemToCart(Guid itemId, string userName)
        {
            Item item = this.context.Items.FindItemByGuid(itemId);

            bool ifAnyItemExistId = this.IfItemExistInCartById(itemId, userName);
            if (!ifAnyItemExistId)
            {
                Cart shoppingCart = new Cart(
                    this.TableCountPlusOne(),
                    itemId,
                    item.Name,
                    item.Price,
                    userName,
                    item.ImageContent);

                this.AddToCartItem(shoppingCart);
            }
            else
            {
                var foundItem = this.FindCartItemByIdAndUserName(itemId, userName);
                foundItem.Quantity++;
                foundItem.Total = foundItem.Quantity * foundItem.UnitPrice;
            }
        }

        public List<ShoppingCartViewModel> DisplayShoppingCart(string userName)
        {
            List<ShoppingCartViewModel> listOfCartItems = new List<ShoppingCartViewModel>();
            foreach (var cartItem in this.GetAccountRelatedCarts(userName))
            {
                var findElementById = this.context.Items.FindItemByGuid(cartItem.ItemId);
                ShoppingCartViewModel cart = new ShoppingCartViewModel(
                    cartItem.ItemId,
                    cartItem.UnitPrice,
                    cartItem.Total,
                    findElementById.ImageContent,
                    findElementById.Brand,
                    findElementById.Name,
                    cartItem.Quantity,
                    userName);

                listOfCartItems.Add(cart);
            }

            return listOfCartItems;
        }

        public int AddOrderTime()
        {
            Order order = new Order();

            this.context.Orders.Add(order);
            this.context.SaveChanges();
            return order.Id;
        }

        public void AddOrder(string userName, int orderId, List<ShoppingCartViewModel> receiptForMail)
        {
            foreach (var item in this.GetAccountRelatedCarts(userName))
            {
                OrderDetail orderDetails = new OrderDetail(
                    item.Total,
                    item.ItemId,
                    orderId,
                    item.Quantity,
                    item.UnitPrice,
                    userName);

                var currentItem = this.context.Items.FindItemByGuid(item.ItemId);
                ShoppingCartViewModel cartForMail = new ShoppingCartViewModel(
                    item.ItemId,
                    item.UnitPrice,
                    item.Total,
                    currentItem.ImageContent,
                    currentItem.Brand,
                    currentItem.Name,
                    item.Quantity,
                    userName);

                receiptForMail.Add(cartForMail);

                this.context.OrderDetails.Add(orderDetails);
            }
        }

        public void ClearCart(string userName)
        {
            foreach (var item in this.GetAccountRelatedCarts(userName))
            {
                this.context.Carts.Remove(item);
            }
        }

        public List<ShoppingHistoryViewModel> GetShoppingHistory(string userName)
        {
            List<ShoppingHistoryViewModel> listOfShoppingHistory = new List<ShoppingHistoryViewModel>();
            foreach (var order in this.GetAccountOrders(userName))
            {
                var foundDate = this.FindOrderById(order.OrderId);
                var findElementById = this.context.Items.FindItemByGuid(order.ItemId);
                ShoppingHistoryViewModel shoppingHistoryModel = new ShoppingHistoryViewModel(
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

                listOfShoppingHistory.Add(shoppingHistoryModel);
            }

            return listOfShoppingHistory;
        }

        private Order FindOrderById(int orderId)
        {
            return this.context.Orders
                .Where(check => check.Id == orderId)
                .FirstOrDefault();
        }

        private bool IfItemExistInCartById(Guid itemId, string userName)
        {
            return this.context.Carts
                .Any(model => model.ItemId == itemId && model.Account == userName);
        }

        private int TableCountPlusOne()
        {
            return this.context.Carts.Count() + 1;
        }

        private void AddToCartItem(Cart objShoppingCartModel)
        {
            this.context.Carts.Add(objShoppingCartModel);
        }

        private Cart FindCartItemByIdAndUserName(Guid itemId, string userName)
        {
            return this.context.Carts
                .Single(model => model.ItemId == itemId && model.Account == userName);
        }

        private IEnumerable<Cart> GetAccountRelatedCarts(string userName)
        {
            return this.context.Carts
                .Where(element => element.Account == userName);
        }

        private IEnumerable<OrderDetail> GetAccountOrders(string userName)
        {
            return this.context.OrderDetails
                .Where(element => element.Account == userName);
        }
    }
}

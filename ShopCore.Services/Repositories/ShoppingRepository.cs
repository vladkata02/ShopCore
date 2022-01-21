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

    internal class ShoppingRepository : IShoppingRepository
    {
        private ShopDBContext context;

        public ShoppingRepository(ShopDBContext context)
        {
            this.context = context;
        }

        public IEnumerable<ShoppingViewModel> GetItems()
        {
            // TODO remove word object from variables names
            return (from objectItem in this.context.Items
                    join
                    objectCategory in this.context.Categories
            on objectItem.CategoryId equals objectCategory.Id
                    select new ShoppingViewModel()
                    {
                        ItemName = objectItem.Name,
                        ImageContent = objectItem.ImageContent,
                        Description = objectItem.Description,
                        ItemPrice = objectItem.Price,
                        ItemBrand = objectItem.Brand,
                        ItemId = objectItem.Id,
                        Category = objectCategory.Name,
                        ItemCode = objectItem.Code,
                    }).ToList();
        }

        public void AddItemToCart(string itemId, string userName)
        {
            // TODO Extract creation logic in constructor
            // TODO remove word object from variables names
            Cart shoppingCart = new Cart();
            Item objectItem = this.FindItemById(itemId);

            var ifAnyItemExistId = this.IfItemExistInCartById(itemId, userName);
            if (ifAnyItemExistId == null)
            {
                shoppingCart.Id = this.TableCount();
                shoppingCart.ItemId = itemId;
                shoppingCart.ImageContent = objectItem.ImageContent;
                shoppingCart.ItemName = objectItem.Name;
                shoppingCart.Quantity = 1;
                shoppingCart.Total = objectItem.Price;
                shoppingCart.Account = userName;
                shoppingCart.UnitPrice = objectItem.Price;
                this.AddToCartItem(shoppingCart);
            }
            else
            {
                var foundItem = this.FindItemQuantityById(itemId, userName);
                foundItem.Quantity++;
                foundItem.Total = foundItem.Quantity * foundItem.UnitPrice;
            }
        }

        public List<ShoppingCartViewModel> DisplayShoppingCart(string userName)
        {
            List<ShoppingCartViewModel> listOfCartItems = new List<ShoppingCartViewModel>();
            foreach (var cartItem in this.GetWhichAccoutCartIs(userName))
            {
                // TODO Extract creation logic in constructor
                // TODO remove word obj from variable name
                ShoppingCartViewModel cart = new ShoppingCartViewModel();
                cart.ItemId = cartItem.ItemId;
                cart.UnitPrice = cartItem.UnitPrice;
                cart.Total = cartItem.Total;

                var findElementById = this.FindElementById(cartItem);
                cart.ImageContent = findElementById.ImageContent;
                cart.ItemBrand = findElementById.Brand;
                cart.ItemName = findElementById.Name;
                cart.Quantity = cart.Quantity;
                cart.Account = userName;

                listOfCartItems.Add(cart);
            }

            return listOfCartItems;
        }

        public int AddOrderTime()
        {
            // TODO Extract creation logic in constructor
            // TODO remove word object from variable name
            Order order = new Order()
            {
                Date = DateTime.Now,
                Number = string.Format("{0:ddmmyyyyyHHmmsss}", DateTime.Now),
            };
            this.context.Orders.Add(order);
            this.context.SaveChanges();
            return order.Id;
        }

        public void AddOrder(string userName, int orderId, List<ShoppingCartViewModel> receiptForMail)
        {
            foreach (var item in this.GetWhichAccoutCartIs(userName))
            {
                // TODO Extract creation logic in constructor
                OrderDetail orderDetails = new OrderDetail();
                orderDetails.Total = item.Total;
                orderDetails.ItemId = item.ItemId;
                orderDetails.OrderId = orderId;
                orderDetails.Quantity = item.Quantity;
                orderDetails.UnitPrice = item.UnitPrice;
                orderDetails.Account = userName;

                // TODO Extract creation logic in constructor
                ShoppingCartViewModel cartForMail = new ShoppingCartViewModel();
                cartForMail.ItemId = item.ItemId;
                cartForMail.UnitPrice = item.UnitPrice;
                cartForMail.Total = item.Total;

                var currentElement = this.FindElementById(item);
                cartForMail.ImageContent = currentElement.ImageContent;
                cartForMail.ItemBrand = currentElement.Brand;
                cartForMail.ItemName = currentElement.Name;
                cartForMail.Quantity = item.Quantity;
                cartForMail.Account = userName;

                receiptForMail.Add(cartForMail);

                this.context.OrderDetails.Add(orderDetails);
            }
        }

        public void ClearCart(string userName)
        {
            foreach (var item in this.GetWhichAccoutCartIs(userName))
            {
                this.context.Carts.Remove(item);
            }
        }

        public List<ShoppingHistoryViewModel> GetShoppingHistory(string userName)
        {
            List<ShoppingHistoryViewModel> listOfShoppingHistory = new List<ShoppingHistoryViewModel>();
            foreach (var order in this.FindAccOrders(userName))
            {
                // TODO Extract creation logic in constructor
                ShoppingHistoryViewModel shoppingHistoryModel = new ShoppingHistoryViewModel();
                shoppingHistoryModel.OrderDetailId = order.Id;
                shoppingHistoryModel.OrderNumber = order.OrderId;
                shoppingHistoryModel.ItemId = order.ItemId;
                shoppingHistoryModel.UnitPrice = order.UnitPrice;
                shoppingHistoryModel.Total = order.Total;

                var foundDate = this.FindOrderDateById(order);

                shoppingHistoryModel.OrderDate = foundDate.Date;

                var findElementById = this.FindItemByIdForOrders(order);
                shoppingHistoryModel.ImageContent = findElementById.ImageContent;
                shoppingHistoryModel.ItemBrand = findElementById.Brand;
                shoppingHistoryModel.ItemName = findElementById.Name;
                shoppingHistoryModel.Quantity = order.Quantity;
                shoppingHistoryModel.Account = userName;

                listOfShoppingHistory.Add(shoppingHistoryModel);
            }

            return listOfShoppingHistory;
        }

        private Order FindOrderDateById(OrderDetail order)
        {
            return this.context.Orders
                .Where(check => check.Id == order.OrderId)
                .FirstOrDefault();
        }

        private Item FindItemById(string itemId)
        {
            // TODO Refactor class types, should not convert int/guid to string for comaprison
            return this.context.Items
                .Single(model => model.Id.ToString() == itemId);
        }

        // TODO method name implies boolean result
        private Cart IfItemExistInCartById(string itemId, string userName)
        {
            return this.context.Carts
                .SingleOrDefault(model => model.ItemId == itemId && model.Account == userName);
        }

        // TODO Method name is lying
        private int TableCount()
        {
            return this.context.Carts.Count() + 1;
        }

        private void AddToCartItem(Cart objShoppingCartModel)
        {
            this.context.Carts.Add(objShoppingCartModel);
        }

        // TODO Method name is lying
        private Cart FindItemQuantityById(string itemId, string userName)
        {
            return this.context.Carts
                .Single(model => model.ItemId == itemId && model.Account == userName);
        }

        // TODO find better name i.e GetAccountRelatedCarts
        private IEnumerable<Cart> GetWhichAccoutCartIs(string userName)
        {
            return this.context.Carts
                .Where(element => element.Account == userName);
        }

        // NOTICE if pass itemId instead of the whole cart, method will became more reusable, in fact the method FindItemById can be used
        private Item FindElementById(Cart cart)
        {
            // TODO Refactor class types, should not convert int/guid to string for comaprison
            return this.context.Items
                .Where(check => check.Id.ToString() == cart.ItemId)
                .FirstOrDefault();
        }

        // TODO do not shorten method names
        private IEnumerable<OrderDetail> FindAccOrders(string userName)
        {
            return this.context.OrderDetails
                .Where(element => element.Account == userName);
        }

        // NOTICE if pass itemId instead of the whole order, method will became more reusable, in fact the method FindItemById can be used
        private Item FindItemByIdForOrders(OrderDetail order)
        {
            // TODO Refactor class types, should not convert int/guid to string for comaprison
            return this.context.Items
                .Where(check => check.Id.ToString() == order.ItemId)
                .FirstOrDefault();
        }
    }
}

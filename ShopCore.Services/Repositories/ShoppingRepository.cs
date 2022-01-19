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
            Cart objectShoppingCart = new Cart();
            Item objectItem = this.FindItemById(itemId);

            var ifAnyItemExistId = this.IfItemExistInCartById(itemId, userName);
            if (ifAnyItemExistId == null)
            {
                objectShoppingCart.Id = this.TableCount();
                objectShoppingCart.ItemId = itemId;
                objectShoppingCart.ImageContent = objectItem.ImageContent;
                objectShoppingCart.ItemName = objectItem.Name;
                objectShoppingCart.Quantity = 1;
                objectShoppingCart.Total = objectItem.Price;
                objectShoppingCart.Account = userName;
                objectShoppingCart.UnitPrice = objectItem.Price;
                this.AddToCartItem(objectShoppingCart);
            }
            else
            {
                var foundItem = this.FindItemQuantityById(itemId, userName);
                foundItem.Quantity++;
                foundItem.Total = foundItem.Quantity * foundItem.UnitPrice;
            }
        }

        public void DisplayShoppingCart(List<ShoppingCartViewModel> list, string userName)
        {
            foreach (var cart in this.GetWhichAccoutCartIs(userName))
            {
                ShoppingCartViewModel objCart = new ShoppingCartViewModel();
                objCart.ItemId = cart.ItemId;
                objCart.UnitPrice = cart.UnitPrice;
                objCart.Total = cart.Total;

                var findElementById = this.FindElementById(cart);
                objCart.ImageContent = findElementById.ImageContent;
                objCart.ItemBrand = findElementById.Brand;
                objCart.ItemName = findElementById.Name;
                objCart.Quantity = cart.Quantity;
                objCart.Account = userName;

                list.Add(objCart);
            }
        }

        public int AddOrderTime()
        {
            Order objectOrder = new Order()
            {
                Date = DateTime.Now,
                Number = string.Format("{0:ddmmyyyyyHHmmsss}", DateTime.Now),
            };
            this.context.Orders.Add(objectOrder);
            this.context.SaveChanges();
            return objectOrder.Id;
        }

        public void AddOrder(string userName, int orderId, List<ShoppingCartViewModel> receiptForMail)
        {
            foreach (var item in this.GetWhichAccoutCartIs(userName))
            {
                OrderDetail objectOrderDetails = new OrderDetail();
                objectOrderDetails.Total = item.Total;
                objectOrderDetails.ItemId = item.ItemId;
                objectOrderDetails.OrderId = orderId;
                objectOrderDetails.Quantity = item.Quantity;
                objectOrderDetails.UnitPrice = item.UnitPrice;
                objectOrderDetails.Account = userName;

                ShoppingCartViewModel objectCartForMail = new ShoppingCartViewModel();
                objectCartForMail.ItemId = item.ItemId;
                objectCartForMail.UnitPrice = item.UnitPrice;
                objectCartForMail.Total = item.Total;

                var currentElement = this.FindElementById(item);
                objectCartForMail.ImageContent = currentElement.ImageContent;
                objectCartForMail.ItemBrand = currentElement.Brand;
                objectCartForMail.ItemName = currentElement.Name;
                objectCartForMail.Quantity = item.Quantity;
                objectCartForMail.Account = userName;

                receiptForMail.Add(objectCartForMail);
                this.AddOrderDetails(objectOrderDetails);
            }
        }

        public void ClearCart(string userName)
        {
            foreach (var item in this.GetWhichAccoutCartIs(userName))
            {
                this.context.Carts.Remove(item);
            }
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

                var foundDate = this.FindOrderDateById(order);

                objectShoppingHistoryModel.OrderDate = foundDate.Date;

                var findElementById = this.FindItemByIdForOrders(order);
                objectShoppingHistoryModel.ImageContent = findElementById.ImageContent;
                objectShoppingHistoryModel.ItemBrand = findElementById.Brand;
                objectShoppingHistoryModel.ItemName = findElementById.Name;
                objectShoppingHistoryModel.Quantity = order.Quantity;
                objectShoppingHistoryModel.Account = userName;

                listOfShoppingHistory.Add(objectShoppingHistoryModel);
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
            return this.context.Items
                .Single(model => model.Id.ToString() == itemId);
        }

        private Cart IfItemExistInCartById(string itemId, string userName)
        {
            return this.context.Carts
                .SingleOrDefault(model => model.ItemId == itemId && model.Account == userName);
        }

        private int TableCount()
        {
            return this.context.Carts.Count() + 1;
        }

        private void AddToCartItem(Cart objShoppingCartModel)
        {
            this.context.Carts.Add(objShoppingCartModel);
        }

        private Cart FindItemQuantityById(string itemId, string userName)
        {
            return this.context.Carts
                .Single(model => model.ItemId == itemId && model.Account == userName);
        }

        private IEnumerable<Cart> GetWhichAccoutCartIs(string userName)
        {
            return this.context.Carts
                .Where(element => element.Account == userName);
        }

        private Item FindElementById(Cart cart)
        {
            return this.context.Items
                .Where(check => check.Id.ToString() == cart.ItemId)
                .FirstOrDefault();
        }

        private void AddOrderDetails(OrderDetail objOrderDetail)
        {
            this.context.OrderDetails.Add(objOrderDetail);
        }

        private IEnumerable<OrderDetail> FindAccOrders(string userName)
        {
            return this.context.OrderDetails
                .Where(element => element.Account == userName);
        }

        private Item FindItemByIdForOrders(OrderDetail order)
        {
            return this.context.Items
                .Where(check => check.Id.ToString() == order.ItemId)
                .FirstOrDefault();
        }
    }
}

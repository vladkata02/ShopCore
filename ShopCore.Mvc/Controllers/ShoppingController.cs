namespace ShopCore.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.Configuration;
    using ShopCore.Data;
    using ShopCore.Data.Context;
    using ShopCore.Data.Models;
    using ShopCore.Services.Interfaces;
    using ShopCore.Services.ViewModel;

    public class ShoppingController : Controller
    {
        private IShoppingRepository shoppingRepository;

        public ShoppingController(IShoppingRepository shoppingRepository)
        {
            this.shoppingRepository = shoppingRepository;
        }

        public IActionResult Index()
        {
            string userName = this.HttpContext.User.Identity.Name;
            this.TempData["username"] = userName;
            IEnumerable<ShoppingViewModel> listOfShoppingViewModels = (from objItem in this.shoppingRepository.GetItems()
                                                                       join
                                                                           objCate in this.shoppingRepository.GetCategories()
                                                                           on objItem.CategoryId equals objCate.CategoryId
                                                                       select new ShoppingViewModel()
                                                                       {
                                                                           ItemName = objItem.ItemName,
                                                                           Image = objItem.Image,
                                                                           Description = objItem.Description,
                                                                           ItemPrice = objItem.ItemPrice,
                                                                           ItemBrand = objItem.ItemBrand,
                                                                           ItemId = objItem.ItemId,
                                                                           Category = objCate.CategoryName,
                                                                           ItemCode = objItem.ItemCode,
                                                                       })
                                                                        .ToList();
            return this.View(listOfShoppingViewModels);
        }

        [HttpPost]
        public JsonResult Index(string itemId)
        {
            string userName = this.HttpContext.User.Identity.Name;
            this.TempData["username"] = userName;
            Cart objShoppingCartModel = new Cart();
            Item objItem = this.shoppingRepository.CheckId(itemId);

            var ifCheckId = this.shoppingRepository.IfCheckId(itemId, userName);
            if (ifCheckId == null)
            {
                objShoppingCartModel.CartId = this.shoppingRepository.TableCount();
                objShoppingCartModel.ItemId = itemId;
                objShoppingCartModel.Image = objItem.Image;
                objShoppingCartModel.ItemName = objItem.ItemName;
                objShoppingCartModel.Quantity = 1;
                objShoppingCartModel.Total = objItem.ItemPrice;
                objShoppingCartModel.CartAcc = userName;
                objShoppingCartModel.UnitPrice = objItem.ItemPrice;
                this.shoppingRepository.AddCartItem(objShoppingCartModel);
            }
            else
            {
                var checkId = this.shoppingRepository.CheckIdForQuantity(itemId, userName);
                checkId.Quantity++;
                checkId.Total = checkId.Quantity * checkId.UnitPrice;
            }

            this.shoppingRepository.Save();
            return this.Json(new { Success = true });
        }

        public IActionResult ShoppingCart()
        {
            string userName = this.HttpContext.User.Identity.Name;
            this.TempData["username"] = userName;
            List<ShoppingCartModel> list = new List<ShoppingCartModel>();
            foreach (var cart in this.shoppingRepository.CheckWhichAccCartIs(userName))
            {
                ShoppingCartModel objCart = new ShoppingCartModel();
                objCart.ItemId = cart.ItemId;
                objCart.UnitPrice = cart.UnitPrice;
                objCart.Total = cart.Total;

                var findElementById = this.shoppingRepository.FindElementById(cart);
                objCart.Image = findElementById.Image;
                objCart.ItemBrand = findElementById.ItemBrand;
                objCart.ItemName = findElementById.ItemName;
                objCart.Quantity = cart.Quantity;
                objCart.CartAcc = userName;

                list.Add(objCart);
            }

            return this.View(list);
        }

        [HttpPost]
        public IActionResult AddOrder()
        {
            string userName = this.HttpContext.User.Identity.Name;
            this.TempData["username"] = userName;
            int orderId = 0;
            Order orderObj = new Order()
            {
                OrderDate = DateTime.Now,
                OrderNumber = string.Format("{0:ddmmyyyyyHHmmsss}", DateTime.Now),
            };
            this.shoppingRepository.AddOrderTime(orderObj);
            this.shoppingRepository.Save();
            orderId = orderObj.OrderId;

            foreach (var item in this.shoppingRepository.CheckWhichAccCartIs(userName))
            {
                OrderDetail objOrderDetail = new OrderDetail();
                objOrderDetail.Total = item.Total;
                objOrderDetail.ItemId = item.ItemId;
                objOrderDetail.OrderId = orderId;
                objOrderDetail.Quantity = item.Quantity;
                objOrderDetail.UnitPrice = item.UnitPrice;
                objOrderDetail.OrderAccMail = userName;
                this.shoppingRepository.AddOrderDetails(objOrderDetail);
            }

            foreach (var item in this.shoppingRepository.CheckWhichAccCartIs(userName))
            {
                this.shoppingRepository.RemoveItem(item);
            }

            this.shoppingRepository.Save();
            return this.RedirectToAction("Index");
        }

        public IActionResult ShoppingHistory()
        {
            string userName = this.HttpContext.User.Identity.Name;
            this.TempData["username"] = userName;
            List<ShoppingHistoryModel> list = new List<ShoppingHistoryModel>();
            foreach (var order in this.shoppingRepository.FindAccOrders(userName))
            {
                ShoppingHistoryModel objShoppingHistoryModel = new ShoppingHistoryModel();
                objShoppingHistoryModel.OrderDetailId = order.OrderDetailId;
                objShoppingHistoryModel.OrderNumber = order.OrderId;
                objShoppingHistoryModel.ItemId = order.ItemId;
                objShoppingHistoryModel.UnitPrice = order.UnitPrice;
                objShoppingHistoryModel.Total = order.Total;

                var findDate = this.shoppingRepository.FindDateById(order);
                objShoppingHistoryModel.OrderDate = findDate.OrderDate;

                var findElementById = this.shoppingRepository.FindItemByIdForOrders(order);
                objShoppingHistoryModel.Image = findElementById.Image;
                objShoppingHistoryModel.ItemBrand = findElementById.ItemBrand;
                objShoppingHistoryModel.ItemName = findElementById.ItemName;
                objShoppingHistoryModel.Quantity = order.Quantity;
                objShoppingHistoryModel.User = userName;

                list.Add(objShoppingHistoryModel);
            }

            return this.View(list);
        }
    }
}
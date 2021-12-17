namespace ShopCore.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.Configuration;
    using ShopCore.Data;
    using ShopCore.Data.Context;
    using ShopCore.Models;
    using ShopCore.ViewModel;

    public class ShoppingController : Controller
    {
        private readonly ShopDBContext context;

        public ShoppingController(ShopDBContext context)
        {
            this.context = context;
        }

        public IActionResult Index()
        {
            string userName = this.HttpContext.User.Identity.Name;
            this.TempData["username"] = userName;
            IEnumerable<ShoppingViewModel> listOfShoppingViewModels = (from objItem in this.context.Items
                                                                       join
                                                                           objCate in this.context.Categories
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
            Item objItem = this.context.Items.Single(model => model.ItemId.ToString() == itemId);

            var ifCheckId = this.context.Carts.SingleOrDefault(model => model.ItemId == itemId && model.CartAcc == userName);
            if (ifCheckId == null)
            {
                objShoppingCartModel.CartId = this.context.Carts.Count() + 1;
                objShoppingCartModel.ItemId = itemId;
                objShoppingCartModel.Image = objItem.Image;
                objShoppingCartModel.ItemName = objItem.ItemName;
                objShoppingCartModel.Quantity = 1;
                objShoppingCartModel.Total = objItem.ItemPrice;
                objShoppingCartModel.CartAcc = userName;
                objShoppingCartModel.UnitPrice = objItem.ItemPrice;
                this.context.Carts.Add(objShoppingCartModel);
            }
            else
            {
                var checkId = this.context.Carts.Single(model => model.ItemId == itemId && model.CartAcc == userName);
                checkId.Quantity++;
                checkId.Total = checkId.Quantity * checkId.UnitPrice;
            }

            this.context.SaveChanges();
            return this.Json(new { Success = true });
        }

        public IActionResult ShoppingCart()
        {
            string userName = this.HttpContext.User.Identity.Name;
            this.TempData["username"] = userName;
            List<ShoppingCartModel> list = new List<ShoppingCartModel>();
            foreach (var cart in this.context.Carts.Where(element => element.CartAcc == userName))
            {
                ShoppingCartModel objCart = new ShoppingCartModel();
                objCart.ItemId = cart.ItemId;
                objCart.UnitPrice = cart.UnitPrice;
                objCart.Total = cart.Total;

                var findElementById = this.context.Items.Where(check => check.ItemId.ToString() == cart.ItemId).FirstOrDefault();
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
            this.context.Orders.Add(orderObj);
            this.context.SaveChanges();
            orderId = orderObj.OrderId;

            foreach (var item in this.context.Carts.Where(checkAcc => checkAcc.CartAcc == userName))
            {
                OrderDetail objOrderDetail = new OrderDetail();
                objOrderDetail.Total = item.Total;
                objOrderDetail.ItemId = item.ItemId;
                objOrderDetail.OrderId = orderId;
                objOrderDetail.Quantity = item.Quantity;
                objOrderDetail.UnitPrice = item.UnitPrice;
                objOrderDetail.OrderAccMail = userName;
                this.context.OrderDetails.Add(objOrderDetail);
            }

            foreach (var item in this.context.Carts.Where(checkAcc => checkAcc.CartAcc == userName))
            {
                this.context.Carts.Remove(item);
            }

            this.context.SaveChanges();
            return this.RedirectToAction("Index");
        }

        public IActionResult ShoppingHistory()
        {
            string userName = this.HttpContext.User.Identity.Name;
            this.TempData["username"] = userName;
            List<ShoppingHistoryModel> list = new List<ShoppingHistoryModel>();
            foreach (var order in this.context.OrderDetails.Where(element => element.OrderAccMail == userName))
            {
                ShoppingHistoryModel objShoppingHistoryModel = new ShoppingHistoryModel();
                objShoppingHistoryModel.OrderDetailId = order.OrderDetailId;
                objShoppingHistoryModel.OrderNumber = order.OrderId;
                objShoppingHistoryModel.ItemId = order.ItemId;
                objShoppingHistoryModel.UnitPrice = order.UnitPrice;
                objShoppingHistoryModel.Total = order.Total;

                var findDate = this.context.Orders.Where(check => check.OrderId == order.OrderId).FirstOrDefault();
                objShoppingHistoryModel.OrderDate = findDate.OrderDate;

                var findElementById = this.context.Items.Where(check => check.ItemId.ToString() == order.ItemId).FirstOrDefault();
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
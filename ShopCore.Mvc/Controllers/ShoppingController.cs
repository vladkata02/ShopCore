﻿namespace ShopCore.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using MailKit.Net.Smtp;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.Configuration;
    using MimeKit;
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
            IEnumerable<ShoppingViewModel> listOfShoppingViewModels = (from objItem in this.shoppingRepository.GetItems()
                                                                       join
                                                                           objCate in this.shoppingRepository.GetCategories()
                                                                           on objItem.CategoryId equals objCate.Id
                                                                       select new ShoppingViewModel()
                                                                       {
                                                                           ItemName = objItem.Name,
                                                                           Image = objItem.ImageContent,
                                                                           Description = objItem.Description,
                                                                           ItemPrice = objItem.Price,
                                                                           ItemBrand = objItem.Brand,
                                                                           ItemId = objItem.Id,
                                                                           Category = objCate.Name,
                                                                           ItemCode = objItem.Code,
                                                                       })
                                                                        .ToList();
            return this.View(listOfShoppingViewModels);
        }

        [HttpPost]
        public JsonResult Index(string itemId)
        {
            string userName = this.HttpContext.User.Identity.Name;
            Cart objShoppingCartModel = new Cart();
            Item objItem = this.shoppingRepository.CheckId(itemId);

            var ifCheckId = this.shoppingRepository.IfCheckId(itemId, userName);
            if (ifCheckId == null)
            {
                objShoppingCartModel.Id = this.shoppingRepository.TableCount();
                objShoppingCartModel.ItemId = itemId;
                objShoppingCartModel.ImageContent = objItem.ImageContent;
                objShoppingCartModel.ItemName = objItem.Name;
                objShoppingCartModel.Quantity = 1;
                objShoppingCartModel.Total = objItem.Price;
                objShoppingCartModel.Account = userName;
                objShoppingCartModel.UnitPrice = objItem.Price;
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
            List<ShoppingCartModel> list = new List<ShoppingCartModel>();
            foreach (var cart in this.shoppingRepository.CheckWhichAccCartIs(userName))
            {
                ShoppingCartModel objCart = new ShoppingCartModel();
                objCart.ItemId = cart.ItemId;
                objCart.UnitPrice = cart.UnitPrice;
                objCart.Total = cart.Total;

                var findElementById = this.shoppingRepository.FindElementById(cart);
                objCart.Image = findElementById.ImageContent;
                objCart.ItemBrand = findElementById.Brand;
                objCart.ItemName = findElementById.Name;
                objCart.Quantity = cart.Quantity;
                objCart.CartAcc = userName;

                list.Add(objCart);
            }

            return this.View(list);
        }

        [HttpPost]
        public IActionResult AddOrder(string email)
        {
            string userName = this.HttpContext.User.Identity.Name;
            int orderId = 0;
            Order orderObj = new Order()
            {
                Date = DateTime.Now,
                Number = string.Format("{0:ddmmyyyyyHHmmsss}", DateTime.Now),
            };
            this.shoppingRepository.AddOrderTime(orderObj);
            this.shoppingRepository.Save();
            orderId = orderObj.Id;

            foreach (var item in this.shoppingRepository.CheckWhichAccCartIs(userName))
            {
                OrderDetail objOrderDetail = new OrderDetail();
                objOrderDetail.Total = item.Total;
                objOrderDetail.ItemId = item.ItemId;
                objOrderDetail.OrderId = orderId;
                objOrderDetail.Quantity = item.Quantity;
                objOrderDetail.UnitPrice = item.UnitPrice;
                objOrderDetail.Account = userName;
                this.shoppingRepository.AddOrderDetails(objOrderDetail);
            }

            BodyBuilder bodyBuilder = new BodyBuilder();
            decimal totalPrice = 0;
            foreach (var item in this.shoppingRepository.CheckWhichAccCartIs(userName))
            {
                bodyBuilder.TextBody += "Item's id - " + item.ItemId.ToString()
                                     + "| Unit Price - " + item.UnitPrice.ToString() + "лв"
                                     + "| Quantity - " + item.Quantity.ToString()
                                     + "| Total - " + item.Total.ToString() + "лв" + Environment.NewLine;
                totalPrice += item.Total;
            }

            bodyBuilder.TextBody += "Final price - " + totalPrice.ToString();

            var mail = new MimeMessage();
            mail.From.Add(MailboxAddress.Parse("ShopCore@papercut.com"));
            mail.To.Add(MailboxAddress.Parse(email));
            mail.Subject = "Order " + orderId + " Receipt";
            mail.Body = bodyBuilder.ToMessageBody();

            using (var emailClient = new SmtpClient())
            {
                emailClient.Connect("127.0.0.1", 25);
                emailClient.Send(mail);
            }

            foreach (var item in this.shoppingRepository.CheckWhichAccCartIs(userName))
            {
                this.shoppingRepository.RemoveItem(item);
            }

            return this.RedirectToAction("Index");
        }

        public IActionResult ShoppingHistory()
        {
            string userName = this.HttpContext.User.Identity.Name;
            List<ShoppingHistoryModel> list = new List<ShoppingHistoryModel>();
            foreach (var order in this.shoppingRepository.FindAccOrders(userName))
            {
                ShoppingHistoryModel objShoppingHistoryModel = new ShoppingHistoryModel();
                objShoppingHistoryModel.OrderDetailId = order.Id;
                objShoppingHistoryModel.OrderNumber = order.OrderId;
                objShoppingHistoryModel.ItemId = order.ItemId;
                objShoppingHistoryModel.UnitPrice = order.UnitPrice;
                objShoppingHistoryModel.Total = order.Total;

                var findDate = this.shoppingRepository.FindDateById(order);
                objShoppingHistoryModel.OrderDate = findDate.Date;

                var findElementById = this.shoppingRepository.FindItemByIdForOrders(order);
                objShoppingHistoryModel.Image = findElementById.ImageContent;
                objShoppingHistoryModel.ItemBrand = findElementById.Brand;
                objShoppingHistoryModel.ItemName = findElementById.Name;
                objShoppingHistoryModel.Quantity = order.Quantity;
                objShoppingHistoryModel.User = userName;

                list.Add(objShoppingHistoryModel);
            }

            return this.View(list);
        }
    }
}
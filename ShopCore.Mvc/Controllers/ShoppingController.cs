namespace ShopCore.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Net.Mail;
    using MailKit.Net.Smtp;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.Configuration;
    using RazorEngine;
    using RazorEngine.Templating;
    using ShopCore.Data;
    using ShopCore.Data.Context;
    using ShopCore.Data.Models;
    using ShopCore.Services.Interfaces;
    using ShopCore.Services.ViewModel;
    using SmtpClient = System.Net.Mail.SmtpClient;

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
                                                                           ImageContent = objItem.ImageContent,
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
                objCart.ImageContent = findElementById.ImageContent;
                objCart.ItemBrand = findElementById.Brand;
                objCart.ItemName = findElementById.Name;
                objCart.Quantity = cart.Quantity;
                objCart.Account = userName;

                list.Add(objCart);
            }

            return this.View(list);
        }

        [HttpPost]
        public IActionResult AddOrder(string email)
        {
            string userName = this.HttpContext.User.Identity.Name;
            int orderId = 0;
            Order orderObject = new Order()
            {
                Date = DateTime.Now,
                Number = string.Format("{0:ddmmyyyyyHHmmsss}", DateTime.Now),
            };
            this.shoppingRepository.AddOrderTime(orderObject);
            this.shoppingRepository.Save();
            orderId = orderObject.Id;

            List<ShoppingCartModel> list = new List<ShoppingCartModel>();

            foreach (var item in this.shoppingRepository.CheckWhichAccCartIs(userName))
            {
                OrderDetail objectOrderDetails = new OrderDetail();
                objectOrderDetails.Total = item.Total;
                objectOrderDetails.ItemId = item.ItemId;
                objectOrderDetails.OrderId = orderId;
                objectOrderDetails.Quantity = item.Quantity;
                objectOrderDetails.UnitPrice = item.UnitPrice;
                objectOrderDetails.Account = userName;

                ShoppingCartModel objectCart = new ShoppingCartModel();
                objectCart.ItemId = item.ItemId;
                objectCart.UnitPrice = item.UnitPrice;
                objectCart.Total = item.Total;

                var currentElement = this.shoppingRepository.FindElementById(item);
                objectCart.ImageContent = currentElement.ImageContent;
                objectCart.ItemBrand = currentElement.Brand;
                objectCart.ItemName = currentElement.Name;
                objectCart.Quantity = item.Quantity;
                objectCart.Account = userName;

                list.Add(objectCart);
                this.shoppingRepository.AddOrderDetails(objectOrderDetails);
            }

            string template = System.IO.File.ReadAllText("Views/EmailTemplates/Receipt.cshtml");
            var result = Engine.Razor.RunCompile(template, DateTime.Now.TimeOfDay.ToString(), null, list);

            MailMessage mail = new MailMessage { Subject = "Order " + orderId + " Receipt", IsBodyHtml = true };
            mail.Body = result;
            mail.From = new MailAddress("ShopCore@papercut.com", "Admin");
            mail.To.Add("ShopCore@papercut.com");

            var smtpClient = new SmtpClient("127.0.0.1", 25);
            smtpClient.Send(mail);

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
                objShoppingHistoryModel.ImageContent = findElementById.ImageContent;
                objShoppingHistoryModel.ItemBrand = findElementById.Brand;
                objShoppingHistoryModel.ItemName = findElementById.Name;
                objShoppingHistoryModel.Quantity = order.Quantity;
                objShoppingHistoryModel.Account = userName;

                list.Add(objShoppingHistoryModel);
            }

            return this.View(list);
        }
    }
}
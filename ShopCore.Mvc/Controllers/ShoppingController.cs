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
    using Microsoft.Extensions.Options;
    using RazorEngine;
    using RazorEngine.Templating;
    using ShopCore.Data;
    using ShopCore.Data.Context;
    using ShopCore.Data.Models;
    using ShopCore.Services.Interfaces;
    using ShopCore.Services.Settings;
    using ShopCore.Services.ViewModel;
    using static ShopCore.Services.Settings.MailSettings;
    using SmtpClient = System.Net.Mail.SmtpClient;

    public class ShoppingController : Controller
    {
        private readonly MailSettings mailSettings;
        private IShoppingRepository shoppingRepository;

        public ShoppingController(IShoppingRepository shoppingRepository, IOptions<MailSettings> mailSettings)
        {
            this.shoppingRepository = shoppingRepository;
            this.mailSettings = mailSettings.Value;
        }

        public IActionResult Index()
        {
            IEnumerable<ShoppingViewModel> listOfShoppingViewModels = this.shoppingRepository.GetItems();
            return this.View(listOfShoppingViewModels);
        }

        [HttpPost]
        public JsonResult Index(string itemId)
        {
            string userName = this.HttpContext.User.Identity.Name;
            Cart objectShoppingCartModel = new Cart();
            Item objectItem = this.shoppingRepository.FindItemById(itemId);

            var ifAnyItemExistId = this.shoppingRepository.IfItemExistInCartById(itemId, userName);
            if (ifAnyItemExistId == null)
            {
                objectShoppingCartModel.Id = this.shoppingRepository.TableCount();
                objectShoppingCartModel.ItemId = itemId;
                objectShoppingCartModel.ImageContent = objectItem.ImageContent;
                objectShoppingCartModel.ItemName = objectItem.Name;
                objectShoppingCartModel.Quantity = 1;
                objectShoppingCartModel.Total = objectItem.Price;
                objectShoppingCartModel.Account = userName;
                objectShoppingCartModel.UnitPrice = objectItem.Price;
                this.shoppingRepository.AddToCartItem(objectShoppingCartModel);
            }
            else
            {
                var foundItem = this.shoppingRepository.FindItemQuantityById(itemId, userName);
                foundItem.Quantity++;
                foundItem.Total = foundItem.Quantity * foundItem.UnitPrice;
            }

            this.shoppingRepository.Save();
            return this.Json(new { Success = true });
        }

        public IActionResult ShoppingCart()
        {
            string userName = this.HttpContext.User.Identity.Name;
            List<ShoppingCartModel> list = new List<ShoppingCartModel>();
            foreach (var cart in this.shoppingRepository.GetWhichAccoutCartIs(userName))
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
            Order objectOrder = new Order()
            {
                Date = DateTime.Now,
                Number = string.Format("{0:ddmmyyyyyHHmmsss}", DateTime.Now),
            };
            this.shoppingRepository.AddOrderTime(objectOrder);
            this.shoppingRepository.Save();
            int orderId = objectOrder.Id;

            List<ShoppingCartModel> receiptForMail = new List<ShoppingCartModel>();

            foreach (var item in this.shoppingRepository.GetWhichAccoutCartIs(userName))
            {
                OrderDetail objectOrderDetails = new OrderDetail();
                objectOrderDetails.Total = item.Total;
                objectOrderDetails.ItemId = item.ItemId;
                objectOrderDetails.OrderId = orderId;
                objectOrderDetails.Quantity = item.Quantity;
                objectOrderDetails.UnitPrice = item.UnitPrice;
                objectOrderDetails.Account = userName;

                ShoppingCartModel objectCartForMail = new ShoppingCartModel();
                objectCartForMail.ItemId = item.ItemId;
                objectCartForMail.UnitPrice = item.UnitPrice;
                objectCartForMail.Total = item.Total;

                var currentElement = this.shoppingRepository.FindElementById(item);
                objectCartForMail.ImageContent = currentElement.ImageContent;
                objectCartForMail.ItemBrand = currentElement.Brand;
                objectCartForMail.ItemName = currentElement.Name;
                objectCartForMail.Quantity = item.Quantity;
                objectCartForMail.Account = userName;

                receiptForMail.Add(objectCartForMail);
                this.shoppingRepository.AddOrderDetails(objectOrderDetails);
            }

            TemplateType template = TemplateType.Receipt;
            string templateContent = System.IO.File.ReadAllText(MailSettings.GetFilePath(template));
            var renderedTemplate = Engine.Razor.RunCompile(templateContent, DateTime.Now.TimeOfDay.ToString(), null, receiptForMail);

            MailMessage mail = new MailMessage { Subject = "Order " + orderId + " Receipt", IsBodyHtml = true };
            mail.Body = renderedTemplate;
            mail.From = new MailAddress(this.mailSettings.From);
            mail.To.Add(email);

            var smtpClient = new SmtpClient(this.mailSettings.SmtpServer, this.mailSettings.Port);
            smtpClient.Send(mail);

            foreach (var item in this.shoppingRepository.GetWhichAccoutCartIs(userName))
            {
                this.shoppingRepository.RemoveItem(item);
            }

            this.shoppingRepository.Save();
            return this.RedirectToAction("Index");
        }

        public IActionResult ShoppingHistory()
        {
            string userName = this.HttpContext.User.Identity.Name;
            List<ShoppingHistoryModel> listOfShoppingHistory = new List<ShoppingHistoryModel>();
            foreach (var order in this.shoppingRepository.FindAccOrders(userName))
            {
                ShoppingHistoryModel objectShoppingHistoryModel = new ShoppingHistoryModel();
                objectShoppingHistoryModel.OrderDetailId = order.Id;
                objectShoppingHistoryModel.OrderNumber = order.OrderId;
                objectShoppingHistoryModel.ItemId = order.ItemId;
                objectShoppingHistoryModel.UnitPrice = order.UnitPrice;
                objectShoppingHistoryModel.Total = order.Total;

                var foundDate = this.shoppingRepository.FindDateById(order);
                objectShoppingHistoryModel.OrderDate = foundDate.Date;

                var findElementById = this.shoppingRepository.FindItemByIdForOrders(order);
                objectShoppingHistoryModel.ImageContent = findElementById.ImageContent;
                objectShoppingHistoryModel.ItemBrand = findElementById.Brand;
                objectShoppingHistoryModel.ItemName = findElementById.Name;
                objectShoppingHistoryModel.Quantity = order.Quantity;
                objectShoppingHistoryModel.Account = userName;

                listOfShoppingHistory.Add(objectShoppingHistoryModel);
            }

            return this.View(listOfShoppingHistory);
        }
    }
}
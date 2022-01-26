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
    using ShopCore.Services.Interfaces;
    using ShopCore.Services.Settings;
    using ShopCore.Services.ViewModel;
    using static ShopCore.Services.Settings.MailSettings;
    using SmtpClient = System.Net.Mail.SmtpClient;

    public class ShoppingController : Controller
    {
        private readonly MailSettings mailSettings;
        private IShoppingRepository shoppingRepository;
        private IUnitOfWork unitOfWork;

        public ShoppingController(IShoppingRepository shoppingRepository, IOptions<MailSettings> mailSettings, IUnitOfWork unitOfWork)
        {
            this.shoppingRepository = shoppingRepository;
            this.mailSettings = mailSettings.Value;
            this.unitOfWork = unitOfWork;
        }

        public IActionResult Index()
        {
            return this.View(this.shoppingRepository.GetItems());
        }

        [HttpPost]
        public JsonResult Index(Guid itemId)
        {
            string userName = this.HttpContext.User.Identity.Name;
            this.shoppingRepository.AddItemToCart(itemId, userName);
            this.unitOfWork.SaveChanges();

            return this.Json(new { Success = true });
        }

        public IActionResult ShoppingCart()
        {
            string userName = this.HttpContext.User.Identity.Name;

            return this.View(this.shoppingRepository.DisplayShoppingCart(userName));
        }

        [HttpPost]
        public IActionResult AddOrder(string email)
        {
            string userName = this.HttpContext.User.Identity.Name;
            int orderId = this.shoppingRepository.AddOrderTime();
            this.unitOfWork.SaveChanges();

            List<ShoppingCartViewModel> receiptForMail = new List<ShoppingCartViewModel>();

            this.shoppingRepository.AddOrder(userName, orderId, receiptForMail);

            TemplateType template = TemplateType.Receipt;
            string templateContent = System.IO.File.ReadAllText(MailSettings.GetFilePath(template));
            var renderedTemplate = Engine.Razor.RunCompile(templateContent, DateTime.Now.TimeOfDay.ToString(), null, receiptForMail);

            MailMessage mail = new MailMessage { Subject = "Order " + orderId + " Receipt", IsBodyHtml = true };
            mail.Body = renderedTemplate;
            mail.From = new MailAddress(this.mailSettings.From);
            mail.To.Add(email);

            var smtpClient = new SmtpClient(this.mailSettings.SmtpServer, this.mailSettings.Port);
            smtpClient.Send(mail);

            this.shoppingRepository.ClearCart(userName);

            this.unitOfWork.SaveChanges();
            return this.RedirectToAction("Index");
        }

        public IActionResult ShoppingHistory()
        {
            string userName = this.HttpContext.User.Identity.Name;

            return this.View(this.shoppingRepository.GetShoppingHistory(userName));
        }
    }
}
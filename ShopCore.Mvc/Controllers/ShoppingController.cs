namespace ShopCore.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Net.Mail;
    using System.Security.Claims;
    using MailKit.Net.Smtp;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.Logging;
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
        private readonly ILogger<ShoppingController> logger;
        private readonly MailSettings mailSettings;
        private IShoppingRepository shoppingRepository;
        private IUnitOfWork unitOfWork;

        public ShoppingController(IShoppingRepository shoppingRepository, IOptions<MailSettings> mailSettings, IUnitOfWork unitOfWork, ILogger<ShoppingController> logger)
        {
            this.logger = logger;
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
            string email = this.HttpContext.User.FindFirstValue(ClaimTypes.Email);
            string typeLogin = this.HttpContext.User.FindFirst("TypeLogin").Value;
            this.shoppingRepository.AddItemToCart(itemId, email, typeLogin);
            this.unitOfWork.SaveChanges();
            return this.Json(new { Success = true });
        }

        public IActionResult ShoppingCart()
        {
            if (this.User.Identity.IsAuthenticated)
            {
                string email = this.HttpContext.User.FindFirstValue(ClaimTypes.Email);
                string typeLogin = this.HttpContext.User.FindFirst("TypeLogin").Value;
                return this.View(this.shoppingRepository.DisplayShoppingCart(email, typeLogin));
            }
            else
            {
                return this.View();
            }
        }

        [HttpPost]
        public IActionResult AddOrder()
        {
            string email = this.HttpContext.User.FindFirstValue(ClaimTypes.Email);
            string typeLogin = this.HttpContext.User.FindFirst("TypeLogin").Value;

            int orderId = this.shoppingRepository.AddOrderTime();
            this.unitOfWork.SaveChanges();

            List<ShoppingCartViewModel> receiptForMail = new List<ShoppingCartViewModel>();

            this.shoppingRepository.AddOrder(orderId, receiptForMail, email, typeLogin);

            TemplateType template = TemplateType.Receipt;
            string templateContent = System.IO.File.ReadAllText(MailSettings.GetFilePath(template));
            var renderedTemplate = Engine.Razor.RunCompile(templateContent, DateTime.Now.TimeOfDay.ToString(), null, receiptForMail);

            MailMessage mail = new MailMessage { Subject = "Order " + orderId + " Receipt", IsBodyHtml = true };
            mail.Body = renderedTemplate;
            mail.From = new MailAddress(this.mailSettings.From);
            mail.To.Add(email);

            var smtpClient = new SmtpClient(this.mailSettings.SmtpServer, this.mailSettings.Port);
            smtpClient.Send(mail);

            this.shoppingRepository.ClearCart(email, typeLogin);

            this.unitOfWork.SaveChanges();
            return this.RedirectToAction("Index");
        }

        public IActionResult ShoppingHistory()
        {
            string email = this.HttpContext.User.FindFirstValue(ClaimTypes.Email);
            string typeLogin = this.HttpContext.User.FindFirst("TypeLogin").Value;

            return this.View(this.shoppingRepository.GetShoppingHistory(email, typeLogin));
        }
    }
}
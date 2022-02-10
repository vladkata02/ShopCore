namespace ShopCore.StatisticsMail
{
    using System;
    using System.Collections.Generic;
    using System.Net.Mail;
    using Hangfire;
    using Microsoft.Extensions.Options;
    using RazorEngine;
    using RazorEngine.Templating;
    using ShopCore.Services.Interfaces;
    using ShopCore.Services.Settings;
    using ShopCore.Services.ViewModel;
    using static ShopCore.Services.Settings.MailSettings;

    public class MailSender
    {
        private readonly MailSettings mailSettings;
        private IMailSenderRepository everyDayMailSenderRepository;

        public MailSender(IOptions<MailSettings> mailSettings, IMailSenderRepository everyDayMailSenderRepository)
        {
            this.mailSettings = mailSettings.Value;
            this.everyDayMailSenderRepository = everyDayMailSenderRepository;
        }

        public void SendStatisticsMail()
        {
            List<ShoppingHistoryViewModel> itemsSoldForTheDay = new List<ShoppingHistoryViewModel>();

            this.everyDayMailSenderRepository.GetTodaysOrders(itemsSoldForTheDay);

            TemplateType template = TemplateType.DailyActivity;
            string templateContent = System.IO.File.ReadAllText(MailSettings.GetFilePath(template));
            var renderedTemplate = Engine.Razor.RunCompile(templateContent, DateTime.Now.TimeOfDay.ToString(), null, itemsSoldForTheDay);

            MailMessage mail = new MailMessage { Subject = $"Orders for {DateTime.Today.ToString("dd/MM/yyyy")}", IsBodyHtml = true };
            mail.Body = renderedTemplate;
            mail.From = new MailAddress(this.mailSettings.From);
            mail.To.Add(new MailAddress(this.mailSettings.To));

            var smtpClient = new SmtpClient(this.mailSettings.SmtpServer, this.mailSettings.Port);
            smtpClient.Send(mail);
        }
    }
}

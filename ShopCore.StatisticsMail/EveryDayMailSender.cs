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

    public class EveryDayMailSender
    {
        private readonly MailSettings mailSettings;
        private IEveryDayMailSenderRepository everyDayMailSenderRepository;

        public EveryDayMailSender(IOptions<MailSettings> mailSettings, IEveryDayMailSenderRepository everyDayMailSenderRepository)
        {
            this.mailSettings = mailSettings.Value;
            this.everyDayMailSenderRepository = everyDayMailSenderRepository;
        }

        public void SendStatisticsMail()
        {
            List<ShoppingHistoryViewModel> itemsSoldForTheDay = this.everyDayMailSenderRepository.GetTodaysOrders();
            TemplateType template = TemplateType.DailyActivity;
            string templateContent = System.IO.File.ReadAllText(MailSettings.GetFilePath(template));
            var renderedTemplate = Engine.Razor.RunCompile(templateContent, DateTime.Now.TimeOfDay.ToString(), null, itemsSoldForTheDay);

            MailMessage mail = new MailMessage { Subject = "Statistics for the day", IsBodyHtml = true };
            mail.Body = renderedTemplate;
            mail.From = new MailAddress(this.mailSettings.From);
            mail.To.Add(new MailAddress(this.mailSettings.To));

            var smtpClient = new SmtpClient(this.mailSettings.SmtpServer, this.mailSettings.Port);
            smtpClient.Send(mail);
        }

        public void SendMailEveryDay()
        {
            var server = new BackgroundJobServer();
            RecurringJob.AddOrUpdate<EveryDayMailSender>("test", x => x.SendStatisticsMail(), Cron.Minutely);
            RecurringJob.Trigger("test");
        }
    }
}

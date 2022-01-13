namespace ShopCore.Services.Settings
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using ShopCore.Services.Interfaces;

    public class MailSettings
    {
        public string From { get; set; }

        public string SmtpServer { get; set; }

        public int Port { get; set; }

        public string TemplatePath { get; set; }
    }
}

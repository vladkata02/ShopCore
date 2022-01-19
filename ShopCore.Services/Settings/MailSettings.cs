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
        public enum TemplateType
        {
            Receipt,
        }

        public static string TemplateFolder { get; set; }

        public string From { get; set; }

        public string SmtpServer { get; set; }

        public int Port { get; set; }

        public static string GetFilePath(TemplateType template)
        {
            switch (template)
            {
                case TemplateType.Receipt:
                    var fileName = template;
                    var templateWholePathe = string.Concat(MailSettings.TemplateFolder, fileName, ".cshtml");
                    return templateWholePathe;
                default:
                    return null;
            }
        }
    }
}

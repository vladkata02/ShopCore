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
            DailyActivity,
        }

        public static string TemplateFolder { get; set; }

        public string From { get; set; }

        public string To { get; set; }

        public string SmtpServer { get; set; }

        public int Port { get; set; }

        public static string GetFilePath(TemplateType template)
        {
            string fileName;
            switch (template)
            {
                case TemplateType.Receipt:
                    fileName = template.ToString();
                    break;
                case TemplateType.DailyActivity:
                    fileName = template.ToString();
                    break;
                default:
                    return null;
            }

            var templateWholePath = string.Concat(MailSettings.TemplateFolder, fileName, ".cshtml");
            return templateWholePath;
        }
    }
}

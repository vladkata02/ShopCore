using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using ShopCore.Services.Settings;
using System;
using System.IO;
using static ShopCore.Services.Settings.MailSettings;

namespace ShopCore.Utilities
{
        
    public class File
        {

        private readonly MailSettings mailSettings;
        public File(IOptions<MailSettings> mailSettings)
        {
            this.mailSettings = mailSettings.Value;
        }
        public static string GetFileFullName(IFormFile files)
        {
            var fileName = Path.GetFileName(files.FileName);
            var fileExtension = Path.GetExtension(fileName);
            var newFileName = string.Concat(Convert.ToString(Guid.NewGuid()), fileExtension);

            return newFileName;
        }
    }
}

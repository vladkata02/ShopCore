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
        public static string GetFileFullName(IFormFile files)
        {
            string fileName = Path.GetFileName(files.FileName);
            string fileExtension = Path.GetExtension(fileName);
            string newFileName = string.Concat(Convert.ToString(Guid.NewGuid()), fileExtension);

            return newFileName;
        }
    }
}

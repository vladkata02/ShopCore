using Microsoft.AspNetCore.Http;
using System;
using System.IO;

namespace ShopCore.Utilities
{
        public static class FileName
        {

        public static string GetFileName(IFormFile files)
        {
            var fileName = Path.GetFileName(files.FileName);
            var fileExtension = Path.GetExtension(fileName);
            var newFileName = string.Concat(Convert.ToString(Guid.NewGuid()), fileExtension);

            return newFileName;
        }
    }
}

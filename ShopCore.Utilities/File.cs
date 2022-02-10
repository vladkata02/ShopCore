namespace ShopCore.Utilities
{
    using System;
    using System.IO;
    using Microsoft.AspNetCore.Http;

    public class File
    {
        public static string GetFileFullName(IFormFile files)
        {
            string fileName = Path.GetFileName(files.FileName);
            string fileExtension = Path.GetExtension(fileName);
            string newFileName = string.Concat(Convert.ToString(Guid.NewGuid()), fileExtension);

            return newFileName;
        }

        public static byte[] GetImageContent(IFormFile files)
        {
            byte[] imageContent = null;
            using (var target = new MemoryStream())
            {
                files.CopyTo(target);
                imageContent = target.ToArray();
            }

            return imageContent;
        }
    }
}

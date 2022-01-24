namespace ShopCore.Data.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.IO;
    using Microsoft.AspNetCore.Http;

    public class Item
    {
        public Guid Id { get; set; }

        public int CategoryId { get; set; }

        public string Code { get; set; }

        public string Name { get; set; }

        public string Brand { get; set; }

        public string Description { get; set; }

        public string ImageName { get; set; }

        public byte[] ImageContent { get; set; }

        public decimal Price { get; set; }

        public Item()
        {
        }

        public Item(int categoryId, string description, string code, string name, string brand, decimal price, string newFileName, byte[] imageContent)
        {
            this.CategoryId = categoryId;
            this.Description = description;
            this.Code = code;
            this.Id = Guid.NewGuid();
            this.Name = name;
            this.Brand = brand;
            this.Price = price;
            this.ImageName = newFileName;
            this.ImageContent = imageContent;
        }
    }
}

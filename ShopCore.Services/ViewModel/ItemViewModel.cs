namespace ShopCore.Services.ViewModel
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Web;
    using ShopCore.Data.Models;

    public class ItemViewModel
    {
        public Guid Id { get; set; }

        public int CategoryId { get; set; }

        public string Code { get; set; }

        public string Name { get; set; }

        public string Brand { get; set; }

        public string Description { get; set; }

        public decimal Price { get; set; }

        public byte[] ImageContent { get; set; }

        public string ImageName { get; set; }
    }
}
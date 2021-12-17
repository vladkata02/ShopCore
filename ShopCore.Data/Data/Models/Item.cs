namespace ShopCore.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    public class Item
    {
        public Guid ItemId { get; set; }

        public int CategoryId { get; set; }

        public string ItemCode { get; set; }

        public string ItemName { get; set; }

        public string ItemBrand { get; set; }

        public string Description { get; set; }

        public string ImageName { get; set; }

        public byte[] Image { get; set; }

        public decimal ItemPrice { get; set; }
    }
}

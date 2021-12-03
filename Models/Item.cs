using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ShopCore.Models
{     
    public class Item
    {
        public System.Guid ItemId { get; set; }
        public int CategoryId { get; set; }
        public string ItemCode { get; set; }
        public string ItemName { get; set; }
        public string ItemBrand { get; set; }
        public string Description { get; set; }
        public string ImagePath { get; set; }
        public decimal ItemPrice { get; set; }
    }
}

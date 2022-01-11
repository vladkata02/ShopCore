namespace ShopCore.Services.ViewModel
{
    using System;
    using System.Linq;
    using System.Web;

    public class ShoppingViewModel
    {
        public Guid ItemId { get; set; }

        public string ItemName { get; set; }

        public decimal ItemPrice { get; set; }

        public byte[] ImageContent { get; set; }

        public string Description { get; set; }

        public string ItemBrand { get; set; }

        public string ItemCode { get; set; }

        public string Category { get; set; }
    }
}
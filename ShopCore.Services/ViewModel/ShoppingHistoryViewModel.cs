namespace ShopCore.Services.ViewModel
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Web;

    public class ShoppingHistoryViewModel
    {
        public int OrderDetailId { get; set; }

        public string ItemId { get; set; }

        public decimal Quantity { get; set; }

        public decimal UnitPrice { get; set; }

        public decimal Total { get; set; }

        public byte[] ImageContent { get; set; }

        public string ItemBrand { get; set; }

        public string ItemName { get; set; }

        public DateTime OrderDate { get; set; }

        public int OrderNumber { get; set; }

        public string Account { get; set; }
    }
}
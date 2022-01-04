using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ShopCore.Services.ViewModel
{
    public class ShoppingHistoryModel
    {
        public int OrderDetailId { get; set; }

        public string ItemId { get; set; }

        public decimal Quantity { get; set; }

        public decimal UnitPrice { get; set; }

        public decimal Total { get; set; }

        public byte[] Image { get; set; }

        public string ItemBrand { get; set; }

        public string ItemName { get; set; }

        public DateTime OrderDate { get; set; }

        public int OrderNumber { get; set; }

        public string User { get; set; }
    }
}
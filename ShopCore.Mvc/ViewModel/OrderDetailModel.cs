using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ShopCore.ViewModel
{
    public class OrderDetailModel
    {
        public int OrderDetailId { get; set; }

        public int OrderId { get; set; }

        public string ItemId { get; set; }

        public decimal Quantity { get; set; }

        public decimal UnitPrice { get; set; }

        public decimal Total { get; set; }

        public string OrderAccMail { get; set; }
    }
}
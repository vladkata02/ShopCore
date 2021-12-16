using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ShopCore.Models
{
    public class OrderDetail
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

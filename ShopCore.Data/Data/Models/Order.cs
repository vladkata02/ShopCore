using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ShopCore.Models
{
    
    public class Order
    {
        public int OrderId { get; set; }
        public System.DateTime OrderDate { get; set; }
        public string OrderNumber { get; set; }
    }
}

﻿namespace ShopCore.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Linq;
    using System.Threading.Tasks;
    using Microsoft.EntityFrameworkCore;

    [Table("Carts")]
    public class Cart
    {
        public int CartId { get; set; }

        public string ItemId { get; set; }

        public decimal Quantity { get; set; }

        public decimal UnitPrice { get; set; }

        public decimal Total { get; set; }

        public string CartAcc { get; set; }

        public string ItemName { get; set; }

        public byte[] Image { get; set; }
    }
}

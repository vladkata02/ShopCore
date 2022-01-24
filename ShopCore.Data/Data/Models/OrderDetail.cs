namespace ShopCore.Data.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    public class OrderDetail
    {
        public int Id { get; set; }

        public int OrderId { get; set; }

        public Guid ItemId { get; set; }

        public decimal Quantity { get; set; }

        public decimal UnitPrice { get; set; }

        public decimal Total { get; set; }

        public string Account { get; set; }

        public OrderDetail()
        {
        }

        public OrderDetail(decimal total, Guid itemId, int orderId, decimal quantity, decimal unitPrice, string userName)
        {
            this.Total = total;
            this.ItemId = itemId;
            this.OrderId = orderId;
            this.Quantity = quantity;
            this.UnitPrice = unitPrice;
            this.Account = userName;
        }
    }
}

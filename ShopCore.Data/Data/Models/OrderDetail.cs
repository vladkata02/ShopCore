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

        public string Email { get; set; }

        public string TypeLogin { get; set; }

        public OrderDetail()
        {
        }

        public OrderDetail(decimal total, Guid itemId, int orderId, decimal quantity, decimal unitPrice, string email, string typeLogin)
        {
            this.Total = total;
            this.ItemId = itemId;
            this.OrderId = orderId;
            this.Quantity = quantity;
            this.UnitPrice = unitPrice;
            this.Email = email;
            this.TypeLogin = typeLogin;
        }
    }
}

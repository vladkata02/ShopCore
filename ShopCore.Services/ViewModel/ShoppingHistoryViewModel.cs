namespace ShopCore.Services.ViewModel
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Web;

    public class ShoppingHistoryViewModel
    {
        public int OrderDetailId { get; set; }

        public Guid ItemId { get; set; }

        public decimal Quantity { get; set; }

        public decimal UnitPrice { get; set; }

        public decimal Total { get; set; }

        public byte[] ImageContent { get; set; }

        public string ItemBrand { get; set; }

        public string ItemName { get; set; }

        public DateTime OrderDate { get; set; }

        public int OrderNumber { get; set; }

        public string Account { get; set; }

        public ShoppingHistoryViewModel(int id, int orderId, Guid itemId, decimal unitPrice, decimal total, DateTime date, byte[] imageContent, string itemBrand, string itemName, decimal quantity, string userName)
        {
            this.OrderDetailId = id;
            this.OrderNumber = orderId;
            this.ItemId = itemId;
            this.UnitPrice = unitPrice;
            this.Total = total;
            this.OrderDate = date;
            this.ImageContent = imageContent;
            this.ItemBrand = itemBrand;
            this.ItemName = itemName;
            this.Quantity = quantity;
            this.Account = userName;
        }
    }
}
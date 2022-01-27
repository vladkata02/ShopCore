namespace ShopCore.Services.ViewModel
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Web;

    public class ShoppingCartViewModel
    {
        public Guid ItemId { get; set; }

        public decimal Quantity { get; set; }

        public decimal UnitPrice { get; set; }

        public decimal Total { get; set; }

        public byte[] ImageContent { get; set; }

        public string ItemName { get; set; }

        public string ItemBrand { get; set; }

        public string Account { get; set; }

        public ShoppingCartViewModel(Guid itemId, decimal unitPrice, decimal total, byte[] imageContent, string itemBrand, string itemName, decimal quantity, string userName)
        {
            this.ItemId = itemId;
            this.UnitPrice = unitPrice;
            this.Total = total;
            this.ImageContent = imageContent;
            this.ItemBrand = itemBrand;
            this.ItemName = itemName;
            this.Quantity = quantity;
            this.Account = userName;
        }
    }
}
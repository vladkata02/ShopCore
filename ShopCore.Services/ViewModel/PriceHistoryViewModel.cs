namespace ShopCore.Services.ViewModel
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    public class PriceHistoryViewModel
    {
        public Guid ItemId { get; set; }

        public decimal CurrentPrice { get; set; }

        public byte[] ImageContent { get; set; }

        public string ItemBrand { get; set; }

        public string ItemName { get; set; }

        public System.DateTime Date { get; set; }

        public PriceHistoryViewModel(decimal priceValue, DateTime date, byte[] imageContent, string brand, string name, Guid itemGuid)
        {
            this.CurrentPrice = priceValue;
            this.Date = date;
            this.ImageContent = imageContent;
            this.ItemBrand = brand;
            this.ItemName = name;
            this.ItemId = itemGuid;
        }
    }
}

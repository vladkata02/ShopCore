namespace ShopCore.Services.ViewModel
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    public class PriceHistoryViewModel
    {
        public string ItemId { get; set; }

        public decimal CurrentPrice { get; set; }

        public byte[] ImageContent { get; set; }

        public string ItemBrand { get; set; }

        public string ItemName { get; set; }

        public System.DateTime Date { get; set; }
    }
}

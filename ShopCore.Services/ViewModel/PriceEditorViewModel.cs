namespace ShopCore.Services.ViewModel
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    public class PriceEditorViewModel
    {
        public decimal CurrentPrice { get; set; }

        public Guid ItemId { get; set; }

        public string ItemName { get; set; }

        public decimal ItemPrice { get; set; }

        public byte[] ImageContent { get; set; }

        public string ItemBrand { get; set; }
    }
}

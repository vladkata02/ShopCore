using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ShopCore.Services.ViewModel
{
    public class PriceHistoryViewModel
    {
        public string ItemId { get; set; }

        public decimal CurrentPrice { get; set; }

        public byte[] Image { get; set; }

        public string ItemBrand { get; set; }

        public string ItemName { get; set; }

        public System.DateTime DateOfPrice { get; set; }
    }
}

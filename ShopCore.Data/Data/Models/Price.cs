namespace ShopCore.Models
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    public class Price
    {
        public int PriceId { get; set; }

        public string ItemId { get; set; }

        public decimal PriceOfItem { get; set; }

        public DateTime DateOfPrice { get; set; }
    }
}

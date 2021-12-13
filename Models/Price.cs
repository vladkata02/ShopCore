using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ShopCore.Models
{
    public class Price
    {
        public int PriceId { get; set; }
        public string ItemId { get; set; }
        public decimal PriceOfItem { get; set; }
        public DateTime DateOfPrice { get; set; }
    }
}

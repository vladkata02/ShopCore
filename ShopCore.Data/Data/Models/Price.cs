namespace ShopCore.Data.Models
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    public class Price
    {
        public int Id { get; set; }

        public Guid ItemId { get; set; }

        public decimal PriceValue { get; set; }

        public DateTime Date { get; set; }
    }
}

namespace ShopCore.Data.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    public class Order
    {
        public int Id { get; set; }

        public DateTime Date { get; set; }

        public string Number { get; set; }

        public Order()
        {
            this.Date = DateTime.Now;
            this.Number = string.Format("{0:ddmmyyyyyHHmmsss}", DateTime.Now);
        }
    }
}

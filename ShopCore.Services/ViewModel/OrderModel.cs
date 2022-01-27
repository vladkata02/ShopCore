namespace ShopCore.Services.ViewModel
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Web;

    public class OrderModel
    {
        public int Id { get; set; }

        public DateTime Date { get; set; }

        public string Number { get; set; }
    }
}
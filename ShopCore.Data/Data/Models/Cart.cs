namespace ShopCore.Data.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Linq;
    using System.Threading.Tasks;
    using Microsoft.EntityFrameworkCore;

    [Table("Carts")]
    public class Cart
    {
        public int Id { get; set; }

        public Guid ItemId { get; set; }

        public decimal Quantity { get; set; }

        public decimal UnitPrice { get; set; }

        public decimal Total { get; set; }

        public string Email { get; set; }

        public string TypeLogin { get; set; }

        public string ItemName { get; set; }

        public byte[] ImageContent { get; set; }

        public Cart()
        {
        }

        public Cart(int id, Guid itemId, string name, decimal price, byte[] imageContent, string typeLogin, string email)
        {
            this.Id = id;
            this.ItemId = itemId;
            this.Quantity = 1;
            this.UnitPrice = price;
            this.Total = price;
            this.Email = email;
            this.TypeLogin = typeLogin;
            this.ItemName = name;
            this.ImageContent = imageContent;
        }
    }
}

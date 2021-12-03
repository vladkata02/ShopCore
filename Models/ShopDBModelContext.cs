using Microsoft.EntityFrameworkCore;
using ShopCore.Models;

namespace ShopCore.Data
    {
        public class ShopDBEntities3 : DbContext
        {
        public ShopDBEntities3(DbContextOptions<ShopDBEntities3> options)
                : base(options)
            {
            }

        public virtual DbSet<Item> Items { get; set; }
        public virtual DbSet<Category> Categories { get; set; }
        public virtual DbSet<Order> Orders { get; set; }
        public virtual DbSet<OrderDetail> OrderDetails { get; set; }
        public virtual DbSet<user> users { get; set; }
    }
}
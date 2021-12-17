namespace ShopCore.Data.Context
    {
    using Microsoft.EntityFrameworkCore;
    using ShopCore.Models;

    public class ShopDBContext : DbContext
        {
        public ShopDBContext(DbContextOptions<ShopDBContext> options)
                : base(options)
            {
            }

        public DbSet<Item> Items { get; set; }

        public DbSet<Category> Categories { get; set; }

        public DbSet<Order> Orders { get; set; }

        public DbSet<OrderDetail> OrderDetails { get; set; }

        public DbSet<User> Users { get; set; }

        public DbSet<Cart> Carts { get; set; }

        public DbSet<Price> Prices { get; set; }
    }
}
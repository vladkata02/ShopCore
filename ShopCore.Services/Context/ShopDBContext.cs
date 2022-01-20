namespace ShopCore.Services.Context
    {
    using System.IO;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Design;
    using Microsoft.Extensions.Configuration;
    using ShopCore.Data.Models;

    public class ShopDBContext : DbContext
        {
        public ShopDBContext(DbContextOptions<ShopDBContext> options)
                : base(options)
            {
            }

        public virtual DbSet<Item> Items { get; set; }

        public virtual DbSet<Category> Categories { get; set; }

        public virtual DbSet<Order> Orders { get; set; }

        public virtual DbSet<OrderDetail> OrderDetails { get; set; }

        public virtual DbSet<User> Users { get; set; }

        public virtual DbSet<Cart> Carts { get; set; }

        public virtual DbSet<Price> Prices { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Cart>()
                .Property(p => p.Quantity)
                .HasColumnType("decimal(5, 3)");
            modelBuilder.Entity<Cart>()
               .Property(p => p.Total)
               .HasColumnType("decimal(5, 2)");
            modelBuilder.Entity<Cart>()
               .Property(p => p.UnitPrice)
               .HasColumnType("decimal(5, 2)");

            modelBuilder.Entity<Item>()
               .Property(p => p.Price)
               .HasColumnType("decimal(5, 2)");

            modelBuilder.Entity<OrderDetail>()
                .Property(p => p.Quantity)
                .HasColumnType("decimal(5, 3)");
            modelBuilder.Entity<OrderDetail>()
              .Property(p => p.Total)
              .HasColumnType("decimal(5, 2)");
            modelBuilder.Entity<OrderDetail>()
               .Property(p => p.UnitPrice)
               .HasColumnType("decimal(5, 2)");

            modelBuilder.Entity<Price>()
               .Property(p => p.PriceValue)
               .HasColumnType("decimal(5, 2)");
        }
    }
}
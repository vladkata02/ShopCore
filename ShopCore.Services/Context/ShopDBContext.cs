namespace ShopCore.Services.Context
    {
    using System.IO;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Design;
    using Microsoft.Extensions.Configuration;
    using ShopCore.Data.Models;

    internal class ShopDBContext : DbContext
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

        public class DesignTimeDbContextFactory : IDesignTimeDbContextFactory<ShopDBContext>
        {
            public ShopDBContext CreateDbContext(string[] args)
            {
                IConfigurationRoot configuration = new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory()).AddJsonFile(@Directory.GetCurrentDirectory() + "/../ShopCore.Mvc/appsettings.json").Build();
                var builder = new DbContextOptionsBuilder<ShopDBContext>();
                var connectionString = configuration.GetConnectionString("DefaultConnectionString");
                builder.UseSqlServer(connectionString);
                return new ShopDBContext(builder.Options);
            }
        }
    }
}
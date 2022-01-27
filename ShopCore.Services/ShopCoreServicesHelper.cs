namespace ShopCore.Services
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using Microsoft.Extensions.DependencyInjection;
    using ShopCore.Services.Interfaces;
    using ShopCore.Services.Repositories;

    public static class ShopCoreServicesHelper
    {
    public static IServiceCollection AddShopCoreServices(this IServiceCollection services)
    {
            services.AddScoped<IItemRepository, ItemRepository>();
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<IPriceRepository, PriceRepository>();
            services.AddScoped<IShoppingRepository, ShoppingRepository>();
            services.AddScoped<IShoppingHistoryRepository, ShoppingHistoryRepository>();
            services.AddScoped<IMailSenderRepository, MailSenderRepository>();
            services.AddScoped<IUnitOfWork, UnitOfWork>();

            return services;
    }
}
}

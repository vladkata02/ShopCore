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

    // TODO Прието е имената на такива класове да бъдат ShopCoreServicesHelper или ShopCoreServicesExtensions
    public static class ShopCoreServicesCollection
    {
    public static IServiceCollection AddShopCoreServices(this IServiceCollection services)
    {
            services.AddScoped<IItemRepository, ItemRepository>();
            services.AddScoped<IAccountRepository, AccountRepository>();
            services.AddScoped<IPriceRepository, PriceRepository>();
            services.AddScoped<IShoppingRepository, ShoppingRepository>();
            services.AddScoped<IShoppingHistoryRepository, ShoppingHistoryRepository>();

            return services;
    }
}
}

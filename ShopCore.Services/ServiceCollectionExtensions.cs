namespace ShopCore.Services
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using ShopCore.Services.Context;

    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection RegisterDataServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<ShopDBContext>(optionsAction: o => o.UseSqlServer(configuration["ConnectionStrings:DefaultConnectionString"]));
            return services;
        }
     }
}

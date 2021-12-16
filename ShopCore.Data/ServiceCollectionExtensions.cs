using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using ShopCore.Data.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShopCore.Data
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection RegisterDataServices( this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<ShopDBContext>(optionsAction: o => o.UseSqlServer("Data Source=DESKTOP-MPTUQQD;Initial Catalog=ShopDB;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False;MultipleActiveResultSets=true"));
            return services;
        }

     }
}

namespace ShopCore
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Hangfire;
    using Hangfire.MemoryStorage;
    using Microsoft.AspNetCore.Authentication.Cookies;
    using Microsoft.AspNetCore.Builder;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.AspNetCore.HttpsPolicy;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Hosting;
    using ShopCore.Data;
    using ShopCore.Data.Models;
    using ShopCore.Services;
    using ShopCore.Services.Context;
    using ShopCore.Services.Interfaces;
    using ShopCore.Services.Repositories;
    using ShopCore.Services.Settings;
    using ShopCore.StatisticsMail;

    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            this.Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddHangfire(options =>
            {
                 options.UseMemoryStorage();
            });
            services.AddSession();
            services.AddMemoryCache();
            services.AddMvc();
            services.AddControllersWithViews();
            services.RegisterDataServices(this.Configuration);
            services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
                    .AddCookie();
            services.AddShopCoreServices();
            services.Configure<MailSettings>(this.Configuration.GetSection("EmailConfiguration"));
            services.AddControllers();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        [Obsolete]
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseHangfireDashboard();
            app.UseHangfireServer();
            app.UseRouting();
            app.UseAuthentication();
            app.UseAuthorization();
            RecurringJob.AddOrUpdate<MailSender>("Today's activity", x => x.SendStatisticsMail(), "0 21 * * *");

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Shopping}/{action=Index}/{id?}");
            });
        }
    }
}

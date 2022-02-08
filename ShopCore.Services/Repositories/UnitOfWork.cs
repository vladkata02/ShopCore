namespace ShopCore.Services.Repositories
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using Microsoft.Extensions.Logging;
    using ShopCore.Data.Models;
    using ShopCore.Services.Context;
    using ShopCore.Services.Interfaces;

    internal class UnitOfWork : IUnitOfWork
    {
        private readonly ILogger<UnitOfWork> logger;
        private ShopDBContext context;

        public UnitOfWork(ShopDBContext context, ILogger<UnitOfWork> logger)
        {
            this.logger = logger;
            this.context = context;
        }

        public void SaveChanges()
        {
            this.context.SaveChanges();
        }
    }
}

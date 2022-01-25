namespace ShopCore.Services.Repositories
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using ShopCore.Data.Models;
    using ShopCore.Services.Context;
    using ShopCore.Services.Interfaces;

    internal class UnitOfWork : IUnitOfWork
    {
        private ShopDBContext context;

        public UnitOfWork(ShopDBContext context)
        {
            this.context = context;
        }

        public Item FindItemByGuid(Guid itemId)
        {
            return this.context.Items
                .Where(check => check.Id == itemId)
                .FirstOrDefault();
        }

        public void SaveChanges()
        {
            this.context.SaveChanges();
        }
    }
}

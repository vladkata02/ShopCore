namespace ShopCore.Services.Context
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using Microsoft.EntityFrameworkCore;
    using ShopCore.Data.Models;

    public static class ExtensionMethods
    {
        public static Item FindItemByGuid(this DbSet<Item> items, Guid itemId)
        {
            return items.Where(check => check.Id == itemId)
                .FirstOrDefault();
        }
    }
}

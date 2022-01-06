using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using ShopCore.Data.Context;
using ShopCore.Data.Models;
using ShopCore.Services.Interfaces;

namespace ShopCore.Services.Repositories
{
    public class ItemRepository : IItemRepository
    {
        private ShopDBContext context;

        public ItemRepository(ShopDBContext context)
        {
            this.context = context;
        }

        public List<Category> GetCategories()
        {
            return this.context.Categories.Select(x => new Category { Id = x.Id, Name = x.Name }).ToList();
        }

        public void AddItem(Item objItem)
        {
            this.context.Items.Add(objItem);
        }

        public void Save()
        {
            this.context.SaveChanges();
        }
    }
}

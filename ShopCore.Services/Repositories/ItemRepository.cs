namespace ShopCore.Services.Repositories
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Linq.Expressions;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Http;
    using ShopCore.Data.Models;
    using ShopCore.Services.Context;
    using ShopCore.Services.Interfaces;
    using ShopCore.Services.ViewModel;

    internal class ItemRepository : IItemRepository
    {
        private ShopDBContext context;

        public ItemRepository(ShopDBContext context)
        {
            this.context = context;
        }

        public List<CategoryViewModel> GetCategories()
        {
            return this.context.Categories
                .Select(x => new CategoryViewModel { Id = x.Id, Name = x.Name })
                .ToList();
        }

        public void Add(ItemViewModel itemViewModel, string newFileName, IFormFile files)
        {
            // TODO create item constructor and hide object creation logic there

            var item = new Item(
                      itemViewModel.CategoryId,
                      itemViewModel.Description,
                      itemViewModel.Code,
                      itemViewModel.Name,
                      itemViewModel.Brand,
                      itemViewModel.Price);

            item.ImageName = newFileName;

            using (var target = new MemoryStream())
            {
                files.CopyTo(target);
                item.ImageContent = target.ToArray();
            }

            this.context.Items.Add(item);
        }
    }
}

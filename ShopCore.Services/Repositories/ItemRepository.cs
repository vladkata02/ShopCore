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

        public void Add(ItemViewModel objectItemViewModel, string newFileName, IFormFile files)
        {
            Item objectItem = new Item();
            objectItem.ImageName = newFileName;
            objectItem.CategoryId = objectItemViewModel.CategoryId;
            objectItem.Description = objectItemViewModel.Description;
            objectItem.Code = objectItemViewModel.Code;
            objectItem.Id = Guid.NewGuid();
            objectItem.Name = objectItemViewModel.Name;
            objectItem.Brand = objectItemViewModel.Brand;
            objectItem.Price = objectItemViewModel.Price;

            using (var target = new MemoryStream())
            {
                files.CopyTo(target);
                objectItem.ImageContent = target.ToArray();
            }

            this.context.Items.Add(objectItem);
        }
    }
}

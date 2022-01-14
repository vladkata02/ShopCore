namespace ShopCore.Services.Interfaces
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Http;
    using ShopCore.Data.Models;
    using ShopCore.Services.ViewModel;

    public interface IItemRepository
    {
        List<Category> GetCategories();

        void AddItem(ItemViewModel objectItemViewModel, string newFileName, IFormFile files);

        void Save();
    }
}

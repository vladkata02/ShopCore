namespace ShopCore.Services.Interfaces
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using ShopCore.Data.Models;

    public interface IItemRepository
    {
        List<Category> GetCategories();

        void AddItem(Item objItem);

        void Save();
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ShopCore.Data.Models;

namespace ShopCore.Services.Interfaces
{
    public interface IItemRepository
    {
        IEnumerable<Category> GetCategories();

        void AddItem(Item objItem);

        void Save();
    }
}

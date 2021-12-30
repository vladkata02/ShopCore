using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ShopCore.Models;

namespace ShopCore.Mvc.Interfaces
{
    public interface IItemRepository
    {
        IEnumerable<Category> GetCategories();

        void AddItem(Item objItem);

        void Save();
    }
}

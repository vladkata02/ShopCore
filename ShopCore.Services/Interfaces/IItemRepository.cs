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
        // NOTICE It's recommended to use abstraction(interfaces) rather than concrete implementation(classes) when dealing with API contracts(interfaces)
        // Instead of List IList may be used
        // The whole point is when interfaces are used, every class that implements the interface may be used.
        List<CategoryViewModel> GetCategories();

        // TODO object is redudant in objectItemViewModel
        void Add(ItemViewModel objectItemViewModel, string newFileName, byte[] imageContent);
    }
}

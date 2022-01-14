namespace ShopCore.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.Configuration;
    using ShopCore.Data.Models;
    using ShopCore.Services.Interfaces;
    using ShopCore.Services.Repositories;
    using ShopCore.Services.ViewModel;

    public class ItemController : Controller
    {
        private IItemRepository itemRepository;

        public ItemController(IItemRepository itemRepository)
        {
            this.itemRepository = itemRepository;
        }

        public IActionResult Index()
        {
            List<CategoryViewModel> categoryList = this.itemRepository.GetCategories();
            this.ViewBag.CategoriesList = categoryList;

            return this.View();
        }

        [HttpPost]
        public IActionResult Index(ItemViewModel objectItemViewModel, IFormFile files)
        {
            string newFileName = Utilities.File.GetFileFullName(files);
            this.itemRepository.AddItem(objectItemViewModel, newFileName, files);
            this.itemRepository.Save();

            return this.RedirectToAction("Index");
        }
    }
}
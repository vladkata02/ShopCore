namespace ShopCore.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.IO;
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
            List<Category> categoryList = this.itemRepository.GetCategories();
            this.ViewBag.CategoriesList = categoryList;
            return this.View();
        }

        [HttpPost]
        public IActionResult Index(ItemViewModel objectItemViewModel, IFormFile files)
        {
            var fileName = Path.GetFileName(files.FileName);
            var fileExtension = Path.GetExtension(fileName);
            var newFileName = string.Concat(Convert.ToString(Guid.NewGuid()), fileExtension);

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

            this.itemRepository.AddItem(objectItem);
            this.itemRepository.Save();

            return this.RedirectToAction("Index");
        }
    }
}
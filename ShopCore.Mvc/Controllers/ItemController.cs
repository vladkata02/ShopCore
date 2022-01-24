namespace ShopCore.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.Configuration;
    using ShopCore.Services.Interfaces;
    using ShopCore.Services.Repositories;
    using ShopCore.Services.ViewModel;

    public class ItemController : Controller
    {
        private IItemRepository itemRepository;
        private IUnitOfWork unitOfWork;

        public ItemController(IItemRepository itemRepository, IUnitOfWork unitOfWork)
        {
            this.itemRepository = itemRepository;
            this.unitOfWork = unitOfWork;
        }

        public IActionResult Index()
        {
            List<CategoryViewModel> listOfCategories = this.itemRepository.GetCategories();
            this.ViewBag.CategoriesList = listOfCategories;

            return this.View();
        }

        [HttpPost]
        public IActionResult Index(ItemViewModel objectItemViewModel, IFormFile files)
        {

            string newFileName = Utilities.File.GetFileFullName(files);
            byte[] imageContent = Utilities.File.GetImageContent(files);

            this.itemRepository.Add(objectItemViewModel, newFileName, imageContent);
            this.unitOfWork.SaveChanges();

            return this.RedirectToAction("Index");
        }
    }
}
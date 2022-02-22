namespace ShopCore.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.Logging;
    using ShopCore.Services.Interfaces;
    using ShopCore.Services.Repositories;
    using ShopCore.Services.ViewModel;

    public class ItemController : Controller
    {
        private readonly ILogger<ItemController> logger;
        private IItemRepository itemRepository;
        private IUnitOfWork unitOfWork;

        public ItemController(IItemRepository itemRepository, IUnitOfWork unitOfWork, ILogger<ItemController> logger)
        {
            this.logger = logger;
            this.itemRepository = itemRepository;
            this.unitOfWork = unitOfWork;
        }

        [Authorize(Roles = "admin")]
        public IActionResult Index()
        {
            IList<CategoryViewModel> listOfCategories = this.itemRepository.GetCategories();
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
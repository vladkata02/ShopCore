﻿namespace ShopCore.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
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
            string userName = this.HttpContext.User.Identity.Name;
            this.TempData["username"] = userName;
            ItemViewModel objItemViewModel = new ItemViewModel();
            objItemViewModel.CategorySelectListItem = this.itemRepository.GetCategories();
            return this.View(objItemViewModel);
        }

        [HttpPost]
        public IActionResult Index(ItemViewModel objItemViewModel, IFormFile files)
        {
            string userName = this.HttpContext.User.Identity.Name;
            this.TempData["username"] = userName;

            var fileName = Path.GetFileName(files.FileName);
            var fileExtension = Path.GetExtension(fileName);
            var newFileName = string.Concat(Convert.ToString(Guid.NewGuid()), fileExtension);

            Item objItem = new Item();
            objItem.ImageName = newFileName;
            objItem.CategoryId = objItemViewModel.CategoryId;
            objItem.Description = objItemViewModel.Description;
            objItem.Code = objItemViewModel.ItemCode;
            objItem.Id = Guid.NewGuid();
            objItem.Name = objItemViewModel.ItemName;
            objItem.Brand = objItemViewModel.ItemBrand;
            objItem.Price = objItemViewModel.ItemPrice;

            using (var target = new MemoryStream())
            {
                files.CopyTo(target);
                objItem.ImageContent = target.ToArray();
            }

            this.itemRepository.AddItem(objItem);
            this.itemRepository.Save();

            return this.RedirectToAction("Index");
        }
    }
}
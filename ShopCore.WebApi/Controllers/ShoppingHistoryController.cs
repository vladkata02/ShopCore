﻿namespace ShopCore.WebApi.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Mvc;
    using ShopCore.Services.Interfaces;
    using ShopCore.Services.ViewModel;

    [Route("api/ShoppingHistoryController")]
    [ApiController]
    public class ShoppingHistoryController : ControllerBase
    {
        private IShoppingHistoryRepository shoppingHistoryRepository;

        public ShoppingHistoryController(IShoppingHistoryRepository shoppingHistoryRepository)
        {
            this.shoppingHistoryRepository = shoppingHistoryRepository;
        }

        [HttpGet("{id}")]
        public List<ShoppingHistoryModel> Get(string id)
        {
            string userName = id.ToString();
            List<ShoppingHistoryModel> listOfShoppingHistory = new List<ShoppingHistoryModel>();
            listOfShoppingHistory = this.shoppingHistoryRepository.GetShoppingHistory(userName, listOfShoppingHistory);

            return listOfShoppingHistory;
        }
    }
}

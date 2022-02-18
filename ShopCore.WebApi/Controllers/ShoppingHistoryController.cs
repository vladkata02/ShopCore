namespace ShopCore.WebApi.Controllers
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

        [HttpGet("{id}/{typeLogin}")]
        public List<ShoppingHistoryViewModel> Get(string id, string login)
        {
            string userName = id.ToString();
            string typeLogin = login.ToString();
            return this.shoppingHistoryRepository.GetShoppingHistory(userName, typeLogin);
        }
    }
}

namespace ShopCore.WebApi.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Mvc;
    using ShopCore.Services.Interfaces;
    using ShopCore.WebApi.ViewModel;

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
            List<ShoppingHistoryModel> list = new List<ShoppingHistoryModel>();

            foreach (var order in this.shoppingHistoryRepository.FindAccOrders(userName))
            {
                ShoppingHistoryModel objShoppingHistoryModel = new ShoppingHistoryModel();
                objShoppingHistoryModel.OrderDetailId = order.Id;
                objShoppingHistoryModel.OrderNumber = order.OrderId;
                objShoppingHistoryModel.ItemId = order.ItemId;
                objShoppingHistoryModel.UnitPrice = order.UnitPrice;
                objShoppingHistoryModel.Total = order.Total;

                var findDate = this.shoppingHistoryRepository.FindDateById(order);
                objShoppingHistoryModel.OrderDate = findDate.Date;

                var findElementById = this.shoppingHistoryRepository.FindItemByIdForOrders(order);
                objShoppingHistoryModel.ItemBrand = findElementById.Brand;
                objShoppingHistoryModel.ItemName = findElementById.Name;
                objShoppingHistoryModel.Quantity = order.Quantity;
                objShoppingHistoryModel.User = userName;

                list.Add(objShoppingHistoryModel);
            }

            return list;
        }
    }
}

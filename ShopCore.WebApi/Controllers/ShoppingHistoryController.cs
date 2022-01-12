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

        [HttpGet("{id}")]
        public List<ShoppingHistoryModel> Get(string id)
        {
            string userName = id.ToString();
            List<ShoppingHistoryModel> listOfShoppingHistory = new List<ShoppingHistoryModel>();

            foreach (var order in this.shoppingHistoryRepository.FindAccOrders(userName))
            {
                ShoppingHistoryModel objShoppingHistoryModel = new ShoppingHistoryModel();
                objShoppingHistoryModel.OrderDetailId = order.Id;
                objShoppingHistoryModel.OrderNumber = order.OrderId;
                objShoppingHistoryModel.ItemId = order.ItemId;
                objShoppingHistoryModel.UnitPrice = order.UnitPrice;
                objShoppingHistoryModel.Total = order.Total;

                var foundDate = this.shoppingHistoryRepository.FindDateById(order);
                objShoppingHistoryModel.OrderDate = foundDate.Date;

                var foundElementById = this.shoppingHistoryRepository.FindItemByIdForOrders(order);
                objShoppingHistoryModel.ItemBrand = foundElementById.Brand;
                objShoppingHistoryModel.ItemName = foundElementById.Name;
                objShoppingHistoryModel.Quantity = order.Quantity;
                objShoppingHistoryModel.Account = userName;

                listOfShoppingHistory.Add(objShoppingHistoryModel);
            }

            return listOfShoppingHistory;
        }
    }
}

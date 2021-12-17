namespace ShopCore.WebApi.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Mvc;
    using ShopCore.Data.Context;
    using ShopCore.WebApi.ViewModel;

    [Route("api/ShoppingHistoryController")]
    [ApiController]
    public class ShoppingHistoryController : ControllerBase
    {
        private readonly ShopDBContext context;

        public ShoppingHistoryController(ShopDBContext context)
        {
            this.context = context;
        }

        [HttpGet("{id}")]
        public List<ShoppingHistoryModel> Get(string id)
        {
            string userName = id.ToString();
            List<ShoppingHistoryModel> list = new List<ShoppingHistoryModel>();

            foreach (var order in this.context.OrderDetails.Where(element => element.OrderAccMail == userName))
            {
                ShoppingHistoryModel objShoppingHistoryModel = new ShoppingHistoryModel();
                objShoppingHistoryModel.OrderDetailId = order.OrderDetailId;
                objShoppingHistoryModel.OrderNumber = order.OrderId;
                objShoppingHistoryModel.ItemId = order.ItemId;
                objShoppingHistoryModel.UnitPrice = order.UnitPrice;
                objShoppingHistoryModel.Total = order.Total;

                var findDate = this.context.Orders.Where(check => check.OrderId == order.OrderId).FirstOrDefault();
                objShoppingHistoryModel.OrderDate = findDate.OrderDate;

                var findElementById = this.context.Items.Where(check => check.ItemId.ToString() == order.ItemId).FirstOrDefault();
                objShoppingHistoryModel.ItemBrand = findElementById.ItemBrand;
                objShoppingHistoryModel.ItemName = findElementById.ItemName;
                objShoppingHistoryModel.Quantity = order.Quantity;
                objShoppingHistoryModel.User = userName;

                list.Add(objShoppingHistoryModel);
            }

            return list;
        }
    }
}

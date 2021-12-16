using Microsoft.AspNetCore.Mvc;
using ShopCore.Data.Context;
using ShopCore.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace ShopCore.Controllers
{
    [Route("api/ShoppingHistoryController")]
    [ApiController]
    public class ShoppingHistoryController : ControllerBase
    {
        private readonly ShopDBContext _context;


        public ShoppingHistoryController(ShopDBContext context) 
        {
            _context = context;
        }

        [HttpGet("{id}")]
        public List<ShoppingHistoryModel> Get(string id)
        {

            string userName = id.ToString();
            List<ShoppingHistoryModel> list = new List<ShoppingHistoryModel>();
            foreach (var order in _context.OrderDetails.Where(element => element.OrderAccMail == userName))
            {

                ShoppingHistoryModel objShoppingHistoryModel = new ShoppingHistoryModel();
                objShoppingHistoryModel.OrderDetailId = order.OrderDetailId;
                objShoppingHistoryModel.OrderNumber = order.OrderId;
                objShoppingHistoryModel.ItemId = order.ItemId;
                objShoppingHistoryModel.UnitPrice = order.UnitPrice;
                objShoppingHistoryModel.Total = order.Total;

                var findDate = _context.Orders.Where(check => check.OrderId == order.OrderId).FirstOrDefault();
                objShoppingHistoryModel.OrderDate = findDate.OrderDate;

                var findElementById = _context.Items.Where(check => check.ItemId.ToString() == order.ItemId).FirstOrDefault();
                objShoppingHistoryModel.ItemBrand = findElementById.ItemBrand;
                objShoppingHistoryModel.ItemName = findElementById.ItemName;
                objShoppingHistoryModel.Quantity = order.Quantity;

                objShoppingHistoryModel.User = userName;

                list.Add(objShoppingHistoryModel);

            }
            return list;
        }

        // GET api/<ShoppingHistoryController>/5
       
        // POST api/<ShoppingHistoryController>
        // PUT api/<ShoppingHistoryController>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/<ShoppingHistoryController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}

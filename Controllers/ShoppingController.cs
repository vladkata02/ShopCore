using ShopCore.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using ShopCore.ViewModel;
using Microsoft.Extensions.Configuration;
using ShopCore.Data;

namespace ShopCore.Controllers
{
    public class ShoppingController : Controller
    {
        private readonly ShopDBEntities3 objCartDbEntities;
        private List<ShoppingCartModel> listOfShoppingCartModels;


        public ShoppingController(ShopDBEntities3 _objCartDbEntities)
        {
            objCartDbEntities = _objCartDbEntities;
            listOfShoppingCartModels = new List<ShoppingCartModel>();
        }

        // GET: Shopping
        public IActionResult Index1()
        {

            IEnumerable<ShoppingViewModel> listOfShoppingViewModels = (from objItem in objCartDbEntities.Items
                                                                       join
                                                                           objCate in objCartDbEntities.Categories
                                                                           on objItem.CategoryId equals objCate.CategoryId
                                                                       select new ShoppingViewModel()
                                                                       {

                                                                           ItemName = objItem.ItemName,
                                                                           ImagePath = objItem.ImagePath,
                                                                           Description = objItem.Description,
                                                                           ItemPrice = objItem.ItemPrice,
                                                                           ItemBrand = objItem.ItemBrand,
                                                                           ItemId = objItem.ItemId,
                                                                           Category = objCate.CategoryName,
                                                                           ItemCode = objItem.ItemCode
                                                                       }
                                                                        ).ToList();
            return View(listOfShoppingViewModels);
        }
        [HttpPost]
        public JsonResult Index1(string ItemId)
        {

            ShoppingCartModel objShoppingCartModel = new ShoppingCartModel();
            Item objItem = objCartDbEntities.Items.Single(model => model.ItemId.ToString() == ItemId);

            if (ViewData["CartCounter"] != null)
            {
                listOfShoppingCartModels = ViewData["CartItem"] as List<ShoppingCartModel>;
            }
            if (listOfShoppingCartModels.Any(model => model.ItemId == ItemId))
            {
                objShoppingCartModel = listOfShoppingCartModels.Single(model => model.ItemId == ItemId);
                objShoppingCartModel.Quantity = objShoppingCartModel.Quantity + 1;
                objShoppingCartModel.Total = objShoppingCartModel.Quantity * objShoppingCartModel.UnitPrice;
            }
            else
            {
                objShoppingCartModel.ItemId = ItemId;
                objShoppingCartModel.ImagePath = objItem.ImagePath;
                objShoppingCartModel.ItemName = objItem.ItemName;
                objShoppingCartModel.Quantity = 1;
                objShoppingCartModel.Total = objItem.ItemPrice;
                objShoppingCartModel.UnitPrice = objItem.ItemPrice;
                listOfShoppingCartModels.Add(objShoppingCartModel);
            }
            ViewData["CartCounter"] = listOfShoppingCartModels.Count;
            ViewData["CartItem"] = listOfShoppingCartModels;
            return Json(new { Success = true, Counter = listOfShoppingCartModels.Count });
        }

        public IActionResult ShoppingCart()
        {
            listOfShoppingCartModels = ViewData["CartItem"] as List<ShoppingCartModel>;
            return View(listOfShoppingCartModels);
        }

        [HttpPost]
        public IActionResult AddOrder()
        {
            int OrderId = 0;
            listOfShoppingCartModels = ViewData["CartItem"] as List<ShoppingCartModel>;
            Order orderObj = new Order()
            {
                OrderDate = DateTime.Now,
                OrderNumber = String.Format("{0:ddmmyyyyyHHmmsss}", DateTime.Now)
            };
            objCartDbEntities.Orders.Add(orderObj);
            objCartDbEntities.SaveChanges();
            OrderId = orderObj.OrderId;

            foreach (var item in listOfShoppingCartModels)
            {
                OrderDetail objOrderDetail = new OrderDetail();
                objOrderDetail.Total = item.Total;
                objOrderDetail.ItemId = item.ItemId;
                objOrderDetail.OrderId = OrderId;
                objOrderDetail.Quantity = item.Quantity;
                objOrderDetail.UnitPrice = item.UnitPrice;
                objOrderDetail.OrderAccMail = (string)ViewData["Username"];
                objCartDbEntities.OrderDetails.Add(objOrderDetail);
                objCartDbEntities.SaveChanges();
            }

            ViewData["CartItem"] = null;
            ViewData["CartCounter"] = null;

            return RedirectToAction("Index");
        }
        public IActionResult ShoppingHistory()
        {
            string userName = ViewData["Username"].ToString();
            List<ShoppingHistoryModel> list = new List<ShoppingHistoryModel>();
            foreach (var order in objCartDbEntities.OrderDetails.Where(element => element.OrderAccMail == userName))
            {
                
                ShoppingHistoryModel objShoppingHistoryModel = new ShoppingHistoryModel();
                objShoppingHistoryModel.OrderDetailId = order.OrderDetailId;
                objShoppingHistoryModel.OrderNumber = order.OrderId;
                objShoppingHistoryModel.ItemId = order.ItemId;
                objShoppingHistoryModel.UnitPrice = order.UnitPrice;
                objShoppingHistoryModel.Total = order.Total;
                
                var findDate = objCartDbEntities.Orders.Where(check => check.OrderId == order.OrderId).FirstOrDefault();
                objShoppingHistoryModel.OrderDate = findDate.OrderDate;

                var findElementById = objCartDbEntities.Items.Where(check => check.ItemId.ToString() == order.ItemId).FirstOrDefault();
                objShoppingHistoryModel.ImagePath = findElementById.ImagePath;
                objShoppingHistoryModel.ItemBrand = findElementById.ItemBrand;
                objShoppingHistoryModel.ItemName = findElementById.ItemName;
                objShoppingHistoryModel.Quantity = order.Quantity;

                objShoppingHistoryModel.User = userName;

                list.Add(objShoppingHistoryModel);
               
            }
            return View(list);
        }
    }
}
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
        private readonly ShopDBContext _context;


        public ShoppingController(ShopDBContext context)
        {
            _context = context;
        }
        // GET: Shopping
        public IActionResult Index()
        {
            string userName = HttpContext.User.Identity.Name;
            TempData["username"] = userName;
            IEnumerable<ShoppingViewModel> listOfShoppingViewModels = (from objItem in _context.Items
                                                                       join
                                                                           objCate in _context.Categories
                                                                           on objItem.CategoryId equals objCate.CategoryId
                                                                       select new ShoppingViewModel()
                                                                       {

                                                                           ItemName = objItem.ItemName,
                                                                           //ImagePath = objItem.ImagePath,
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
        public JsonResult Index(string ItemId)
        {
            string userName = HttpContext.User.Identity.Name;
            TempData["username"] = userName;
            Cart objShoppingCartModel = new Cart();
            Item objItem = _context.Items.Single(model => model.ItemId.ToString() == ItemId);

            var ifCheckId = _context.Carts.SingleOrDefault(model => model.ItemId == ItemId);
            if (ifCheckId==null)
            {
                objShoppingCartModel.CartId = _context.Carts.Count()+1;
                objShoppingCartModel.ItemId = ItemId;
                //objShoppingCartModel.ImagePath = objItem.ImagePath;
                objShoppingCartModel.ItemName = objItem.ItemName;
                objShoppingCartModel.Quantity = 1;
                objShoppingCartModel.Total = objItem.ItemPrice;
                objShoppingCartModel.CartAcc = userName;
                objShoppingCartModel.UnitPrice = objItem.ItemPrice;
                _context.Carts.Add(objShoppingCartModel);
                
               
            }
            else
            {
                var checkId = _context.Carts.Single(model => model.ItemId == ItemId);
                checkId.Quantity++;
                checkId.Total = checkId.Quantity * checkId.UnitPrice;
            }
            _context.SaveChanges();
            return Json(new { Success = true});
        }
        public IActionResult ShoppingCart()
        {
            string userName = HttpContext.User.Identity.Name;
            TempData["username"] = userName;                   
            List<ShoppingCartModel> list = new List<ShoppingCartModel>();                                
            foreach (var cart in _context.Carts.Where(element => element.CartAcc == userName))                 
            {

                ShoppingCartModel ObjCart = new ShoppingCartModel();                                     
                ObjCart.ItemId = cart.ItemId;                                                                  
                ObjCart.UnitPrice = cart.UnitPrice;                                                            
                ObjCart.Total = cart.Total;                                                                    

                var findElementById = _context.Items.Where(check => check.ItemId.ToString() == cart.ItemId).FirstOrDefault();
                //objShoppingHistoryModel.ImagePath = findElementById.ImagePath;
                ObjCart.ItemBrand = findElementById.ItemBrand;
                ObjCart.ItemName = findElementById.ItemName;
                ObjCart.Quantity = cart.Quantity;
                ObjCart.CartAcc = userName;

                list.Add(ObjCart);

            }
            return View(list);
        }

        [HttpPost]
        public IActionResult AddOrder()
        {
            string userName = HttpContext.User.Identity.Name;
            TempData["username"] = userName;
            int OrderId = 0;
            //listOfShoppingCartModels = TempData["CartItem"] as List<ShoppingCartModel>;
            Order orderObj = new Order()
            {
                OrderDate = DateTime.Now,
                OrderNumber = String.Format("{0:ddmmyyyyyHHmmsss}", DateTime.Now)
            };
            _context.Orders.Add(orderObj);
            _context.SaveChanges();
            OrderId = orderObj.OrderId;

            foreach (var item in _context.Carts.Where(checkAcc=> checkAcc.CartAcc== userName))
            {
                OrderDetail objOrderDetail = new OrderDetail();
                objOrderDetail.Total = item.Total;
                objOrderDetail.ItemId = item.ItemId;
                objOrderDetail.OrderId = OrderId;
                objOrderDetail.Quantity = item.Quantity;
                objOrderDetail.UnitPrice = item.UnitPrice;
                objOrderDetail.OrderAccMail = userName;
                _context.OrderDetails.Add(objOrderDetail);
                _context.SaveChanges();
            }

            TempData["CartItem"] = null;
            TempData["CartCounter"] = null;

            return RedirectToAction("Index");
        }
        public IActionResult ShoppingHistory()
        {
            string userName = HttpContext.User.Identity.Name;
            TempData["username"] = userName;
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
                //objShoppingHistoryModel.ImagePath = findElementById.ImagePath;
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
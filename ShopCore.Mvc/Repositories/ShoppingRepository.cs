﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using ShopCore.Data.Context;
using ShopCore.Models;
using ShopCore.Mvc.Interfaces;
using ShopCore.ViewModel;

namespace ShopCore.Mvc.Repositories
{
    public class ShoppingRepository : IShoppingRepository
    {
        private ShopDBContext context;

        public ShoppingRepository(ShopDBContext context)
        {
            this.context = context;
        }

        public IEnumerable<Category> GetCategories()
        {
            return this.context.Categories.ToList();
        }

        public IEnumerable<Item> GetItems()
        {
            return this.context.Items.ToList();
        }

        public Item CheckId(string itemId)
        {
            return this.context.Items.Single(model => model.ItemId.ToString() == itemId);
        }

        public Cart IfCheckId(string itemId, string userName)
        {
            return this.context.Carts.SingleOrDefault(model => model.ItemId == itemId && model.CartAcc == userName);
        }

        public int TableCount()
        {
            return this.context.Carts.Count() + 1;
        }

        public void AddCartItem(Cart objShoppingCartModel)
        {
            this.context.Carts.Add(objShoppingCartModel);
        }

        public Cart CheckIdForQuantity(string itemId, string userName)
        {
            return this.context.Carts.Single(model => model.ItemId == itemId && model.CartAcc == userName);
        }

        public IEnumerable<Cart> CheckWhichAccCartIs(string userName)
        {
            return this.context.Carts.Where(element => element.CartAcc == userName);
        }

        public Item FindElementById(Cart cart)
        {
            return this.context.Items.Where(check => check.ItemId.ToString() == cart.ItemId).FirstOrDefault();
        }

        public void UpdatePrice(Item entity)
        {
            this.context.Entry(entity).State = EntityState.Modified;
        }

        public IEnumerable<Price> WhereId(Guid itemId)
        {
            return this.context.Prices.Where(element => element.ItemId == itemId.ToString());
        }

        public Item FindItemById(Guid itemId, PriceHistoryViewModel objPriceHistoryModel)
        {
            return this.context.Items.Where(check => check.ItemId.ToString() == objPriceHistoryModel.ItemId).FirstOrDefault();
        }

        public void AddOrderTime(Order orderObj)
        {
            this.context.Orders.Add(orderObj);
        }

        public void AddOrderDetails(OrderDetail objOrderDetail)
        {
            this.context.OrderDetails.Add(objOrderDetail);
        }

        public IEnumerable<OrderDetail> FindAccOrders(string userName)
        {
            return this.context.OrderDetails.Where(element => element.OrderAccMail == userName);
        }

        public Order FindDateById(OrderDetail order)
        {
            return this.context.Orders.Where(check => check.OrderId == order.OrderId).FirstOrDefault();
        }

        public Item FindItemByIdForOrders(OrderDetail order)
        {
            return this.context.Items.Where(check => check.ItemId.ToString() == order.ItemId).FirstOrDefault();
        }

        public void RemoveItem(Cart item)
        {
            this.context.Carts.Remove(item);
        }

        public void Save()
        {
            this.context.SaveChanges();
        }
    }
}

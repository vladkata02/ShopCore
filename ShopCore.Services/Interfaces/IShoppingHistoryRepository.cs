﻿namespace ShopCore.Services.Interfaces
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using ShopCore.Data.Models;
    using ShopCore.Services.ViewModel;

    public interface IShoppingHistoryRepository
    {
        Order FindDateById(OrderDetail order);

        Item FindItemByIdForOrders(OrderDetail order);

        IEnumerable<OrderDetail> FindAccOrders(string userName);

        List<ShoppingHistoryViewModel> GetShoppingHistory(string userName, List<ShoppingHistoryViewModel> listOfShoppingHistory);
    }
}

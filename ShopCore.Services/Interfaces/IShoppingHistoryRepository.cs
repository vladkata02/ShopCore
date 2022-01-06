﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ShopCore.Data.Models;
using ShopCore.Services.ViewModel;

namespace ShopCore.Services.Interfaces
{
    public interface IShoppingHistoryRepository
    {
        Order FindDateById(OrderDetail order);

        Item FindItemByIdForOrders(OrderDetail order);

        IEnumerable<OrderDetail> FindAccOrders(string userName);
    }
}

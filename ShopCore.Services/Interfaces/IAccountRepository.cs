﻿namespace ShopCore.Services.Interfaces
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using ShopCore.Data.Models;
    using ShopCore.Services.ViewModel;

    public interface IAccountRepository
    {
        void Add(User user);

        void Save();

        User LoginCheck(LoginViewModel model);
    }
}
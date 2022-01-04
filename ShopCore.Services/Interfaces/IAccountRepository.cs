using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ShopCore.Models;
using ShopCore.Services.ViewModel;

namespace ShopCore.Services.Interfaces
{
    public interface IAccountRepository
    {
        void AddAccount(User user);

        void Save();

        User LoginCheck(LoginViewModel model);
    }
}

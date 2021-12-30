using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ShopCore.Models;
using ShopCore.ViewModel;

namespace ShopCore.Mvc.Interfaces
{
    public interface IAccountRepository
    {
        void AddAccount(User user);

        void Save();

        User LoginCheck(LoginViewModel model);
    }
}

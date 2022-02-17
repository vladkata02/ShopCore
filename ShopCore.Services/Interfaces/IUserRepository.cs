namespace ShopCore.Services.Interfaces
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using ShopCore.Data.Models;
    using ShopCore.Services.ViewModel;

    public interface IUserRepository
    {
        void Add(RegisterViewModel model);

        void FacebookAdd(string userName, string email);

        bool UserExists(string email);

        UserViewModel LoginVerification(LoginViewModel model);
    }
}

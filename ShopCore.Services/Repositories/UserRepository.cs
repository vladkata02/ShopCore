namespace ShopCore.Services.Repositories
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Linq.Expressions;
    using System.Threading.Tasks;
    using ShopCore.Data.Context;
    using ShopCore.Data.Models;
    using ShopCore.Services.Interfaces;
    using ShopCore.Services.ViewModel;

    public class UserRepository : IUserRepository
    {
        private ShopDBContext context;

        public UserRepository(ShopDBContext context)
        {
            this.context = context;
        }

        public User LoginCheck(LoginViewModel model)
        {
            return this.context.Users
                .Where(usr => usr.Username == model.UserName && usr.Password == model.Password)
                .SingleOrDefault();
        }

        public void Add(User user)
        {
            this.context.Users.Add(user);
        }

        public void Save()
        {
            this.context.SaveChanges();
        }
    }
}

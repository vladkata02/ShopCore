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

        public UserViewModel LoginVerification(LoginViewModel model)
        {
            var entityUser = this.context.Users
                .Where(usr => usr.Username == model.UserName && usr.Password == model.Password)
                .SingleOrDefault();
            UserViewModel user = new UserViewModel();
            user.Id = entityUser.Id;
            user.Username = entityUser.Username;
            user.Password = entityUser.Password;
            user.Roles = entityUser.Roles;

            return user;
        }

        public void Add(RegisterViewModel model)
        {
            User user = new User();
            user.Username = model.UserName;
            user.Password = model.Password;
            user.Roles = "Manager,Admin";
            this.context.Users.Add(user);
        }

        public void Save()
        {
            this.context.SaveChanges();
        }
    }
}

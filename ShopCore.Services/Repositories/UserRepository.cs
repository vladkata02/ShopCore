namespace ShopCore.Services.Repositories
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Linq.Expressions;
    using System.Threading.Tasks;
    using Microsoft.Extensions.Logging;
    using ShopCore.Data.Models;
    using ShopCore.Services.Context;
    using ShopCore.Services.Interfaces;
    using ShopCore.Services.ViewModel;

    internal class UserRepository : IUserRepository
    {
        private readonly ILogger<UserRepository> logger;
        private ShopDBContext context;

        public UserRepository(ShopDBContext context, ILogger<UserRepository> logger)
        {
            this.logger = logger;
            this.context = context;
        }

        public UserViewModel LoginVerification(LoginViewModel model)
        {
            var entityUser = this.context.Users
                .Where(usr => usr.Username == model.UserName && usr.Password == model.Password)
                .SingleOrDefault();

            if (entityUser == null)
            {
                return null;
            }

            UserViewModel user = new UserViewModel(
                entityUser.Id,
                entityUser.Username,
                entityUser.Password,
                entityUser.Roles,
                entityUser.FullName);

            return user;
        }

        public void Add(RegisterViewModel model)
        {
            const string roles = "Manager,Admin";
            User user = new User(model.UserName, model.Password, roles);
            this.context.Users.Add(user);
        }
    }
}

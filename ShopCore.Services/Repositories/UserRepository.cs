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
        private IUnitOfWork unitOfWork;

        public UserRepository(ShopDBContext context, ILogger<UserRepository> logger, IUnitOfWork unitOfWork)
        {
            this.logger = logger;
            this.context = context;
            this.unitOfWork = unitOfWork;
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
                entityUser.FullName,
                entityUser.Email);

            return user;
        }

        public bool UserExists(string email)
        {
            if (this.context.Users.Any(user => user.Email == email && user.LoginType == "Facebook"))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public void FacebookAdd(string userName, string email)
        {
            User user = new User(userName, email);
            this.context.Users.Add(user);
            this.unitOfWork.SaveChanges();
        }

        public void Add(RegisterViewModel model)
        {
            User user = new User(model.FullName, model.UserName, model.Password, model.Email);
            this.context.Users.Add(user);
        }
    }
}

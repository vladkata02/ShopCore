namespace ShopCore.Services.Interfaces
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using ShopCore.Data.Models;
    using ShopCore.Services.ViewModel;

    // Когато контекста на репото е user, би трябвало да се придържаш към същия речник
    // public interface IUserRepository
    public interface IAccountRepository
    {
        // TODO AddAccount в репо Account е тавтология
        // Ако подаваш EF entity като параметър, контролера трябва да разбира от EF класове, а това е грешно!
        void AddAccount(User user);

        void Save();

        // TODO Това име освен, че е неподходящо, обратно в контролера се връша entity вместо ViewModel!
        User LoginCheck(LoginViewModel model);
    }
}

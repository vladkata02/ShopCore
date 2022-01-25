namespace ShopCore.Services.Interfaces
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using ShopCore.Data.Models;

    public interface IUnitOfWork
    {
        Item FindItemByGuid(Guid itemId);

        void SaveChanges();
    }
}

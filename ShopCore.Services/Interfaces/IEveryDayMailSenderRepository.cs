namespace ShopCore.Services.Interfaces
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using ShopCore.Services.ViewModel;

    public interface IEveryDayMailSenderRepository
    {
        public void GetTodaysOrders(List<ShoppingHistoryViewModel> itemsSoldForTheDay);
    }
}

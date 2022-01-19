namespace ShopCore.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Mvc;
    using ShopCore.Services.Interfaces;
    using ShopCore.Services.ViewModel;

    public class PriceController : Controller
    {
        private IPriceRepository priceRepository;

        public PriceController(IPriceRepository priceRepository)
        {
            this.priceRepository = priceRepository;
        }

        [HttpPost]
        public IActionResult Index(Guid itemGuid)
        {
            PriceEditorViewModel priceEditor = this.priceRepository.GetPriceEditor(itemGuid);

            return this.View(priceEditor);
        }

        public IActionResult PriceHistory(PriceEditorViewModel priceEditor, Guid itemGuid)
        {
            if (priceEditor.CurrentPrice != 0)
            {
                this.priceRepository.ChangePrice(priceEditor, itemGuid);
            }

            List<PriceHistoryViewModel> listOfItemsHistory = new List<PriceHistoryViewModel>();
            listOfItemsHistory = this.priceRepository.GetPriceHistory(listOfItemsHistory, itemGuid);

            return this.View(listOfItemsHistory);
        }
    }
}

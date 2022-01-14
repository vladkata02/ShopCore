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
        public IActionResult Index(Guid buttonPassingId)
        {
            PriceEditorViewModel objectPriceEditor = this.priceRepository.GetPriceEditor(buttonPassingId);

            return this.View(objectPriceEditor);
        }

        public IActionResult PriceHistory(PriceEditorViewModel objectItem, Guid buttonPassingId)
        {
            if (objectItem.CurrentPrice != 0)
            {
                this.priceRepository.ChangePrice(objectItem, buttonPassingId);
            }

            List<PriceHistoryViewModel> listOfItemsHistory = new List<PriceHistoryViewModel>();
            listOfItemsHistory = this.priceRepository.GetPriceHistory(listOfItemsHistory, buttonPassingId);

            return this.View(listOfItemsHistory);
        }
    }
}

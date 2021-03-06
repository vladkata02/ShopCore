namespace ShopCore.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.Logging;
    using ShopCore.Services.Interfaces;
    using ShopCore.Services.ViewModel;

    public class PriceController : Controller
    {
        private readonly ILogger<PriceController> logger;
        private IPriceRepository priceRepository;

        public PriceController(IPriceRepository priceRepository, ILogger<PriceController> logger)
        {
            this.logger = logger;
            this.priceRepository = priceRepository;
        }

        [HttpPost]
        public IActionResult Index(Guid itemGuid)
        {
            return this.View(this.priceRepository.GetPriceEditor(itemGuid));
        }

        public IActionResult PriceHistory(PriceEditorViewModel priceEditor, Guid itemGuid)
        {
            this.priceRepository.ChangePrice(priceEditor, itemGuid);
            return this.View(this.priceRepository.GetPriceHistory(itemGuid));
        }
    }
}

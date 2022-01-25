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
        private IUnitOfWork unitOfWork;

        public PriceController(IPriceRepository priceRepository, IUnitOfWork unitOfWork)
        {
            this.priceRepository = priceRepository;
            this.unitOfWork = unitOfWork;
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

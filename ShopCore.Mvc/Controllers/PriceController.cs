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
            // TODO the business logic below depends only on priceRepository
            // and must be extracted there
            if (priceEditor.CurrentPrice != 0)
            {
                // TODO method names should contains a verb + (adjective ) + noun
                // IfAnyPricesInDatabase should be changed
                bool hasAnyPrice = this.priceRepository.IfAnyPricesInDatabase(itemGuid);
                if (!hasAnyPrice)
                {
                    this.priceRepository.AddFirstPrice(itemGuid);
                    this.unitOfWork.SaveChanges();
                }

                this.priceRepository.UpdatePrice(itemGuid, priceEditor);
                this.priceRepository.AddChangedPrice(itemGuid, priceEditor);
                this.unitOfWork.SaveChanges();
            }

            return this.View(this.priceRepository.GetPriceHistory(itemGuid));
        }
    }
}

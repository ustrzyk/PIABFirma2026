using Firma.Interfaces.CMS;
using Firma.Interfaces.Sklep;
using Microsoft.AspNetCore.Mvc;

namespace Firma.PortalWWW.Controllers
{
    public class ProducentController : PortalControllerBase
    {
        private readonly IProducentService _producentService;

        public ProducentController(
            IProducentService producentService,
            IStronaService stronaService,
            IAktualnoscService aktualnoscService,
            IUstawieniePortaluService ustawieniePortaluService)
            : base(stronaService, aktualnoscService, ustawieniePortaluService)
        {
            _producentService = producentService;
        }

        public async Task<IActionResult> Index()
        {
            await PrzygotujDaneDoLayoutu();

            // Pobieram producentów
            var items = await _producentService.GetProducenci();

            return View(items);
        }

        public async Task<IActionResult> Szczegoly(int id)
        {
            await PrzygotujDaneDoLayoutu();

            // Pobieram szczegóły producenta
            var item = await _producentService.GetProducent(id);

            if (item == null)
            {
                return NotFound();
            }

            return View(item);
        }
    }
}
using Firma.Interfaces.CMS;
using Microsoft.AspNetCore.Mvc;

namespace Firma.PortalWWW.Controllers
{
    public class PromocjaController : PortalControllerBase
    {
        private readonly IPromocjaService _promocjaService;

        public PromocjaController(
            IPromocjaService promocjaService,
            IStronaService stronaService,
            IAktualnoscService aktualnoscService,
            IUstawieniePortaluService ustawieniePortaluService)
            : base(stronaService, aktualnoscService, ustawieniePortaluService)
        {
            _promocjaService = promocjaService;
        }

        public async Task<IActionResult> Index()
        {
            await PrzygotujDaneDoLayoutu();

            // Pobieram promocje
            var items = await _promocjaService.GetPromocje();

            return View(items);
        }

        public async Task<IActionResult> Szczegoly(int id)
        {
            await PrzygotujDaneDoLayoutu();

            // Pobieram szczegóły promocji
            var item = await _promocjaService.GetPromocja(id);

            if (item == null)
            {
                return NotFound();
            }

            return View(item);
        }
    }
}
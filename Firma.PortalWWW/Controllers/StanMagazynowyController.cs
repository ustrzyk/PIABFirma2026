using Firma.Interfaces.CMS;
using Firma.Interfaces.Sklep;
using Microsoft.AspNetCore.Mvc;

namespace Firma.PortalWWW.Controllers
{
    public class StanMagazynowyController : PortalControllerBase
    {
        private readonly IStanMagazynowyService _stanMagazynowyService;

        public StanMagazynowyController(
            IStanMagazynowyService stanMagazynowyService,
            IStronaService stronaService,
            IAktualnoscService aktualnoscService,
            IUstawieniePortaluService ustawieniePortaluService)
            : base(stronaService, aktualnoscService, ustawieniePortaluService)
        {
            _stanMagazynowyService = stanMagazynowyService;
        }

        public async Task<IActionResult> Index()
        {
            await PrzygotujDaneDoLayoutu();

            // Pobieram stany magazynowe
            var items = await _stanMagazynowyService.GetStanyMagazynowe();

            return View(items);
        }

        public async Task<IActionResult> Szczegoly(int id)
        {
            await PrzygotujDaneDoLayoutu();

            // Pobieram szczegóły stanu magazynowego
            var item = await _stanMagazynowyService.GetStanMagazynowy(id);

            if (item == null)
            {
                return NotFound();
            }

            return View(item);
        }
    }
}
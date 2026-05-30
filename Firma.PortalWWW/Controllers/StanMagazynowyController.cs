using Firma.Interfaces.CMS;
using Firma.Interfaces.Sklep;
using Microsoft.AspNetCore.Mvc;

namespace Firma.PortalWWW.Controllers
{
    public class StanMagazynowyController : Controller
    {
        private readonly IStanMagazynowyService _stanMagazynowyService;
        private readonly IStronaService _stronaService;
        private readonly IAktualnoscService _aktualnoscService;

        public StanMagazynowyController(
            IStanMagazynowyService stanMagazynowyService,
            IStronaService stronaService,
            IAktualnoscService aktualnoscService)
        {
            _stanMagazynowyService = stanMagazynowyService;
            _stronaService = stronaService;
            _aktualnoscService = aktualnoscService;
        }

        public async Task<IActionResult> Index()
        {
            ViewBag.ModelStrony = await _stronaService.GetStronyByPozycja();
            ViewBag.ModelAktualnosci = await _aktualnoscService.GetAktualnoscByPozycjaTake(3);

            var items = await _stanMagazynowyService.GetStanyMagazynowe();

            return View(items);
        }

        public async Task<IActionResult> Szczegoly(int id)
        {
            ViewBag.ModelStrony = await _stronaService.GetStronyByPozycja();
            ViewBag.ModelAktualnosci = await _aktualnoscService.GetAktualnoscByPozycjaTake(3);

            var item = await _stanMagazynowyService.GetStanMagazynowy(id);

            if (item == null)
            {
                return NotFound();
            }

            return View(item);
        }
    }
}
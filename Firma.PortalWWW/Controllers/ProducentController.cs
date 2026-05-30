using Firma.Interfaces.CMS;
using Firma.Interfaces.Sklep;
using Microsoft.AspNetCore.Mvc;

namespace Firma.PortalWWW.Controllers
{
    public class ProducentController : Controller
    {
        private readonly IProducentService _producentService;
        private readonly IStronaService _stronaService;
        private readonly IAktualnoscService _aktualnoscService;

        public ProducentController(
            IProducentService producentService,
            IStronaService stronaService,
            IAktualnoscService aktualnoscService)
        {
            _producentService = producentService;
            _stronaService = stronaService;
            _aktualnoscService = aktualnoscService;
        }

        public async Task<IActionResult> Index()
        {
            ViewBag.ModelStrony = await _stronaService.GetStronyByPozycja();
            ViewBag.ModelAktualnosci = await _aktualnoscService.GetAktualnoscByPozycjaTake(3);

            var items = await _producentService.GetProducenci();

            return View(items);
        }

        public async Task<IActionResult> Szczegoly(int id)
        {
            ViewBag.ModelStrony = await _stronaService.GetStronyByPozycja();
            ViewBag.ModelAktualnosci = await _aktualnoscService.GetAktualnoscByPozycjaTake(3);

            var item = await _producentService.GetProducent(id);

            if (item == null)
            {
                return NotFound();
            }

            return View(item);
        }
    }
}
using Firma.Interfaces.CMS;
using Microsoft.AspNetCore.Mvc;

namespace Firma.PortalWWW.Controllers
{
    public class PromocjaController : Controller
    {
        private readonly IPromocjaService _promocjaService;
        private readonly IStronaService _stronaService;
        private readonly IAktualnoscService _aktualnoscService;

        public PromocjaController(
            IPromocjaService promocjaService,
            IStronaService stronaService,
            IAktualnoscService aktualnoscService)
        {
            _promocjaService = promocjaService;
            _stronaService = stronaService;
            _aktualnoscService = aktualnoscService;
        }

        public async Task<IActionResult> Index()
        {
            ViewBag.ModelStrony = await _stronaService.GetStronyByPozycja();
            ViewBag.ModelAktualnosci = await _aktualnoscService.GetAktualnoscByPozycjaTake(3);

            var items = await _promocjaService.GetPromocje();

            return View(items);
        }

        public async Task<IActionResult> Szczegoly(int id)
        {
            ViewBag.ModelStrony = await _stronaService.GetStronyByPozycja();
            ViewBag.ModelAktualnosci = await _aktualnoscService.GetAktualnoscByPozycjaTake(3);

            var item = await _promocjaService.GetPromocja(id);

            if (item == null)
            {
                return NotFound();
            }

            return View(item);
        }
    }
}
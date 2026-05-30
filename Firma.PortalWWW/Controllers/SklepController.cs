using Firma.Interfaces.CMS;
using Firma.Interfaces.Sklep;
using Microsoft.AspNetCore.Mvc;

namespace Firma.PortalWWW.Controllers
{
    public class SklepController : Controller
    {
        private readonly ITowarService _towarService;
        private readonly IStronaService _stronaService;
        private readonly IAktualnoscService _aktualnoscService;

        public SklepController(
            ITowarService towarService,
            IStronaService stronaService,
            IAktualnoscService aktualnoscService)
        {
            _towarService = towarService;
            _stronaService = stronaService;
            _aktualnoscService = aktualnoscService;
        }

        public async Task<IActionResult> Index(int? id)
        {
            ViewBag.ModelStrony = await _stronaService.GetStronyByPozycja();
            ViewBag.ModelAktualnosci = await _aktualnoscService.GetAktualnoscByPozycjaTake(3);

            var items = await _towarService.GetTowaryDanegoRodzaju(id);

            return View(items);
        }

        public async Task<IActionResult> Szczegoly(int id)
        {
            ViewBag.ModelStrony = await _stronaService.GetStronyByPozycja();
            ViewBag.ModelAktualnosci = await _aktualnoscService.GetAktualnoscByPozycjaTake(3);

            var item = await _towarService.GetTowar(id);

            if (item == null)
            {
                return NotFound();
            }

            return View(item);
        }
    }
}
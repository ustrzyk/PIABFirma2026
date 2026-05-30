using Firma.Interfaces.CMS;
using Microsoft.AspNetCore.Mvc;

namespace Firma.PortalWWW.Controllers
{
    public class UstawieniePortaluController : Controller
    {
        private readonly IUstawieniePortaluService _ustawieniePortaluService;
        private readonly IStronaService _stronaService;
        private readonly IAktualnoscService _aktualnoscService;

        public UstawieniePortaluController(
            IUstawieniePortaluService ustawieniePortaluService,
            IStronaService stronaService,
            IAktualnoscService aktualnoscService)
        {
            _ustawieniePortaluService = ustawieniePortaluService;
            _stronaService = stronaService;
            _aktualnoscService = aktualnoscService;
        }

        public async Task<IActionResult> Index()
        {
            ViewBag.ModelStrony = await _stronaService.GetStronyByPozycja();
            ViewBag.ModelAktualnosci = await _aktualnoscService.GetAktualnoscByPozycjaTake(3);

            var items = await _ustawieniePortaluService.GetUstawieniaPortalu();

            return View(items);
        }

        public async Task<IActionResult> Szczegoly(int id)
        {
            ViewBag.ModelStrony = await _stronaService.GetStronyByPozycja();
            ViewBag.ModelAktualnosci = await _aktualnoscService.GetAktualnoscByPozycjaTake(3);

            var item = await _ustawieniePortaluService.GetUstawieniePortalu(id);

            if (item == null)
            {
                return NotFound();
            }

            return View(item);
        }
    }
}
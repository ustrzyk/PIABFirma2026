using Firma.Interfaces.CMS;
using Microsoft.AspNetCore.Mvc;

namespace Firma.PortalWWW.Controllers
{
    public class AktualnoscController : Controller
    {
        private readonly IAktualnoscService _aktualnoscService;
        private readonly IStronaService _stronaService;

        public AktualnoscController(
            IAktualnoscService aktualnoscService,
            IStronaService stronaService)
        {
            _aktualnoscService = aktualnoscService;
            _stronaService = stronaService;
        }

        public async Task<IActionResult> Index(int id)
        {
            ViewBag.ModelStrony = await _stronaService.GetStronyByPozycja();
            ViewBag.ModelAktualnosci = await _aktualnoscService.GetAktualnoscByPozycjaTake(3);

            var item = await _aktualnoscService.GetAktualnosc(id);

            if (item == null)
            {
                return NotFound();
            }

            return View(item);
        }
    }
}
using Firma.Interfaces.CMS;
using Firma.Interfaces.Sklep;
using Firma.PortalWWW.Models;
using Microsoft.AspNetCore.Mvc;

namespace Firma.PortalWWW.Controllers
{
    public class StatusZamowieniaController : PortalControllerBase
    {
        private readonly IStatusZamowieniaService _statusZamowieniaService;

        public StatusZamowieniaController(
            IStatusZamowieniaService statusZamowieniaService,
            IStronaService stronaService,
            IAktualnoscService aktualnoscService,
            IUstawieniePortaluService ustawieniePortaluService)
            : base(stronaService, aktualnoscService, ustawieniePortaluService)
        {
            _statusZamowieniaService = statusZamowieniaService;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            await PrzygotujDaneDoLayoutu();
            ViewBag.UkryjAktualnosci = true;

            return View(new StatusZamowieniaViewModel());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Index(StatusZamowieniaViewModel model)
        {
            await PrzygotujDaneDoLayoutu();
            ViewBag.UkryjAktualnosci = true;

            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var wynik = await _statusZamowieniaService.SprawdzStatus(
                model.NumerZamowienia,
                model.Email);

            if (wynik == null)
            {
                ModelState.AddModelError(
                    string.Empty,
                    "Nie znaleziono zamówienia dla podanego numeru i adresu e-mail.");

                return View(model);
            }

            model.Wynik = wynik;

            return View(model);
        }
    }
}
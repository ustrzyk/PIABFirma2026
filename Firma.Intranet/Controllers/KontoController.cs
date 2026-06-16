using Firma.Intranet.Interfaces.Intranet;
using Firma.Intranet.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Firma.Intranet.Controllers
{
    public class KontoController : Controller
    {
        private readonly IKontoIntranetService _kontoIntranetService;

        public KontoController(IKontoIntranetService kontoIntranetService)
        {
            _kontoIntranetService = kontoIntranetService;
        }

        [AllowAnonymous]
        [HttpGet]
        public IActionResult Logowanie(string? returnUrl = null)
        {
            ViewData["ReturnUrl"] = returnUrl;

            return View(new LogowanieViewModel());
        }

        [AllowAnonymous]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Logowanie(
            LogowanieViewModel model,
            string? returnUrl = null)
        {
            ViewData["ReturnUrl"] = returnUrl;

            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var wynik = await _kontoIntranetService.Zaloguj(
                model.Email,
                model.Haslo,
                model.ZapamietajMnie);

            if (wynik.CzySukces)
            {
                if (!string.IsNullOrWhiteSpace(returnUrl) && Url.IsLocalUrl(returnUrl))
                {
                    return Redirect(returnUrl);
                }

                return RedirectToAction("Index", "Home");
            }

            ModelState.AddModelError(string.Empty, wynik.KomunikatBledu);

            return View(model);
        }

        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Wyloguj()
        {
            await _kontoIntranetService.Wyloguj();

            return RedirectToAction(nameof(Logowanie));
        }

        [AllowAnonymous]
        public IActionResult BrakDostepu()
        {
            return View();
        }
    }
}
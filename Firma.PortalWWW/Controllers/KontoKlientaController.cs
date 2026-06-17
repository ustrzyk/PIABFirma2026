using Firma.Interfaces.CMS;
using Firma.Interfaces.Sklep;
using Firma.PortalWWW.Models;
using Firma.Services.Data.Dto.ZamowieniaPubliczne;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Firma.PortalWWW.Controllers
{
    public class KontoKlientaController : PortalControllerBase
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly IKontoKlientaService _kontoKlientaService;

        public KontoKlientaController(
            UserManager<IdentityUser> userManager,
            SignInManager<IdentityUser> signInManager,
            IKontoKlientaService kontoKlientaService,
            IStronaService stronaService,
            IAktualnoscService aktualnoscService,
            IUstawieniePortaluService ustawieniePortaluService)
            : base(stronaService, aktualnoscService, ustawieniePortaluService)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _kontoKlientaService = kontoKlientaService;
        }

        [Authorize]
        public async Task<IActionResult> Panel()
        {
            await PrzygotujDaneDoLayoutu();
            ViewBag.UkryjAktualnosci = true;

            var email = PobierzEmailZalogowanegoKlienta();

            var zamowienia = await _kontoKlientaService.PobierzZamowieniaKlienta(email);
            var daneKlienta = await _kontoKlientaService.PobierzDaneKlienta(email);

            ViewBag.DaneKlienta = daneKlienta;

            return View(zamowienia);
        }

        [Authorize]
        public async Task<IActionResult> Dane()
        {
            await PrzygotujDaneDoLayoutu();
            ViewBag.UkryjAktualnosci = true;

            var email = PobierzEmailZalogowanegoKlienta();

            var daneKlienta = await _kontoKlientaService.PobierzDaneKlienta(email);

            if (daneKlienta == null)
            {
                return View(new KontoKlientaDaneViewModel
                {
                    Email = email
                });
            }

            return View(new KontoKlientaDaneViewModel
            {
                Imie = daneKlienta.Imie,
                Nazwisko = daneKlienta.Nazwisko,
                Email = daneKlienta.Email,
                Telefon = daneKlienta.Telefon,
                Ulica = daneKlienta.Ulica,
                NumerDomu = daneKlienta.NumerDomu,
                NumerLokalu = daneKlienta.NumerLokalu,
                KodPocztowy = daneKlienta.KodPocztowy,
                Miasto = daneKlienta.Miasto
            });
        }

        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Dane(KontoKlientaDaneViewModel model)
        {
            await PrzygotujDaneDoLayoutu();
            ViewBag.UkryjAktualnosci = true;

            model.Email = PobierzEmailZalogowanegoKlienta();
            ModelState.Remove(nameof(KontoKlientaDaneViewModel.Email));

            if (!ModelState.IsValid)
            {
                return View(model);
            }

            await _kontoKlientaService.AktualizujDaneKlienta(new KontoKlientaDaneDto
            {
                Imie = model.Imie,
                Nazwisko = model.Nazwisko,
                Email = model.Email,
                Telefon = model.Telefon,
                Ulica = model.Ulica,
                NumerDomu = model.NumerDomu,
                NumerLokalu = model.NumerLokalu,
                KodPocztowy = model.KodPocztowy,
                Miasto = model.Miasto
            });

            TempData["KomunikatKontaKlienta"] = "Dane konta zostały zapisane.";

            return RedirectToAction(nameof(Panel));
        }

        [Authorize]
        public async Task<IActionResult> Zamowienie(int id)
        {
            await PrzygotujDaneDoLayoutu();
            ViewBag.UkryjAktualnosci = true;

            var email = PobierzEmailZalogowanegoKlienta();

            var zamowienie = await _kontoKlientaService.PobierzSzczegolyZamowieniaKlienta(email, id);

            if (zamowienie == null)
            {
                return NotFound();
            }

            return View(zamowienie);
        }

        [AllowAnonymous]
        [HttpGet]
        public async Task<IActionResult> Logowanie(string? returnUrl = null)
        {
            await PrzygotujDaneDoLayoutu();
            ViewBag.UkryjAktualnosci = true;

            if (User.Identity?.IsAuthenticated == true)
            {
                return RedirectToAction(nameof(Panel));
            }

            return View(new KontoKlientaLogowanieViewModel
            {
                ReturnUrl = returnUrl
            });
        }

        [AllowAnonymous]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Logowanie(KontoKlientaLogowanieViewModel model)
        {
            await PrzygotujDaneDoLayoutu();
            ViewBag.UkryjAktualnosci = true;

            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var wynik = await _signInManager.PasswordSignInAsync(
                model.Email.Trim(),
                model.Haslo,
                model.ZapamietajMnie,
                lockoutOnFailure: false);

            if (wynik.Succeeded)
            {
                if (!string.IsNullOrWhiteSpace(model.ReturnUrl) && Url.IsLocalUrl(model.ReturnUrl))
                {
                    return LocalRedirect(model.ReturnUrl);
                }

                return RedirectToAction(nameof(Panel));
            }

            ModelState.AddModelError(string.Empty, "Nieprawidłowy e-mail lub hasło.");

            return View(model);
        }

        [AllowAnonymous]
        [HttpGet]
        public async Task<IActionResult> Rejestracja()
        {
            await PrzygotujDaneDoLayoutu();
            ViewBag.UkryjAktualnosci = true;

            if (User.Identity?.IsAuthenticated == true)
            {
                return RedirectToAction(nameof(Panel));
            }

            return View(new KontoKlientaRejestracjaViewModel());
        }

        [AllowAnonymous]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Rejestracja(KontoKlientaRejestracjaViewModel model)
        {
            await PrzygotujDaneDoLayoutu();
            ViewBag.UkryjAktualnosci = true;

            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var email = model.Email.Trim().ToLowerInvariant();

            var uzytkownik = new IdentityUser
            {
                UserName = email,
                Email = email,
                EmailConfirmed = true
            };

            var wynik = await _userManager.CreateAsync(uzytkownik, model.Haslo);

            if (!wynik.Succeeded)
            {
                foreach (var blad in wynik.Errors)
                {
                    ModelState.AddModelError(string.Empty, blad.Description);
                }

                return View(model);
            }

            await _kontoKlientaService.UtworzLubAktualizujKlienta(
                email,
                model.Imie,
                model.Nazwisko,
                model.Telefon);

            await _signInManager.SignInAsync(uzytkownik, isPersistent: false);

            return RedirectToAction(nameof(Panel));
        }

        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Wyloguj()
        {
            await _signInManager.SignOutAsync();

            return RedirectToAction("Index", "Home");
        }

        private string PobierzEmailZalogowanegoKlienta()
        {
            return User.Identity?.Name?.Trim().ToLowerInvariant() ?? string.Empty;
        }
    }
}
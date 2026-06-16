using System.Text.Json;
using Firma.Interfaces.CMS;
using Firma.Interfaces.Sklep;
using Firma.PortalWWW.Models;
using Firma.Services.Data.Dto.ZamowieniaPubliczne;
using Microsoft.AspNetCore.Mvc;

namespace Firma.PortalWWW.Controllers
{
    public class KoszykController : PortalControllerBase
    {
        private const string KoszykSessionKey = "PortalKoszyk";

        private readonly ITowarService _towarService;
        private readonly IZamowieniePubliczneService _zamowieniePubliczneService;

        public KoszykController(
            ITowarService towarService,
            IZamowieniePubliczneService zamowieniePubliczneService,
            IStronaService stronaService,
            IAktualnoscService aktualnoscService,
            IUstawieniePortaluService ustawieniePortaluService)
            : base(stronaService, aktualnoscService, ustawieniePortaluService)
        {
            _towarService = towarService;
            _zamowieniePubliczneService = zamowieniePubliczneService;
        }

        public async Task<IActionResult> Index()
        {
            await PrzygotujDaneDoLayoutu();
            ViewBag.UkryjAktualnosci = true;

            return View(PobierzKoszyk());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Dodaj(int idTowaru, int ilosc = 1, string? returnUrl = null)
        {
            var towar = await _towarService.GetTowar(idTowaru);

            if (towar == null)
            {
                TempData["KoszykKomunikat"] = "Nie znaleziono produktu.";
                return PrzekierujPoOperacji(returnUrl);
            }

            if (!towar.CzyDostepny || towar.IloscSztuk == null || towar.IloscSztuk <= 0)
            {
                TempData["KoszykKomunikat"] = "Produkt nie jest aktualnie dostępny.";
                return PrzekierujPoOperacji(returnUrl);
            }

            var koszyk = PobierzKoszyk();
            var pozycja = koszyk.Pozycje.FirstOrDefault(p => p.IdTowaru == idTowaru);

            var iloscDoDodania = Math.Clamp(ilosc, 1, 20);
            var maksymalnaIlosc = Math.Min(towar.IloscSztuk.Value, 20);

            if (pozycja == null)
            {
                koszyk.Pozycje.Add(new KoszykPozycjaViewModel
                {
                    IdTowaru = towar.IdTowaru,
                    Kod = towar.Kod,
                    Nazwa = towar.Nazwa,
                    FotoUrl = towar.FotoUrl,
                    Producent = towar.Producent,
                    Rodzaj = towar.Rodzaj,
                    Cena = towar.Cena,
                    Ilosc = Math.Min(iloscDoDodania, maksymalnaIlosc),
                    DostepnaIlosc = towar.IloscSztuk
                });
            }
            else
            {
                pozycja.Kod = towar.Kod;
                pozycja.Nazwa = towar.Nazwa;
                pozycja.FotoUrl = towar.FotoUrl;
                pozycja.Producent = towar.Producent;
                pozycja.Rodzaj = towar.Rodzaj;
                pozycja.Cena = towar.Cena;
                pozycja.DostepnaIlosc = towar.IloscSztuk;
                pozycja.Ilosc = Math.Min(pozycja.Ilosc + iloscDoDodania, maksymalnaIlosc);
            }

            ZapiszKoszyk(koszyk);

            TempData["KoszykKomunikat"] = "Produkt dodano do koszyka.";

            return PrzekierujPoOperacji(returnUrl);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult ZmienIlosc(int idTowaru, int ilosc)
        {
            var koszyk = PobierzKoszyk();
            var pozycja = koszyk.Pozycje.FirstOrDefault(p => p.IdTowaru == idTowaru);

            if (pozycja != null)
            {
                var maksymalnaIlosc = Math.Min(pozycja.DostepnaIlosc ?? 20, 20);
                pozycja.Ilosc = Math.Clamp(ilosc, 1, maksymalnaIlosc);

                ZapiszKoszyk(koszyk);
                TempData["KoszykKomunikat"] = "Zmieniono ilość produktu.";
            }

            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Usun(int idTowaru)
        {
            var koszyk = PobierzKoszyk();

            var pozycja = koszyk.Pozycje.FirstOrDefault(p => p.IdTowaru == idTowaru);

            if (pozycja != null)
            {
                koszyk.Pozycje.Remove(pozycja);
                ZapiszKoszyk(koszyk);
                TempData["KoszykKomunikat"] = "Usunięto produkt z koszyka.";
            }

            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Wyczysc()
        {
            HttpContext.Session.Remove(KoszykSessionKey);
            TempData["KoszykKomunikat"] = "Koszyk został wyczyszczony.";

            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public async Task<IActionResult> Zamowienie()
        {
            await PrzygotujDaneDoLayoutu();
            ViewBag.UkryjAktualnosci = true;

            var koszyk = PobierzKoszyk();

            if (!koszyk.Pozycje.Any())
            {
                TempData["KoszykKomunikat"] = "Koszyk jest pusty.";
                return RedirectToAction(nameof(Index));
            }

            ViewBag.Koszyk = koszyk;

            return View(new DaneZamowieniaViewModel());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Zamowienie(DaneZamowieniaViewModel model)
        {
            await PrzygotujDaneDoLayoutu();
            ViewBag.UkryjAktualnosci = true;

            var koszyk = PobierzKoszyk();

            if (!koszyk.Pozycje.Any())
            {
                TempData["KoszykKomunikat"] = "Koszyk jest pusty.";
                return RedirectToAction(nameof(Index));
            }

            ViewBag.Koszyk = koszyk;

            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var wynik = await _zamowieniePubliczneService.ZlozZamowienie(new DaneZamowieniaPublicznegoDto
            {
                Imie = model.Imie,
                Nazwisko = model.Nazwisko,
                Email = model.Email,
                Telefon = model.Telefon,
                Ulica = model.Ulica,
                NumerDomu = model.NumerDomu,
                NumerLokalu = model.NumerLokalu,
                KodPocztowy = model.KodPocztowy,
                Miasto = model.Miasto,
                Pozycje = koszyk.Pozycje
                    .Select(p => new PozycjaZamowieniaPublicznegoDto
                    {
                        IdTowaru = p.IdTowaru,
                        Ilosc = p.Ilosc
                    })
                    .ToList()
            });

            if (!wynik.CzySukces)
            {
                ModelState.AddModelError(string.Empty, wynik.KomunikatBledu);
                return View(model);
            }

            HttpContext.Session.Remove(KoszykSessionKey);

            ViewBag.EmailZamowienia = model.Email;

            return View("Potwierdzenie", wynik);
        }

        private KoszykViewModel PobierzKoszyk()
        {
            var json = HttpContext.Session.GetString(KoszykSessionKey);

            if (string.IsNullOrWhiteSpace(json))
            {
                return new KoszykViewModel();
            }

            return JsonSerializer.Deserialize<KoszykViewModel>(json) ?? new KoszykViewModel();
        }

        private void ZapiszKoszyk(KoszykViewModel koszyk)
        {
            var json = JsonSerializer.Serialize(koszyk);

            HttpContext.Session.SetString(KoszykSessionKey, json);
        }

        private IActionResult PrzekierujPoOperacji(string? returnUrl)
        {
            if (!string.IsNullOrWhiteSpace(returnUrl) && Url.IsLocalUrl(returnUrl))
            {
                return LocalRedirect(returnUrl);
            }

            return RedirectToAction(nameof(Index));
        }
    }
}
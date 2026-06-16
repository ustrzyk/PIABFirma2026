using System.Security.Claims;
using Firma.Intranet.Interfaces.Intranet;
using Firma.Intranet.Models;
using Firma.Intranet.Services.Data.Intranet;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Firma.Intranet.Controllers
{
    [Authorize(Roles = "Administrator")]
    public class UzytkownikController : Controller
    {
        private readonly IUzytkownikIntranetService _uzytkownikIntranetService;

        public UzytkownikController(IUzytkownikIntranetService uzytkownikIntranetService)
        {
            _uzytkownikIntranetService = uzytkownikIntranetService;
        }

        public async Task<IActionResult> Index()
        {
            var uzytkownicy = await _uzytkownikIntranetService.PobierzListe(
                PobierzIdAktualnegoUzytkownika());

            var model = uzytkownicy
                .Select(u => new UzytkownikListaItemViewModel
                {
                    Id = u.Id,
                    Email = u.Email,
                    NazwaUzytkownika = u.NazwaUzytkownika,
                    Role = u.Role,
                    CzyAktualnieZalogowany = u.CzyAktualnieZalogowany
                })
                .ToList();

            return View(model);
        }

        public async Task<IActionResult> Create()
        {
            var model = new UzytkownikCreateViewModel
            {
                DostepneRole = await _uzytkownikIntranetService.PobierzRole()
            };

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(UzytkownikCreateViewModel model)
        {
            model.DostepneRole = await _uzytkownikIntranetService.PobierzRole();

            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var wynik = await _uzytkownikIntranetService.Dodaj(
                model.Email,
                model.Haslo,
                model.Rola);

            if (!wynik.CzySukces)
            {
                DodajBledy(wynik);

                return View(model);
            }

            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Edit(string id)
        {
            var uzytkownik = await _uzytkownikIntranetService.PobierzDoEdycji(
                id,
                PobierzIdAktualnegoUzytkownika());

            if (uzytkownik == null)
            {
                return NotFound();
            }

            var model = new UzytkownikEditViewModel
            {
                Id = uzytkownik.Id,
                Email = uzytkownik.Email,
                Rola = uzytkownik.Rola,
                CzyAktualnieZalogowany = uzytkownik.CzyAktualnieZalogowany,
                DostepneRole = uzytkownik.DostepneRole
            };

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, UzytkownikEditViewModel model)
        {
            if (id != model.Id)
            {
                return NotFound();
            }

            model.DostepneRole = await _uzytkownikIntranetService.PobierzRole();
            model.CzyAktualnieZalogowany = model.Id == PobierzIdAktualnegoUzytkownika();

            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var wynik = await _uzytkownikIntranetService.Aktualizuj(
                id,
                model.Email,
                model.Rola,
                PobierzIdAktualnegoUzytkownika());

            if (!wynik.CzyZnaleziono)
            {
                return NotFound();
            }

            if (!wynik.CzySukces)
            {
                DodajBledy(wynik);

                return View(model);
            }

            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> ResetHasla(string id)
        {
            var email = await _uzytkownikIntranetService.PobierzEmail(id);

            if (email == null)
            {
                return NotFound();
            }

            var model = new ResetHaslaUzytkownikaViewModel
            {
                Id = id,
                Email = email
            };

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ResetHasla(string id, ResetHaslaUzytkownikaViewModel model)
        {
            if (id != model.Id)
            {
                return NotFound();
            }

            var email = await _uzytkownikIntranetService.PobierzEmail(id);

            if (email == null)
            {
                return NotFound();
            }

            model.Email = email;

            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var wynik = await _uzytkownikIntranetService.ResetujHaslo(
                id,
                model.NoweHaslo);

            if (!wynik.CzyZnaleziono)
            {
                return NotFound();
            }

            if (!wynik.CzySukces)
            {
                DodajBledy(wynik);

                return View(model);
            }

            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Delete(string id)
        {
            var uzytkownik = await _uzytkownikIntranetService.PobierzDoUsuniecia(
                id,
                PobierzIdAktualnegoUzytkownika());

            if (uzytkownik == null)
            {
                return NotFound();
            }

            var model = MapujDoModeluUsuniecia(uzytkownik);

            return View(model);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            var wynik = await _uzytkownikIntranetService.Usun(
                id,
                PobierzIdAktualnegoUzytkownika());

            if (!wynik.CzyZnaleziono)
            {
                return NotFound();
            }

            if (!wynik.CzySukces)
            {
                DodajBledy(wynik);

                var uzytkownik = await _uzytkownikIntranetService.PobierzDoUsuniecia(
                    id,
                    PobierzIdAktualnegoUzytkownika());

                if (uzytkownik == null)
                {
                    return NotFound();
                }

                return View(MapujDoModeluUsuniecia(uzytkownik));
            }

            return RedirectToAction(nameof(Index));
        }

        private string? PobierzIdAktualnegoUzytkownika()
        {
            return User.FindFirstValue(ClaimTypes.NameIdentifier);
        }

        private static UsunUzytkownikaViewModel MapujDoModeluUsuniecia(UzytkownikUsuniecieDto uzytkownik)
        {
            return new UsunUzytkownikaViewModel
            {
                Id = uzytkownik.Id,
                Email = uzytkownik.Email,
                Role = uzytkownik.Role,
                CzyAktualnieZalogowany = uzytkownik.CzyAktualnieZalogowany
            };
        }

        private void DodajBledy(OperacjaUzytkownikaWynikDto wynik)
        {
            foreach (var blad in wynik.Bledy)
            {
                ModelState.AddModelError(string.Empty, blad);
            }
        }
    }
}
using Firma.Data.Data.CMS;
using Firma.Intranet.Interfaces.Intranet;
using Microsoft.AspNetCore.Mvc;

namespace Firma.Intranet.Controllers
{
    public class UstawieniePortaluController : Controller
    {
        private readonly IUstawieniePortaluIntranetService _ustawieniePortaluIntranetService;

        public UstawieniePortaluController(IUstawieniePortaluIntranetService ustawieniePortaluIntranetService)
        {
            _ustawieniePortaluIntranetService = ustawieniePortaluIntranetService;
        }

        public async Task<IActionResult> Index()
        {
            var ustawieniaPortalu = await _ustawieniePortaluIntranetService.PobierzListe();

            return View(ustawieniaPortalu);
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var ustawieniePortalu = await _ustawieniePortaluIntranetService.PobierzSzczegoly(id.Value);

            if (ustawieniePortalu == null)
            {
                return NotFound();
            }

            return View(ustawieniePortalu);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("IdUstawieniaPortalu,Klucz,Wartosc,Opis,CzyAktywny")] UstawieniePortalu ustawieniePortalu)
        {
            if (ModelState.IsValid && await _ustawieniePortaluIntranetService.CzyKluczIstnieje(ustawieniePortalu.Klucz))
            {
                ModelState.AddModelError(
                    nameof(UstawieniePortalu.Klucz),
                    "Ustawienie z takim kluczem już istnieje.");
            }

            if (ModelState.IsValid)
            {
                await _ustawieniePortaluIntranetService.Dodaj(ustawieniePortalu);

                return RedirectToAction(nameof(Index));
            }

            return View(ustawieniePortalu);
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var ustawieniePortalu = await _ustawieniePortaluIntranetService.PobierzDoEdycji(id.Value);

            if (ustawieniePortalu == null)
            {
                return NotFound();
            }

            return View(ustawieniePortalu);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("IdUstawieniaPortalu,Klucz,Wartosc,Opis,CzyAktywny")] UstawieniePortalu ustawieniePortalu)
        {
            if (id != ustawieniePortalu.IdUstawieniaPortalu)
            {
                return NotFound();
            }

            if (ModelState.IsValid && await _ustawieniePortaluIntranetService.CzyKluczIstnieje(
                    ustawieniePortalu.Klucz,
                    ustawieniePortalu.IdUstawieniaPortalu))
            {
                ModelState.AddModelError(
                    nameof(UstawieniePortalu.Klucz),
                    "Ustawienie z takim kluczem już istnieje.");
            }

            if (ModelState.IsValid)
            {
                var zapisano = await _ustawieniePortaluIntranetService.Aktualizuj(id, ustawieniePortalu);

                if (!zapisano)
                {
                    return NotFound();
                }

                return RedirectToAction(nameof(Index));
            }

            return View(ustawieniePortalu);
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var ustawieniePortalu = await _ustawieniePortaluIntranetService.PobierzDoUsuniecia(id.Value);

            if (ustawieniePortalu == null)
            {
                return NotFound();
            }

            return View(ustawieniePortalu);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await _ustawieniePortaluIntranetService.Usun(id);

            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Aktywuj(int id)
        {
            await _ustawieniePortaluIntranetService.Aktywuj(id);

            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Dezaktywuj(int id)
        {
            await _ustawieniePortaluIntranetService.Dezaktywuj(id);

            return RedirectToAction(nameof(Index));
        }
    }
}
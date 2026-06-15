using Firma.Data.Data.CMS;
using Firma.Intranet.Interfaces.Intranet;
using Microsoft.AspNetCore.Mvc;

namespace Firma.Intranet.Controllers
{
    public class AktualnoscController : Controller
    {
        private readonly IAktualnoscIntranetService _aktualnoscIntranetService;

        public AktualnoscController(IAktualnoscIntranetService aktualnoscIntranetService)
        {
            _aktualnoscIntranetService = aktualnoscIntranetService;
        }

        public async Task<IActionResult> Index()
        {
            var aktualnosci = await _aktualnoscIntranetService.PobierzListe();

            return View(aktualnosci);
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var aktualnosc = await _aktualnoscIntranetService.PobierzSzczegoly(id.Value);

            if (aktualnosc == null)
            {
                return NotFound();
            }

            return View(aktualnosc);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("IdAktualnosci,LinkTytul,Tytul,Tresc,Pozycja,CzyAktywny")] Aktualnosc aktualnosc)
        {
            if (ModelState.IsValid)
            {
                await _aktualnoscIntranetService.Dodaj(aktualnosc);

                return RedirectToAction(nameof(Index));
            }

            return View(aktualnosc);
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var aktualnosc = await _aktualnoscIntranetService.PobierzDoEdycji(id.Value);

            if (aktualnosc == null)
            {
                return NotFound();
            }

            return View(aktualnosc);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("IdAktualnosci,LinkTytul,Tytul,Tresc,Pozycja,CzyAktywny")] Aktualnosc aktualnosc)
        {
            if (id != aktualnosc.IdAktualnosci)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                var zapisano = await _aktualnoscIntranetService.Aktualizuj(id, aktualnosc);

                if (!zapisano)
                {
                    return NotFound();
                }

                return RedirectToAction(nameof(Index));
            }

            return View(aktualnosc);
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var aktualnosc = await _aktualnoscIntranetService.PobierzDoUsuniecia(id.Value);

            if (aktualnosc == null)
            {
                return NotFound();
            }

            return View(aktualnosc);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await _aktualnoscIntranetService.Usun(id);

            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Aktywuj(int id)
        {
            await _aktualnoscIntranetService.Aktywuj(id);

            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Dezaktywuj(int id)
        {
            await _aktualnoscIntranetService.Dezaktywuj(id);

            return RedirectToAction(nameof(Index));
        }
    }
}
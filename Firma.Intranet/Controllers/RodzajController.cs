using Firma.Data.Data.Sklep;
using Firma.Intranet.Interfaces.Intranet;
using Microsoft.AspNetCore.Mvc;

namespace Firma.Intranet.Controllers
{
    public class RodzajController : Controller
    {
        private readonly IRodzajIntranetService _rodzajIntranetService;

        public RodzajController(IRodzajIntranetService rodzajIntranetService)
        {
            _rodzajIntranetService = rodzajIntranetService;
        }

        public async Task<IActionResult> Index()
        {
            var rodzaje = await _rodzajIntranetService.PobierzListe();

            return View(rodzaje);
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var rodzaj = await _rodzajIntranetService.PobierzSzczegoly(id.Value);

            if (rodzaj == null)
            {
                return NotFound();
            }

            return View(rodzaj);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("IdRodzaju,Nazwa,Opis,CzyAktywny")] Rodzaj rodzaj)
        {
            if (ModelState.IsValid)
            {
                await _rodzajIntranetService.Dodaj(rodzaj);

                return RedirectToAction(nameof(Index));
            }

            return View(rodzaj);
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var rodzaj = await _rodzajIntranetService.PobierzDoEdycji(id.Value);

            if (rodzaj == null)
            {
                return NotFound();
            }

            return View(rodzaj);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("IdRodzaju,Nazwa,Opis,CzyAktywny")] Rodzaj rodzaj)
        {
            if (id != rodzaj.IdRodzaju)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                var zapisano = await _rodzajIntranetService.Aktualizuj(id, rodzaj);

                if (!zapisano)
                {
                    return NotFound();
                }

                return RedirectToAction(nameof(Index));
            }

            return View(rodzaj);
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var rodzaj = await _rodzajIntranetService.PobierzDoUsuniecia(id.Value);

            if (rodzaj == null)
            {
                return NotFound();
            }

            return View(rodzaj);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await _rodzajIntranetService.UsunAlboDezaktywuj(id);

            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Aktywuj(int id)
        {
            await _rodzajIntranetService.Aktywuj(id);

            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Dezaktywuj(int id)
        {
            await _rodzajIntranetService.Dezaktywuj(id);

            return RedirectToAction(nameof(Index));
        }
    }
}
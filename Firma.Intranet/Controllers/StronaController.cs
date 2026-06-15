using Firma.Data.Data.CMS;
using Firma.Intranet.Interfaces.Intranet;
using Microsoft.AspNetCore.Mvc;

namespace Firma.Intranet.Controllers
{
    public class StronaController : Controller
    {
        private readonly IStronaIntranetService _stronaIntranetService;

        public StronaController(IStronaIntranetService stronaIntranetService)
        {
            _stronaIntranetService = stronaIntranetService;
        }

        public async Task<IActionResult> Index()
        {
            var strony = await _stronaIntranetService.PobierzListe();

            return View(strony);
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var strona = await _stronaIntranetService.PobierzSzczegoly(id.Value);

            if (strona == null)
            {
                return NotFound();
            }

            return View(strona);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("IdStrony,LinkTytul,Tytul,Tresc,Pozycja,CzyAktywny")] Strona strona)
        {
            if (ModelState.IsValid)
            {
                await _stronaIntranetService.Dodaj(strona);

                return RedirectToAction(nameof(Index));
            }

            return View(strona);
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var strona = await _stronaIntranetService.PobierzDoEdycji(id.Value);

            if (strona == null)
            {
                return NotFound();
            }

            return View(strona);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("IdStrony,LinkTytul,Tytul,Tresc,Pozycja,CzyAktywny")] Strona strona)
        {
            if (id != strona.IdStrony)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                var zapisano = await _stronaIntranetService.Aktualizuj(id, strona);

                if (!zapisano)
                {
                    return NotFound();
                }

                return RedirectToAction(nameof(Index));
            }

            return View(strona);
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var strona = await _stronaIntranetService.PobierzDoUsuniecia(id.Value);

            if (strona == null)
            {
                return NotFound();
            }

            return View(strona);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await _stronaIntranetService.Usun(id);

            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Aktywuj(int id)
        {
            await _stronaIntranetService.Aktywuj(id);

            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Dezaktywuj(int id)
        {
            await _stronaIntranetService.Dezaktywuj(id);

            return RedirectToAction(nameof(Index));
        }
    }
}
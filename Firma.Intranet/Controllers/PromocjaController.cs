using Firma.Data.Data.CMS;
using Firma.Intranet.Interfaces.Intranet;
using Microsoft.AspNetCore.Mvc;

namespace Firma.Intranet.Controllers
{
    public class PromocjaController : Controller
    {
        private readonly IPromocjaIntranetService _promocjaIntranetService;

        public PromocjaController(IPromocjaIntranetService promocjaIntranetService)
        {
            _promocjaIntranetService = promocjaIntranetService;
        }

        public async Task<IActionResult> Index()
        {
            var promocje = await _promocjaIntranetService.PobierzListe();

            return View(promocje);
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var promocja = await _promocjaIntranetService.PobierzSzczegoly(id.Value);

            if (promocja == null)
            {
                return NotFound();
            }

            return View(promocja);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("IdPromocji,Tytul,Opis,RabatProcentowy,DataOd,DataDo,CzyAktywny")] Promocja promocja)
        {
            WalidujDaty(promocja);

            if (ModelState.IsValid)
            {
                await _promocjaIntranetService.Dodaj(promocja);

                return RedirectToAction(nameof(Index));
            }

            return View(promocja);
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var promocja = await _promocjaIntranetService.PobierzDoEdycji(id.Value);

            if (promocja == null)
            {
                return NotFound();
            }

            return View(promocja);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("IdPromocji,Tytul,Opis,RabatProcentowy,DataOd,DataDo,CzyAktywny")] Promocja promocja)
        {
            if (id != promocja.IdPromocji)
            {
                return NotFound();
            }

            WalidujDaty(promocja);

            if (ModelState.IsValid)
            {
                var zapisano = await _promocjaIntranetService.Aktualizuj(id, promocja);

                if (!zapisano)
                {
                    return NotFound();
                }

                return RedirectToAction(nameof(Index));
            }

            return View(promocja);
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var promocja = await _promocjaIntranetService.PobierzDoUsuniecia(id.Value);

            if (promocja == null)
            {
                return NotFound();
            }

            return View(promocja);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await _promocjaIntranetService.Usun(id);

            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Aktywuj(int id)
        {
            await _promocjaIntranetService.Aktywuj(id);

            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Dezaktywuj(int id)
        {
            await _promocjaIntranetService.Dezaktywuj(id);

            return RedirectToAction(nameof(Index));
        }

        private void WalidujDaty(Promocja promocja)
        {
            if (promocja.DataOd.HasValue
                && promocja.DataDo.HasValue
                && promocja.DataOd.Value.Date > promocja.DataDo.Value.Date)
            {
                ModelState.AddModelError(
                    nameof(Promocja.DataDo),
                    "Data zakończenia promocji nie może być wcześniejsza niż data rozpoczęcia.");
            }
        }
    }
}
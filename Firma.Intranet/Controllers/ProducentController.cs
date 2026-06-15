using Firma.Data.Data.Sklep;
using Firma.Intranet.Interfaces.Intranet;
using Microsoft.AspNetCore.Mvc;

namespace Firma.Intranet.Controllers
{
    public class ProducentController : Controller
    {
        private readonly IProducentIntranetService _producentIntranetService;

        public ProducentController(IProducentIntranetService producentIntranetService)
        {
            _producentIntranetService = producentIntranetService;
        }

        public async Task<IActionResult> Index()
        {
            var producenci = await _producentIntranetService.PobierzListe();

            return View(producenci);
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var producent = await _producentIntranetService.PobierzSzczegoly(id.Value);

            if (producent == null)
            {
                return NotFound();
            }

            return View(producent);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("IdProducenta,Nazwa,Kraj,StronaWWW,Opis,CzyAktywny")] Producent producent)
        {
            if (ModelState.IsValid)
            {
                await _producentIntranetService.Dodaj(producent);

                return RedirectToAction(nameof(Index));
            }

            return View(producent);
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var producent = await _producentIntranetService.PobierzDoEdycji(id.Value);

            if (producent == null)
            {
                return NotFound();
            }

            return View(producent);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("IdProducenta,Nazwa,Kraj,StronaWWW,Opis,CzyAktywny")] Producent producent)
        {
            if (id != producent.IdProducenta)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                var zapisano = await _producentIntranetService.Aktualizuj(id, producent);

                if (!zapisano)
                {
                    return NotFound();
                }

                return RedirectToAction(nameof(Index));
            }

            return View(producent);
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var producent = await _producentIntranetService.PobierzDoUsuniecia(id.Value);

            if (producent == null)
            {
                return NotFound();
            }

            return View(producent);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await _producentIntranetService.UsunAlboDezaktywuj(id);

            return RedirectToAction(nameof(Index));
        }
    }
}
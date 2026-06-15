using Firma.Data.Data.Sklep;
using Firma.Intranet.Interfaces.Intranet;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Firma.Intranet.Controllers
{
    public class StanMagazynowyController : Controller
    {
        private readonly IStanMagazynowyIntranetService _stanMagazynowyIntranetService;

        public StanMagazynowyController(IStanMagazynowyIntranetService stanMagazynowyIntranetService)
        {
            _stanMagazynowyIntranetService = stanMagazynowyIntranetService;
        }

        public async Task<IActionResult> Index()
        {
            var stanyMagazynowe = await _stanMagazynowyIntranetService.PobierzListe();

            return View(stanyMagazynowe);
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var stanMagazynowy = await _stanMagazynowyIntranetService.PobierzSzczegoly(id.Value);

            if (stanMagazynowy == null)
            {
                return NotFound();
            }

            return View(stanMagazynowy);
        }

        public async Task<IActionResult> Create()
        {
            await PrzygotujTowary();

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("IdStanuMagazynowego,IloscSztuk,MinimalnaIlosc,Lokalizacja,CzyAktywny,IdTowaru")] StanMagazynowy stanMagazynowy)
        {
            if (ModelState.IsValid && await _stanMagazynowyIntranetService.CzyTowarMaStanMagazynowy(stanMagazynowy.IdTowaru))
            {
                ModelState.AddModelError(
                    nameof(StanMagazynowy.IdTowaru),
                    "Wybrany towar ma już przypisany stan magazynowy.");
            }

            if (ModelState.IsValid)
            {
                await _stanMagazynowyIntranetService.Dodaj(stanMagazynowy);

                return RedirectToAction(nameof(Index));
            }

            await PrzygotujTowary(stanMagazynowy.IdTowaru);

            return View(stanMagazynowy);
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var stanMagazynowy = await _stanMagazynowyIntranetService.PobierzDoEdycji(id.Value);

            if (stanMagazynowy == null)
            {
                return NotFound();
            }

            await PrzygotujTowary(stanMagazynowy.IdTowaru);

            return View(stanMagazynowy);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("IdStanuMagazynowego,IloscSztuk,MinimalnaIlosc,Lokalizacja,CzyAktywny,IdTowaru")] StanMagazynowy stanMagazynowy)
        {
            if (id != stanMagazynowy.IdStanuMagazynowego)
            {
                return NotFound();
            }

            if (ModelState.IsValid && await _stanMagazynowyIntranetService.CzyTowarMaStanMagazynowy(stanMagazynowy.IdTowaru, stanMagazynowy.IdStanuMagazynowego))
            {
                ModelState.AddModelError(
                    nameof(StanMagazynowy.IdTowaru),
                    "Wybrany towar ma już przypisany stan magazynowy.");
            }

            if (ModelState.IsValid)
            {
                var zapisano = await _stanMagazynowyIntranetService.Aktualizuj(id, stanMagazynowy);

                if (!zapisano)
                {
                    return NotFound();
                }

                return RedirectToAction(nameof(Index));
            }

            await PrzygotujTowary(stanMagazynowy.IdTowaru);

            return View(stanMagazynowy);
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var stanMagazynowy = await _stanMagazynowyIntranetService.PobierzDoUsuniecia(id.Value);

            if (stanMagazynowy == null)
            {
                return NotFound();
            }

            return View(stanMagazynowy);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await _stanMagazynowyIntranetService.Usun(id);

            return RedirectToAction(nameof(Index));
        }

        private async Task PrzygotujTowary(int? idTowaru = null)
        {
            var towary = await _stanMagazynowyIntranetService.PobierzTowaryDoSelectList();

            ViewData["IdTowaru"] = new SelectList(
                towary,
                "IdTowaru",
                "Nazwa",
                idTowaru);
        }
    }
}
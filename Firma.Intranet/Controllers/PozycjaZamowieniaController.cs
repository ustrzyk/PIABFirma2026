using Firma.Data.Data.Sklep;
using Firma.Intranet.Interfaces.Intranet;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Firma.Intranet.Controllers
{
    public class PozycjaZamowieniaController : Controller
    {
        private readonly IPozycjaZamowieniaIntranetService _pozycjaZamowieniaIntranetService;

        public PozycjaZamowieniaController(IPozycjaZamowieniaIntranetService pozycjaZamowieniaIntranetService)
        {
            _pozycjaZamowieniaIntranetService = pozycjaZamowieniaIntranetService;
        }

        public async Task<IActionResult> Index()
        {
            var pozycjeZamowien = await _pozycjaZamowieniaIntranetService.PobierzListe();

            return View(pozycjeZamowien);
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var pozycjaZamowienia = await _pozycjaZamowieniaIntranetService.PobierzSzczegoly(id.Value);

            if (pozycjaZamowienia == null)
            {
                return NotFound();
            }

            return View(pozycjaZamowienia);
        }

        public async Task<IActionResult> Create()
        {
            await PrzygotujListy();

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("IdPozycjiZamowienia,Ilosc,CenaJednostkowa,IdZamowienia,IdTowaru")] PozycjaZamowienia pozycjaZamowienia)
        {
            if (ModelState.IsValid)
            {
                await _pozycjaZamowieniaIntranetService.Dodaj(pozycjaZamowienia);

                return RedirectToAction(nameof(Index));
            }

            await PrzygotujListy(
                pozycjaZamowienia.IdZamowienia,
                pozycjaZamowienia.IdTowaru);

            return View(pozycjaZamowienia);
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var pozycjaZamowienia = await _pozycjaZamowieniaIntranetService.PobierzDoEdycji(id.Value);

            if (pozycjaZamowienia == null)
            {
                return NotFound();
            }

            await PrzygotujListy(
                pozycjaZamowienia.IdZamowienia,
                pozycjaZamowienia.IdTowaru);

            return View(pozycjaZamowienia);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("IdPozycjiZamowienia,Ilosc,CenaJednostkowa,IdZamowienia,IdTowaru")] PozycjaZamowienia pozycjaZamowienia)
        {
            if (id != pozycjaZamowienia.IdPozycjiZamowienia)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                var zapisano = await _pozycjaZamowieniaIntranetService.Aktualizuj(id, pozycjaZamowienia);

                if (!zapisano)
                {
                    return NotFound();
                }

                return RedirectToAction(nameof(Index));
            }

            await PrzygotujListy(
                pozycjaZamowienia.IdZamowienia,
                pozycjaZamowienia.IdTowaru);

            return View(pozycjaZamowienia);
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var pozycjaZamowienia = await _pozycjaZamowieniaIntranetService.PobierzDoUsuniecia(id.Value);

            if (pozycjaZamowienia == null)
            {
                return NotFound();
            }

            return View(pozycjaZamowienia);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await _pozycjaZamowieniaIntranetService.Usun(id);

            return RedirectToAction(nameof(Index));
        }

        private async Task PrzygotujListy(int? idZamowienia = null, int? idTowaru = null)
        {
            var zamowienia = await _pozycjaZamowieniaIntranetService.PobierzZamowieniaDoSelectList();
            var towary = await _pozycjaZamowieniaIntranetService.PobierzTowaryDoSelectList();

            ViewData["IdZamowienia"] = new SelectList(
                zamowienia,
                "IdZamowienia",
                "NumerZamowienia",
                idZamowienia);

            ViewData["IdTowaru"] = new SelectList(
                towary,
                "IdTowaru",
                "Nazwa",
                idTowaru);
        }
    }
}
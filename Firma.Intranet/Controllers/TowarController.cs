using Firma.Data.Data.Sklep;
using Firma.Intranet.Interfaces.Intranet;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Firma.Intranet.Controllers
{
    public class TowarController : Controller
    {
        private readonly ITowarIntranetService _towarIntranetService;
        private readonly IWebHostEnvironment _environment;

        public TowarController(
            ITowarIntranetService towarIntranetService,
            IWebHostEnvironment environment)
        {
            _towarIntranetService = towarIntranetService;
            _environment = environment;
        }

        public async Task<IActionResult> Index()
        {
            var towary = await _towarIntranetService.PobierzListe();

            return View(towary);
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var towar = await _towarIntranetService.PobierzSzczegoly(id.Value);

            if (towar == null)
            {
                return NotFound();
            }

            return View(towar);
        }

        public async Task<IActionResult> Create()
        {
            await PrzygotujListy();

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("IdTowaru,Kod,Nazwa,Cena,FotoUrl,Opis,CzyAktywny,IdRodzaju,IdProducenta")] Towar towar)
        {
            if (ModelState.IsValid)
            {
                await _towarIntranetService.Dodaj(towar);

                return RedirectToAction(nameof(Index));
            }

            await PrzygotujListy(towar.IdRodzaju, towar.IdProducenta);

            return View(towar);
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var towar = await _towarIntranetService.PobierzDoEdycji(id.Value);

            if (towar == null)
            {
                return NotFound();
            }

            await PrzygotujListy(towar.IdRodzaju, towar.IdProducenta);

            return View(towar);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("IdTowaru,Kod,Nazwa,Cena,FotoUrl,Opis,CzyAktywny,IdRodzaju,IdProducenta")] Towar towar)
        {
            if (id != towar.IdTowaru)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                var zapisano = await _towarIntranetService.Aktualizuj(id, towar);

                if (!zapisano)
                {
                    return NotFound();
                }

                return RedirectToAction(nameof(Index));
            }

            await PrzygotujListy(towar.IdRodzaju, towar.IdProducenta);

            return View(towar);
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var towar = await _towarIntranetService.PobierzDoUsuniecia(id.Value);

            if (towar == null)
            {
                return NotFound();
            }

            return View(towar);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await _towarIntranetService.Usun(id, PobierzFolderUploadu());

            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UsunZaznaczone(int[] ids)
        {
            if (ids == null || ids.Length == 0)
            {
                return RedirectToAction(nameof(Index));
            }

            await _towarIntranetService.UsunZaznaczone(ids, PobierzFolderUploadu());

            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Dezaktywuj(int id)
        {
            await _towarIntranetService.Dezaktywuj(id);

            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Aktywuj(int id)
        {
            await _towarIntranetService.Aktywuj(id);

            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DezaktywujZaznaczone(int[] ids)
        {
            if (ids == null || ids.Length == 0)
            {
                return RedirectToAction(nameof(Index));
            }

            await _towarIntranetService.DezaktywujZaznaczone(ids);

            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AktywujZaznaczone(int[] ids)
        {
            if (ids == null || ids.Length == 0)
            {
                return RedirectToAction(nameof(Index));
            }

            await _towarIntranetService.AktywujZaznaczone(ids);

            return RedirectToAction(nameof(Index));
        }

        private async Task PrzygotujListy(int? idRodzaju = null, int? idProducenta = null)
        {
            var producenci = await _towarIntranetService.PobierzProducentowDoSelectList(idProducenta);
            var rodzaje = await _towarIntranetService.PobierzRodzajeDoSelectList(idRodzaju);

            ViewData["IdProducenta"] = new SelectList(
                producenci,
                "IdProducenta",
                "Nazwa",
                idProducenta);

            ViewData["IdRodzaju"] = new SelectList(
                rodzaje,
                "IdRodzaju",
                "Nazwa",
                idRodzaju);
        }

        private string PobierzFolderUploadu()
        {
            return Path.GetFullPath(Path.Combine(
                _environment.ContentRootPath,
                "..",
                "Firma.PortalWWW",
                "wwwroot",
                "uploads",
                "towary"));
        }
    }
}
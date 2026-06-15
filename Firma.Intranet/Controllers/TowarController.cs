using Firma.Data.Data;
using Firma.Data.Data.Sklep;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace Firma.Intranet.Controllers
{
    public class TowarController : Controller
    {
        private readonly FirmaContext _context;
        private readonly IWebHostEnvironment _environment;

        public TowarController(FirmaContext context, IWebHostEnvironment environment)
        {
            _context = context;
            _environment = environment;
        }

        public async Task<IActionResult> Index()
        {
            // Pobieram towary do listy
            var firmaContext = _context.Towar
                .Include(t => t.Producent)
                .Include(t => t.Rodzaj)
                .OrderBy(t => t.Nazwa);

            return View(await firmaContext.ToListAsync());
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            // Pobieram towar do szczegółów
            var towar = await _context.Towar
                .Include(t => t.Producent)
                .Include(t => t.Rodzaj)
                .Include(t => t.ZalacznikiTowaru)
                .FirstOrDefaultAsync(m => m.IdTowaru == id);

            if (towar == null)
            {
                return NotFound();
            }

            return View(towar);
        }

        public IActionResult Create()
        {
            PrzygotujListy();

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("IdTowaru,Kod,Nazwa,Cena,FotoUrl,Opis,CzyAktywny,IdRodzaju,IdProducenta")] Towar towar)
        {
            if (ModelState.IsValid)
            {
                // Zaokrąglam cenę
                towar.Cena = decimal.Round(towar.Cena, 2, MidpointRounding.AwayFromZero);

                _context.Add(towar);
                await _context.SaveChangesAsync();

                return RedirectToAction(nameof(Index));
            }

            PrzygotujListy(towar.IdRodzaju, towar.IdProducenta);

            return View(towar);
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var towar = await _context.Towar.FindAsync(id);

            if (towar == null)
            {
                return NotFound();
            }

            PrzygotujListy(towar.IdRodzaju, towar.IdProducenta);

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
                try
                {
                    // Zaokrąglam cenę
                    towar.Cena = decimal.Round(towar.Cena, 2, MidpointRounding.AwayFromZero);

                    _context.Update(towar);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TowarExists(towar.IdTowaru))
                    {
                        return NotFound();
                    }

                    throw;
                }

                return RedirectToAction(nameof(Index));
            }

            PrzygotujListy(towar.IdRodzaju, towar.IdProducenta);

            return View(towar);
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var towar = await _context.Towar
                .Include(t => t.Producent)
                .Include(t => t.Rodzaj)
                .FirstOrDefaultAsync(m => m.IdTowaru == id);

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
            var towar = await _context.Towar
                .Include(t => t.StanMagazynowy)
                .Include(t => t.ZalacznikiTowaru)
                .Include(t => t.PozycjaZamowienia)
                .FirstOrDefaultAsync(t => t.IdTowaru == id);

            if (towar != null)
            {
                // Usuwam albo dezaktywuję towar
                UsunTowarAlboDezaktywuj(towar);
                await _context.SaveChangesAsync();
            }

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

            var towary = await _context.Towar
                .Include(t => t.StanMagazynowy)
                .Include(t => t.ZalacznikiTowaru)
                .Include(t => t.PozycjaZamowienia)
                .Where(t => ids.Contains(t.IdTowaru))
                .ToListAsync();

            foreach (var towar in towary)
            {
                // Usuwam albo dezaktywuję zaznaczony towar
                UsunTowarAlboDezaktywuj(towar);
            }

            await _context.SaveChangesAsync();

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

            var towary = await _context.Towar
                .Where(t => ids.Contains(t.IdTowaru))
                .ToListAsync();

            foreach (var towar in towary)
            {
                // Dezaktywuję zaznaczony towar
                towar.CzyAktywny = false;
            }

            await _context.SaveChangesAsync();

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

            var towary = await _context.Towar
                .Where(t => ids.Contains(t.IdTowaru))
                .ToListAsync();

            foreach (var towar in towary)
            {
                // Aktywuję zaznaczony towar
                towar.CzyAktywny = true;
            }

            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        private void UsunTowarAlboDezaktywuj(Towar towar)
        {
            if (towar.PozycjaZamowienia != null && towar.PozycjaZamowienia.Any())
            {
                // Nie usuwam towaru użytego w zamówieniach
                towar.CzyAktywny = false;
                _context.Update(towar);

                return;
            }

            if (towar.ZalacznikiTowaru != null && towar.ZalacznikiTowaru.Any())
            {
                foreach (var zalacznik in towar.ZalacznikiTowaru)
                {
                    // Usuwam plik załącznika
                    UsunPlik(zalacznik.Sciezka);
                }

                _context.ZalacznikTowaru.RemoveRange(towar.ZalacznikiTowaru);
            }

            if (towar.StanMagazynowy != null)
            {
                // Usuwam stan magazynowy towaru
                _context.StanMagazynowy.Remove(towar.StanMagazynowy);
            }

            _context.Towar.Remove(towar);
        }

        private void PrzygotujListy(int? idRodzaju = null, int? idProducenta = null)
        {
            ViewData["IdProducenta"] = new SelectList(
                _context.Producent.OrderBy(p => p.Nazwa),
                "IdProducenta",
                "Nazwa",
                idProducenta);

            ViewData["IdRodzaju"] = new SelectList(
                _context.Rodzaj.OrderBy(r => r.Nazwa),
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

        private string PobierzSciezkeFizyczna(string sciezka)
        {
            var nazwaPliku = sciezka
                .Split('/', StringSplitOptions.RemoveEmptyEntries)
                .LastOrDefault() ?? "";

            return Path.Combine(PobierzFolderUploadu(), nazwaPliku);
        }

        private void UsunPlik(string sciezka)
        {
            var sciezkaFizyczna = PobierzSciezkeFizyczna(sciezka);

            if (System.IO.File.Exists(sciezkaFizyczna))
            {
                System.IO.File.Delete(sciezkaFizyczna);
            }
        }

        private bool TowarExists(int id)
        {
            return _context.Towar.Any(e => e.IdTowaru == id);
        }
    }
}
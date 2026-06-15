using Firma.Data.Data;
using Firma.Data.Data.Sklep;
using Firma.Intranet.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.IO.Compression;

namespace Firma.Intranet.Controllers
{
    public class ZamowienieController : Controller
    {
        private readonly FirmaContext _context;
        private readonly FakturaPdfGenerator _fakturaPdfGenerator;

        public ZamowienieController(FirmaContext context)
        {
            _context = context;
            _fakturaPdfGenerator = new FakturaPdfGenerator();
        }

        public async Task<IActionResult> Index()
        {
            // Pobieram zamówienia do listy
            var firmaContext = _context.Zamowienie
                .Include(z => z.Klient)
                .OrderByDescending(z => z.DataZamowienia);

            return View(await firmaContext.ToListAsync());
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            // Pobieram zamówienie do szczegółów
            var zamowienie = await _context.Zamowienie
                .Include(z => z.Klient)
                .Include(z => z.PozycjaZamowienia)
                    .ThenInclude(p => p.Towar)
                .FirstOrDefaultAsync(m => m.IdZamowienia == id);

            if (zamowienie == null)
            {
                return NotFound();
            }

            return View(zamowienie);
        }

        public IActionResult Create()
        {
            PrzygotujKlientow();

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("IdZamowienia,NumerZamowienia,DataZamowienia,Status,WartoscRazem,Ulica,NumerDomu,NumerLokalu,KodPocztowy,Miasto,IdKlienta")] Zamowienie zamowienie)
        {
            if (ModelState.IsValid)
            {
                // Zaokrąglam wartość
                zamowienie.WartoscRazem = decimal.Round(zamowienie.WartoscRazem, 2, MidpointRounding.AwayFromZero);

                _context.Add(zamowienie);
                await _context.SaveChangesAsync();

                return RedirectToAction(nameof(Index));
            }

            PrzygotujKlientow(zamowienie.IdKlienta);

            return View(zamowienie);
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var zamowienie = await _context.Zamowienie.FindAsync(id);

            if (zamowienie == null)
            {
                return NotFound();
            }

            PrzygotujKlientow(zamowienie.IdKlienta);

            return View(zamowienie);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("IdZamowienia,NumerZamowienia,DataZamowienia,Status,WartoscRazem,Ulica,NumerDomu,NumerLokalu,KodPocztowy,Miasto,IdKlienta")] Zamowienie zamowienie)
        {
            if (id != zamowienie.IdZamowienia)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    // Zaokrąglam wartość
                    zamowienie.WartoscRazem = decimal.Round(zamowienie.WartoscRazem, 2, MidpointRounding.AwayFromZero);

                    _context.Update(zamowienie);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ZamowienieExists(zamowienie.IdZamowienia))
                    {
                        return NotFound();
                    }

                    throw;
                }

                return RedirectToAction(nameof(Index));
            }

            PrzygotujKlientow(zamowienie.IdKlienta);

            return View(zamowienie);
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var zamowienie = await _context.Zamowienie
                .Include(z => z.Klient)
                .FirstOrDefaultAsync(m => m.IdZamowienia == id);

            if (zamowienie == null)
            {
                return NotFound();
            }

            return View(zamowienie);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var zamowienie = await _context.Zamowienie.FindAsync(id);

            if (zamowienie != null)
            {
                _context.Zamowienie.Remove(zamowienie);
            }

            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> PobierzFakturePdf(int id)
        {
            // Pobieram dane do faktury
            var zamowienie = await PobierzZamowienieDoFaktury(id);

            if (zamowienie == null)
            {
                return NotFound();
            }

            var pdf = _fakturaPdfGenerator.Generuj(zamowienie);
            var nazwaPliku = $"Faktura_{CzyscNazwePliku(zamowienie.NumerZamowienia)}.pdf";

            return File(pdf, "application/pdf", nazwaPliku);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> PobierzFakturyPdfZip(int[] ids)
        {
            if (ids == null || ids.Length == 0)
            {
                return RedirectToAction(nameof(Index));
            }

            // Pobieram zamówienia do paczki faktur
            var zamowienia = await _context.Zamowienie
                .Include(z => z.Klient)
                .Include(z => z.PozycjaZamowienia)
                    .ThenInclude(p => p.Towar)
                .Where(z => ids.Contains(z.IdZamowienia))
                .OrderByDescending(z => z.DataZamowienia)
                .ToListAsync();

            if (!zamowienia.Any())
            {
                return RedirectToAction(nameof(Index));
            }

            using var memoryStream = new MemoryStream();
            var uzyteNazwy = new HashSet<string>();

            using (var archive = new ZipArchive(memoryStream, ZipArchiveMode.Create, true))
            {
                foreach (var zamowienie in zamowienia)
                {
                    // Generuję fakturę do ZIP
                    var pdf = _fakturaPdfGenerator.Generuj(zamowienie);
                    var nazwaPliku = PrzygotujUnikalnaNazwePdf(zamowienie, uzyteNazwy);

                    var entry = archive.CreateEntry(nazwaPliku, CompressionLevel.Fastest);

                    using var entryStream = entry.Open();
                    await entryStream.WriteAsync(pdf);
                }
            }

            var zip = memoryStream.ToArray();
            var nazwaZip = $"Faktury_{DateTime.Now:yyyyMMdd_HHmm}.zip";

            return File(zip, "application/zip", nazwaZip);
        }

        private async Task<Zamowienie?> PobierzZamowienieDoFaktury(int id)
        {
            return await _context.Zamowienie
                .Include(z => z.Klient)
                .Include(z => z.PozycjaZamowienia)
                    .ThenInclude(p => p.Towar)
                .FirstOrDefaultAsync(z => z.IdZamowienia == id);
        }

        private void PrzygotujKlientow(int? idKlienta = null)
        {
            ViewData["IdKlienta"] = new SelectList(
                _context.Klient.OrderBy(k => k.Email),
                "IdKlienta",
                "Email",
                idKlienta);
        }

        private static string PrzygotujUnikalnaNazwePdf(Zamowienie zamowienie, HashSet<string> uzyteNazwy)
        {
            var bazaNazwy = $"Faktura_{CzyscNazwePliku(zamowienie.NumerZamowienia)}";
            var nazwaPliku = $"{bazaNazwy}.pdf";
            var licznik = 1;

            while (uzyteNazwy.Contains(nazwaPliku))
            {
                nazwaPliku = $"{bazaNazwy}_{licznik}.pdf";
                licznik++;
            }

            uzyteNazwy.Add(nazwaPliku);

            return nazwaPliku;
        }

        private static string CzyscNazwePliku(string tekst)
        {
            foreach (var znak in Path.GetInvalidFileNameChars())
            {
                tekst = tekst.Replace(znak, '_');
            }

            return tekst;
        }

        private bool ZamowienieExists(int id)
        {
            return _context.Zamowienie.Any(e => e.IdZamowienia == id);
        }
    }
}
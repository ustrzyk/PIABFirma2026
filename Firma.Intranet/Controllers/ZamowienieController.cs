using Firma.Data.Data.Sklep;
using Firma.Intranet.Interfaces.Intranet;
using Firma.Intranet.Models;
using Firma.Intranet.Services.Dokumenty;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.IO.Compression;

namespace Firma.Intranet.Controllers
{
    public class ZamowienieController : Controller
    {
        private readonly IZamowienieIntranetService _zamowienieIntranetService;
        private readonly FakturaPdfGenerator _fakturaPdfGenerator;
        private readonly ZamowienieExcelGenerator _zamowienieExcelGenerator;
        private readonly ZamowienieExcelSzablonGenerator _zamowienieExcelSzablonGenerator;
        private readonly ZamowienieExcelImporter _zamowienieExcelImporter;

        public ZamowienieController(
            IZamowienieIntranetService zamowienieIntranetService,
            FakturaPdfGenerator fakturaPdfGenerator,
            ZamowienieExcelGenerator zamowienieExcelGenerator,
            ZamowienieExcelSzablonGenerator zamowienieExcelSzablonGenerator,
            ZamowienieExcelImporter zamowienieExcelImporter)
        {
            _zamowienieIntranetService = zamowienieIntranetService;
            _fakturaPdfGenerator = fakturaPdfGenerator;
            _zamowienieExcelGenerator = zamowienieExcelGenerator;
            _zamowienieExcelSzablonGenerator = zamowienieExcelSzablonGenerator;
            _zamowienieExcelImporter = zamowienieExcelImporter;
        }

        public async Task<IActionResult> Index()
        {
            var zamowienia = await _zamowienieIntranetService.PobierzListe();

            return View(zamowienia);
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var zamowienie = await _zamowienieIntranetService.PobierzSzczegoly(id.Value);

            if (zamowienie == null)
            {
                return NotFound();
            }

            return View(zamowienie);
        }

        public async Task<IActionResult> Create()
        {
            await PrzygotujKlientow();

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("IdZamowienia,NumerZamowienia,DataZamowienia,Status,WartoscRazem,Ulica,NumerDomu,NumerLokalu,KodPocztowy,Miasto,IdKlienta")] Zamowienie zamowienie)
        {
            if (ModelState.IsValid)
            {
                await _zamowienieIntranetService.Dodaj(zamowienie);

                return RedirectToAction(nameof(Index));
            }

            await PrzygotujKlientow(zamowienie.IdKlienta);

            return View(zamowienie);
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var zamowienie = await _zamowienieIntranetService.PobierzDoEdycji(id.Value);

            if (zamowienie == null)
            {
                return NotFound();
            }

            await PrzygotujKlientow(zamowienie.IdKlienta);

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
                var zapisano = await _zamowienieIntranetService.Aktualizuj(id, zamowienie);

                if (!zapisano)
                {
                    return NotFound();
                }

                return RedirectToAction(nameof(Index));
            }

            await PrzygotujKlientow(zamowienie.IdKlienta);

            return View(zamowienie);
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var zamowienie = await _zamowienieIntranetService.PobierzDoUsuniecia(id.Value);

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
            await _zamowienieIntranetService.Usun(id);

            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> PobierzFakturePdf(int id)
        {
            var zamowienie = await _zamowienieIntranetService.PobierzDoDokumentow(id);

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

            var zamowienia = await _zamowienieIntranetService.PobierzZaznaczoneDoDokumentow(ids);

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

        public async Task<IActionResult> PobierzZamowieniaExcel()
        {
            var zamowienia = await _zamowienieIntranetService.PobierzWszystkieDoDokumentow();

            var excel = _zamowienieExcelGenerator.Generuj(zamowienia);
            var nazwaPliku = $"Zamowienia_{DateTime.Now:yyyyMMdd_HHmm}.xlsx";

            return File(
                excel,
                "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                nazwaPliku);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> PobierzZaznaczoneZamowieniaExcel(int[] ids)
        {
            if (ids == null || ids.Length == 0)
            {
                return RedirectToAction(nameof(Index));
            }

            var zamowienia = await _zamowienieIntranetService.PobierzZaznaczoneDoDokumentow(ids);

            if (!zamowienia.Any())
            {
                return RedirectToAction(nameof(Index));
            }

            var excel = _zamowienieExcelGenerator.Generuj(zamowienia);
            var nazwaPliku = $"Zamowienia_zaznaczone_{DateTime.Now:yyyyMMdd_HHmm}.xlsx";

            return File(
                excel,
                "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                nazwaPliku);
        }

        public IActionResult PobierzSzablonImportuExcel()
        {
            var excel = _zamowienieExcelSzablonGenerator.Generuj();

            return File(
                excel,
                "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                "Szablon_importu_zamowien.xlsx");
        }

        public IActionResult ImportExcel()
        {
            return View(new ImportZamowienExcelViewModel());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ImportExcel(ImportZamowienExcelViewModel model)
        {
            var wynik = await _zamowienieExcelImporter.Importuj(model.Plik);

            model.CzyWykonanoImport = wynik.CzyWykonanoImport;
            model.LiczbaDodanychZamowien = wynik.LiczbaDodanychZamowien;
            model.LiczbaDodanychKlientow = wynik.LiczbaDodanychKlientow;
            model.Bledy = wynik.Bledy;

            return View(model);
        }

        private async Task PrzygotujKlientow(int? idKlienta = null)
        {
            var klienci = await _zamowienieIntranetService.PobierzKlientowDoSelectList();

            ViewData["IdKlienta"] = new SelectList(
                klienci,
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
    }
}
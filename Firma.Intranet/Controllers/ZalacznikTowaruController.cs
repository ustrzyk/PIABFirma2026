using Firma.Data.Data;
using Firma.Data.Data.Sklep;
using Firma.Intranet.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace Firma.Intranet.Controllers
{
    public class ZalacznikTowaruController : Controller
    {
        private readonly FirmaContext _context;
        private readonly IWebHostEnvironment _environment;

        private readonly string[] _dozwoloneRozszerzenia =
        {
            ".pdf",
            ".doc",
            ".docx",
            ".xls",
            ".xlsx",
            ".txt",
            ".png",
            ".jpg",
            ".jpeg",
            ".webp"
        };

        private const long MaksymalnyRozmiarPliku = 10 * 1024 * 1024;

        public ZalacznikTowaruController(FirmaContext context, IWebHostEnvironment environment)
        {
            _context = context;
            _environment = environment;
        }

        public async Task<IActionResult> Index()
        {
            var zalaczniki = await _context.ZalacznikTowaru
                .Include(z => z.Towar)
                .OrderByDescending(z => z.DataDodania)
                .ToListAsync();

            return View(zalaczniki);
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var zalacznik = await _context.ZalacznikTowaru
                .Include(z => z.Towar)
                .FirstOrDefaultAsync(z => z.IdZalacznikaTowaru == id);

            if (zalacznik == null)
            {
                return NotFound();
            }

            return View(zalacznik);
        }

        public IActionResult Create(int? idTowaru)
        {
            var model = new ZalacznikTowaruFormModel
            {
                IdTowaru = idTowaru ?? 0
            };

            PrzygotujTowary(model.IdTowaru);

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ZalacznikTowaruFormModel model)
        {
            if (model.Plik == null || model.Plik.Length == 0)
            {
                ModelState.AddModelError(nameof(model.Plik), "Wybierz plik");
            }
            else
            {
                WalidujPlik(model.Plik);
            }

            if (ModelState.IsValid)
            {
                var zapisanyPlik = await ZapiszPlik(model.Plik!);

                var zalacznik = new ZalacznikTowaru
                {
                    IdTowaru = model.IdTowaru,
                    NazwaPliku = zapisanyPlik.NazwaPliku,
                    NazwaOryginalna = Path.GetFileName(model.Plik!.FileName),
                    Sciezka = zapisanyPlik.Sciezka,
                    TypPliku = model.Plik.ContentType,
                    Rozmiar = model.Plik.Length,
                    Opis = model.Opis,
                    DataDodania = DateTime.Now,
                    CzyAktywny = true
                };

                _context.Add(zalacznik);
                await _context.SaveChangesAsync();

                return RedirectToAction(nameof(Index));
            }

            PrzygotujTowary(model.IdTowaru);

            return View(model);
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var zalacznik = await _context.ZalacznikTowaru.FindAsync(id);

            if (zalacznik == null)
            {
                return NotFound();
            }

            var model = new ZalacznikTowaruFormModel
            {
                IdZalacznikaTowaru = zalacznik.IdZalacznikaTowaru,
                IdTowaru = zalacznik.IdTowaru,
                Opis = zalacznik.Opis
            };

            PrzygotujTowary(model.IdTowaru);

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, ZalacznikTowaruFormModel model)
        {
            if (id != model.IdZalacznikaTowaru)
            {
                return NotFound();
            }

            if (model.Plik != null && model.Plik.Length > 0)
            {
                WalidujPlik(model.Plik);
            }

            if (ModelState.IsValid)
            {
                var zalacznik = await _context.ZalacznikTowaru.FindAsync(id);

                if (zalacznik == null)
                {
                    return NotFound();
                }

                zalacznik.IdTowaru = model.IdTowaru;
                zalacznik.Opis = model.Opis;

                if (model.Plik != null && model.Plik.Length > 0)
                {
                    // Podmieniam plik załącznika
                    UsunPlik(zalacznik.Sciezka);

                    var zapisanyPlik = await ZapiszPlik(model.Plik);

                    zalacznik.NazwaPliku = zapisanyPlik.NazwaPliku;
                    zalacznik.NazwaOryginalna = Path.GetFileName(model.Plik.FileName);
                    zalacznik.Sciezka = zapisanyPlik.Sciezka;
                    zalacznik.TypPliku = model.Plik.ContentType;
                    zalacznik.Rozmiar = model.Plik.Length;
                    zalacznik.DataDodania = DateTime.Now;
                }

                _context.Update(zalacznik);
                await _context.SaveChangesAsync();

                return RedirectToAction(nameof(Index));
            }

            PrzygotujTowary(model.IdTowaru);

            return View(model);
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var zalacznik = await _context.ZalacznikTowaru
                .Include(z => z.Towar)
                .FirstOrDefaultAsync(z => z.IdZalacznikaTowaru == id);

            if (zalacznik == null)
            {
                return NotFound();
            }

            return View(zalacznik);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var zalacznik = await _context.ZalacznikTowaru.FindAsync(id);

            if (zalacznik != null)
            {
                UsunPlik(zalacznik.Sciezka);
                _context.ZalacznikTowaru.Remove(zalacznik);
            }

            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Pobierz(int id)
        {
            var zalacznik = await _context.ZalacznikTowaru.FindAsync(id);

            if (zalacznik == null)
            {
                return NotFound();
            }

            var sciezkaFizyczna = PobierzSciezkeFizyczna(zalacznik.Sciezka);

            if (!System.IO.File.Exists(sciezkaFizyczna))
            {
                return NotFound();
            }

            return PhysicalFile(sciezkaFizyczna, zalacznik.TypPliku, zalacznik.NazwaOryginalna);
        }

        private void PrzygotujTowary(int? idTowaru = null)
        {
            ViewData["IdTowaru"] = new SelectList(
                _context.Towar.OrderBy(t => t.Nazwa),
                "IdTowaru",
                "Nazwa",
                idTowaru);
        }

        private void WalidujPlik(IFormFile plik)
        {
            var rozszerzenie = Path.GetExtension(plik.FileName).ToLowerInvariant();

            if (!_dozwoloneRozszerzenia.Contains(rozszerzenie))
            {
                ModelState.AddModelError(nameof(ZalacznikTowaruFormModel.Plik), "Dozwolone pliki: PDF, DOC, DOCX, XLS, XLSX, TXT, PNG, JPG, JPEG, WEBP");
            }

            if (plik.Length > MaksymalnyRozmiarPliku)
            {
                ModelState.AddModelError(nameof(ZalacznikTowaruFormModel.Plik), "Maksymalny rozmiar pliku to 10 MB");
            }
        }

        private async Task<(string NazwaPliku, string Sciezka)> ZapiszPlik(IFormFile plik)
        {
            var folder = PobierzFolderUploadu();

            Directory.CreateDirectory(folder);

            var rozszerzenie = Path.GetExtension(plik.FileName).ToLowerInvariant();
            var nazwaPliku = $"{Guid.NewGuid():N}{rozszerzenie}";
            var sciezkaFizyczna = Path.Combine(folder, nazwaPliku);

            using (var stream = new FileStream(sciezkaFizyczna, FileMode.Create))
            {
                await plik.CopyToAsync(stream);
            }

            var sciezkaPubliczna = $"/uploads/towary/{nazwaPliku}";

            return (nazwaPliku, sciezkaPubliczna);
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

        private bool ZalacznikTowaruExists(int id)
        {
            return _context.ZalacznikTowaru.Any(e => e.IdZalacznikaTowaru == id);
        }
    }
}
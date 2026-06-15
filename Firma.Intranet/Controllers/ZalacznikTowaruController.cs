using Firma.Intranet.Interfaces.Intranet;
using Firma.Intranet.Models;
using Firma.Intranet.Services.Data.Intranet;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Firma.Intranet.Controllers
{
    public class ZalacznikTowaruController : Controller
    {
        private readonly IZalacznikTowaruIntranetService _zalacznikTowaruIntranetService;
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

        public ZalacznikTowaruController(
            IZalacznikTowaruIntranetService zalacznikTowaruIntranetService,
            IWebHostEnvironment environment)
        {
            _zalacznikTowaruIntranetService = zalacznikTowaruIntranetService;
            _environment = environment;
        }

        public async Task<IActionResult> Index()
        {
            var zalaczniki = await _zalacznikTowaruIntranetService.PobierzListe();

            return View(zalaczniki);
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var zalacznik = await _zalacznikTowaruIntranetService.PobierzSzczegoly(id.Value);

            if (zalacznik == null)
            {
                return NotFound();
            }

            return View(zalacznik);
        }

        public async Task<IActionResult> Create(int? idTowaru)
        {
            var model = new ZalacznikTowaruFormModel
            {
                IdTowaru = idTowaru ?? 0
            };

            await PrzygotujTowary(model.IdTowaru);

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
                var plikDto = PrzygotujPlikDto(model.Plik!);

                await _zalacznikTowaruIntranetService.Dodaj(
                    model.IdTowaru,
                    model.Opis,
                    plikDto,
                    PobierzFolderUploadu());

                return RedirectToAction(nameof(Index));
            }

            await PrzygotujTowary(model.IdTowaru);

            return View(model);
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var zalacznik = await _zalacznikTowaruIntranetService.PobierzDoEdycji(id.Value);

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

            await PrzygotujTowary(model.IdTowaru);

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
                PlikZalacznikaDto? plikDto = null;

                if (model.Plik != null && model.Plik.Length > 0)
                {
                    plikDto = PrzygotujPlikDto(model.Plik);
                }

                var zapisano = await _zalacznikTowaruIntranetService.Aktualizuj(
                    id,
                    model.IdTowaru,
                    model.Opis,
                    plikDto,
                    PobierzFolderUploadu());

                if (!zapisano)
                {
                    return NotFound();
                }

                return RedirectToAction(nameof(Index));
            }

            await PrzygotujTowary(model.IdTowaru);

            return View(model);
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var zalacznik = await _zalacznikTowaruIntranetService.PobierzDoUsuniecia(id.Value);

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
            await _zalacznikTowaruIntranetService.Usun(id, PobierzFolderUploadu());

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

            await _zalacznikTowaruIntranetService.UsunZaznaczone(ids, PobierzFolderUploadu());

            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Pobierz(int id)
        {
            var zalacznik = await _zalacznikTowaruIntranetService.PobierzDoPobrania(id);

            if (zalacznik == null)
            {
                return NotFound();
            }

            var sciezkaFizyczna = _zalacznikTowaruIntranetService.PobierzSciezkeFizyczna(
                PobierzFolderUploadu(),
                zalacznik.Sciezka);

            if (!System.IO.File.Exists(sciezkaFizyczna))
            {
                return NotFound();
            }

            return PhysicalFile(
                sciezkaFizyczna,
                zalacznik.TypPliku,
                zalacznik.NazwaOryginalna);
        }

        private async Task PrzygotujTowary(int? idTowaru = null)
        {
            var towary = await _zalacznikTowaruIntranetService.PobierzTowaryDoSelectList();

            ViewData["IdTowaru"] = new SelectList(
                towary,
                "IdTowaru",
                "Nazwa",
                idTowaru);
        }

        private void WalidujPlik(IFormFile plik)
        {
            var rozszerzenie = Path.GetExtension(plik.FileName).ToLowerInvariant();

            if (!_dozwoloneRozszerzenia.Contains(rozszerzenie))
            {
                ModelState.AddModelError(
                    nameof(ZalacznikTowaruFormModel.Plik),
                    "Dozwolone pliki: PDF, DOC, DOCX, XLS, XLSX, TXT, PNG, JPG, JPEG, WEBP");
            }

            if (plik.Length > MaksymalnyRozmiarPliku)
            {
                ModelState.AddModelError(
                    nameof(ZalacznikTowaruFormModel.Plik),
                    "Maksymalny rozmiar pliku to 10 MB");
            }
        }

        private static PlikZalacznikaDto PrzygotujPlikDto(IFormFile plik)
        {
            return new PlikZalacznikaDto
            {
                Stream = plik.OpenReadStream(),
                NazwaOryginalna = plik.FileName,
                ContentType = plik.ContentType,
                Rozmiar = plik.Length
            };
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
using Firma.Data.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Firma.PortalWWW.Controllers
{
    public class SklepController : Controller
    {
        private readonly FirmaContext _context;

        public SklepController(FirmaContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index(int? id)
        {
            ViewBag.ModelStrony = await _context.Strona
                .Where(s => s.CzyAktywny)
                .OrderBy(s => s.Pozycja)
                .ToListAsync();

            ViewBag.ModelAktualnosci = await _context.Aktualnosc
                .Where(a => a.CzyAktywny)
                .OrderByDescending(a => a.Pozycja)
                .Take(3)
                .ToListAsync();

            ViewBag.Rodzaje = await _context.Rodzaj
                .Where(r => r.CzyAktywny)
                .OrderBy(r => r.Nazwa)
                .ToListAsync();

            var towary = _context.Towar
                .Include(t => t.Rodzaj)
                .Include(t => t.Producent)
                .Include(t => t.StanMagazynowy)
                .Where(t =>
                    t.CzyAktywny &&
                    t.Rodzaj != null &&
                    t.Rodzaj.CzyAktywny &&
                    t.Producent != null &&
                    t.Producent.CzyAktywny)
                .AsQueryable();

            if (id != null)
            {
                towary = towary.Where(t => t.IdRodzaju == id);
            }

            var items = await towary
                .OrderBy(t => t.Nazwa)
                .ToListAsync();

            return View(items);
        }

        public async Task<IActionResult> Szczegoly(int id)
        {
            ViewBag.ModelStrony = await _context.Strona
                .Where(s => s.CzyAktywny)
                .OrderBy(s => s.Pozycja)
                .ToListAsync();

            ViewBag.ModelAktualnosci = await _context.Aktualnosc
                .Where(a => a.CzyAktywny)
                .OrderByDescending(a => a.Pozycja)
                .Take(3)
                .ToListAsync();

            ViewBag.Rodzaje = await _context.Rodzaj
                .Where(r => r.CzyAktywny)
                .OrderBy(r => r.Nazwa)
                .ToListAsync();

            var item = await _context.Towar
                .Include(t => t.Rodzaj)
                .Include(t => t.Producent)
                .Include(t => t.StanMagazynowy)
                .FirstOrDefaultAsync(t =>
                    t.IdTowaru == id &&
                    t.CzyAktywny &&
                    t.Rodzaj != null &&
                    t.Rodzaj.CzyAktywny &&
                    t.Producent != null &&
                    t.Producent.CzyAktywny);

            if (item == null)
            {
                return NotFound();
            }

            return View(item);
        }
    }
}
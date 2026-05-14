using Firma.Data.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Firma.PortalWWW.Controllers
{
    public class StanMagazynowyController : Controller
    {
        private readonly FirmaContext _context;

        public StanMagazynowyController(FirmaContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
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

            var items = await _context.StanMagazynowy
                .Where(s =>
                    s.CzyAktywny &&
                    s.Towar != null &&
                    s.Towar.CzyAktywny)
                .Include(s => s.Towar)
                    .ThenInclude(t => t.Rodzaj)
                .Include(s => s.Towar)
                    .ThenInclude(t => t.Producent)
                .OrderBy(s => s.IloscSztuk)
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

            var item = await _context.StanMagazynowy
                .Include(s => s.Towar)
                    .ThenInclude(t => t.Rodzaj)
                .Include(s => s.Towar)
                    .ThenInclude(t => t.Producent)
                .FirstOrDefaultAsync(s =>
                    s.IdStanuMagazynowego == id &&
                    s.CzyAktywny &&
                    s.Towar != null &&
                    s.Towar.CzyAktywny);

            if (item == null)
            {
                return NotFound();
            }

            return View(item);
        }
    }
}
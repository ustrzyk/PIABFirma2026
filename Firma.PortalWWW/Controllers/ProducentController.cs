using Firma.Data.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Firma.PortalWWW.Controllers
{
    public class ProducentController : Controller
    {
        private readonly FirmaContext _context;

        public ProducentController(FirmaContext context)
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

            var items = await _context.Producent
                .Where(p => p.CzyAktywny)
                .Include(p => p.Towar)
                .OrderBy(p => p.Nazwa)
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

            var item = await _context.Producent
                .Include(p => p.Towar)
                .FirstOrDefaultAsync(p => p.IdProducenta == id && p.CzyAktywny);

            if (item == null)
            {
                return NotFound();
            }

            return View(item);
        }
    }
}
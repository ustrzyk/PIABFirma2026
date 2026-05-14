using Firma.Data.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Firma.PortalWWW.Controllers
{
    public class PromocjaController : Controller
    {
        private readonly FirmaContext _context;

        public PromocjaController(FirmaContext context)
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

            var items = await _context.Promocja
                .Where(p => p.CzyAktywny)
                .OrderByDescending(p => p.DataOd)
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

            var item = await _context.Promocja
                .FirstOrDefaultAsync(p => p.IdPromocji == id && p.CzyAktywny);

            if (item == null)
            {
                return NotFound();
            }

            return View(item);
        }
    }
}
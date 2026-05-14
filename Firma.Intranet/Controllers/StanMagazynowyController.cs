using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Firma.Data.Data;
using Firma.Data.Data.Sklep;

namespace Firma.Intranet.Controllers
{
    public class StanMagazynowyController : Controller
    {
        private readonly FirmaContext _context;

        public StanMagazynowyController(FirmaContext context)
        {
            _context = context;
        }

        // GET: StanMagazynowy
        public async Task<IActionResult> Index()
        {
            var firmaContext = _context.StanMagazynowy
                .Include(s => s.Towar)
                .OrderBy(s => s.Towar != null ? s.Towar.Nazwa : "");

            return View(await firmaContext.ToListAsync());
        }

        // GET: StanMagazynowy/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var stanMagazynowy = await _context.StanMagazynowy
                .Include(s => s.Towar)
                .FirstOrDefaultAsync(m => m.IdStanuMagazynowego == id);

            if (stanMagazynowy == null)
            {
                return NotFound();
            }

            return View(stanMagazynowy);
        }

        // GET: StanMagazynowy/Create
        public IActionResult Create()
        {
            ViewData["IdTowaru"] = new SelectList(_context.Towar.OrderBy(t => t.Nazwa), "IdTowaru", "Nazwa");
            return View();
        }

        // POST: StanMagazynowy/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("IdStanuMagazynowego,IloscSztuk,MinimalnaIlosc,Lokalizacja,CzyAktywny,IdTowaru")] StanMagazynowy stanMagazynowy)
        {
            if (ModelState.IsValid)
            {
                _context.Add(stanMagazynowy);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            ViewData["IdTowaru"] = new SelectList(_context.Towar.OrderBy(t => t.Nazwa), "IdTowaru", "Nazwa", stanMagazynowy.IdTowaru);
            return View(stanMagazynowy);
        }

        // GET: StanMagazynowy/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var stanMagazynowy = await _context.StanMagazynowy.FindAsync(id);

            if (stanMagazynowy == null)
            {
                return NotFound();
            }

            ViewData["IdTowaru"] = new SelectList(_context.Towar.OrderBy(t => t.Nazwa), "IdTowaru", "Nazwa", stanMagazynowy.IdTowaru);
            return View(stanMagazynowy);
        }

        // POST: StanMagazynowy/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("IdStanuMagazynowego,IloscSztuk,MinimalnaIlosc,Lokalizacja,CzyAktywny,IdTowaru")] StanMagazynowy stanMagazynowy)
        {
            if (id != stanMagazynowy.IdStanuMagazynowego)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(stanMagazynowy);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!StanMagazynowyExists(stanMagazynowy.IdStanuMagazynowego))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }

                return RedirectToAction(nameof(Index));
            }

            ViewData["IdTowaru"] = new SelectList(_context.Towar.OrderBy(t => t.Nazwa), "IdTowaru", "Nazwa", stanMagazynowy.IdTowaru);
            return View(stanMagazynowy);
        }

        // GET: StanMagazynowy/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var stanMagazynowy = await _context.StanMagazynowy
                .Include(s => s.Towar)
                .FirstOrDefaultAsync(m => m.IdStanuMagazynowego == id);

            if (stanMagazynowy == null)
            {
                return NotFound();
            }

            return View(stanMagazynowy);
        }

        // POST: StanMagazynowy/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var stanMagazynowy = await _context.StanMagazynowy.FindAsync(id);

            if (stanMagazynowy != null)
            {
                _context.StanMagazynowy.Remove(stanMagazynowy);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool StanMagazynowyExists(int id)
        {
            return _context.StanMagazynowy.Any(e => e.IdStanuMagazynowego == id);
        }
    }
}
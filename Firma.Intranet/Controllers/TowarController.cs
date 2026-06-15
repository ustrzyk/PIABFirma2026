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
    public class TowarController : Controller
    {
        private readonly FirmaContext _context;

        public TowarController(FirmaContext context)
        {
            _context = context;
        }

        // GET: Towar
        public async Task<IActionResult> Index()
        {
            var firmaContext = _context.Towar
                .Include(t => t.Producent)
                .Include(t => t.Rodzaj)
                .OrderBy(t => t.Nazwa);

            return View(await firmaContext.ToListAsync());
        }

        // GET: Towar/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

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

        // GET: Towar/Create
        public IActionResult Create()
        {
            ViewData["IdProducenta"] = new SelectList(_context.Producent.OrderBy(p => p.Nazwa), "IdProducenta", "Nazwa");
            ViewData["IdRodzaju"] = new SelectList(_context.Rodzaj.OrderBy(r => r.Nazwa), "IdRodzaju", "Nazwa");
            return View();
        }

        // POST: Towar/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("IdTowaru,Kod,Nazwa,Cena,FotoUrl,Opis,CzyAktywny,IdRodzaju,IdProducenta")] Towar towar)
        {
            if (ModelState.IsValid)
            {
                towar.Cena = decimal.Round(towar.Cena, 2, MidpointRounding.AwayFromZero);

                _context.Add(towar);
                await _context.SaveChangesAsync();

                return RedirectToAction(nameof(Index));
            }

            ViewData["IdProducenta"] = new SelectList(_context.Producent.OrderBy(p => p.Nazwa), "IdProducenta", "Nazwa", towar.IdProducenta);
            ViewData["IdRodzaju"] = new SelectList(_context.Rodzaj.OrderBy(r => r.Nazwa), "IdRodzaju", "Nazwa", towar.IdRodzaju);

            return View(towar);
        }

        // GET: Towar/Edit/5
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

            ViewData["IdProducenta"] = new SelectList(_context.Producent.OrderBy(p => p.Nazwa), "IdProducenta", "Nazwa", towar.IdProducenta);
            ViewData["IdRodzaju"] = new SelectList(_context.Rodzaj.OrderBy(r => r.Nazwa), "IdRodzaju", "Nazwa", towar.IdRodzaju);

            return View(towar);
        }

        // POST: Towar/Edit/5
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
                    else
                    {
                        throw;
                    }
                }

                return RedirectToAction(nameof(Index));
            }

            ViewData["IdProducenta"] = new SelectList(_context.Producent.OrderBy(p => p.Nazwa), "IdProducenta", "Nazwa", towar.IdProducenta);
            ViewData["IdRodzaju"] = new SelectList(_context.Rodzaj.OrderBy(r => r.Nazwa), "IdRodzaju", "Nazwa", towar.IdRodzaju);

            return View(towar);
        }

        // GET: Towar/Delete/5
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

        // POST: Towar/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var towar = await _context.Towar.FindAsync(id);

            if (towar != null)
            {
                _context.Towar.Remove(towar);
            }

            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        private bool TowarExists(int id)
        {
            return _context.Towar.Any(e => e.IdTowaru == id);
        }
    }
}
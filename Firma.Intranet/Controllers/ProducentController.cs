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
    public class ProducentController : Controller
    {
        private readonly FirmaContext _context;

        public ProducentController(FirmaContext context)
        {
            _context = context;
        }

        // GET: Producent
        public async Task<IActionResult> Index()
        {
            return View(await _context.Producent
                .OrderBy(p => p.Nazwa)
                .ToListAsync());
        }

        // GET: Producent/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var producent = await _context.Producent
                .FirstOrDefaultAsync(m => m.IdProducenta == id);

            if (producent == null)
            {
                return NotFound();
            }

            return View(producent);
        }

        // GET: Producent/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Producent/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("IdProducenta,Nazwa,Kraj,StronaWWW,Opis,CzyAktywny")] Producent producent)
        {
            if (ModelState.IsValid)
            {
                _context.Add(producent);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            return View(producent);
        }

        // GET: Producent/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var producent = await _context.Producent.FindAsync(id);

            if (producent == null)
            {
                return NotFound();
            }

            return View(producent);
        }

        // POST: Producent/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("IdProducenta,Nazwa,Kraj,StronaWWW,Opis,CzyAktywny")] Producent producent)
        {
            if (id != producent.IdProducenta)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(producent);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ProducentExists(producent.IdProducenta))
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

            return View(producent);
        }

        // GET: Producent/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var producent = await _context.Producent
                .FirstOrDefaultAsync(m => m.IdProducenta == id);

            if (producent == null)
            {
                return NotFound();
            }

            return View(producent);
        }

        // POST: Producent/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var producent = await _context.Producent.FindAsync(id);

            if (producent != null)
            {
                _context.Producent.Remove(producent);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ProducentExists(int id)
        {
            return _context.Producent.Any(e => e.IdProducenta == id);
        }
    }
}
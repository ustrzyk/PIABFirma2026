using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Firma.Data.Data;
using Firma.Data.Data.CMS;

namespace Firma.Intranet.Controllers
{
    public class PromocjaController : Controller
    {
        private readonly FirmaContext _context;

        public PromocjaController(FirmaContext context)
        {
            _context = context;
        }

        // GET: Promocja
        public async Task<IActionResult> Index()
        {
            return View(await _context.Promocja
                .OrderByDescending(p => p.DataOd)
                .ToListAsync());
        }

        // GET: Promocja/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var promocja = await _context.Promocja
                .FirstOrDefaultAsync(m => m.IdPromocji == id);

            if (promocja == null)
            {
                return NotFound();
            }

            return View(promocja);
        }

        // GET: Promocja/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Promocja/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("IdPromocji,Tytul,Opis,RabatProcentowy,DataOd,DataDo,CzyAktywny")] Promocja promocja)
        {
            if (ModelState.IsValid)
            {
                _context.Add(promocja);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            return View(promocja);
        }

        // GET: Promocja/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var promocja = await _context.Promocja.FindAsync(id);

            if (promocja == null)
            {
                return NotFound();
            }

            return View(promocja);
        }

        // POST: Promocja/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("IdPromocji,Tytul,Opis,RabatProcentowy,DataOd,DataDo,CzyAktywny")] Promocja promocja)
        {
            if (id != promocja.IdPromocji)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(promocja);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PromocjaExists(promocja.IdPromocji))
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

            return View(promocja);
        }

        // GET: Promocja/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var promocja = await _context.Promocja
                .FirstOrDefaultAsync(m => m.IdPromocji == id);

            if (promocja == null)
            {
                return NotFound();
            }

            return View(promocja);
        }

        // POST: Promocja/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var promocja = await _context.Promocja.FindAsync(id);

            if (promocja != null)
            {
                _context.Promocja.Remove(promocja);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool PromocjaExists(int id)
        {
            return _context.Promocja.Any(e => e.IdPromocji == id);
        }
    }
}
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
    public class UstawieniePortaluController : Controller
    {
        private readonly FirmaContext _context;

        public UstawieniePortaluController(FirmaContext context)
        {
            _context = context;
        }

        // GET: UstawieniePortalu
        public async Task<IActionResult> Index()
        {
            return View(await _context.UstawieniePortalu
                .OrderBy(u => u.Klucz)
                .ToListAsync());
        }

        // GET: UstawieniePortalu/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var ustawieniePortalu = await _context.UstawieniePortalu
                .FirstOrDefaultAsync(m => m.IdUstawieniaPortalu == id);

            if (ustawieniePortalu == null)
            {
                return NotFound();
            }

            return View(ustawieniePortalu);
        }

        // GET: UstawieniePortalu/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: UstawieniePortalu/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("IdUstawieniaPortalu,Klucz,Wartosc,Opis,CzyAktywny")] UstawieniePortalu ustawieniePortalu)
        {
            if (ModelState.IsValid)
            {
                _context.Add(ustawieniePortalu);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            return View(ustawieniePortalu);
        }

        // GET: UstawieniePortalu/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var ustawieniePortalu = await _context.UstawieniePortalu.FindAsync(id);

            if (ustawieniePortalu == null)
            {
                return NotFound();
            }

            return View(ustawieniePortalu);
        }

        // POST: UstawieniePortalu/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("IdUstawieniaPortalu,Klucz,Wartosc,Opis,CzyAktywny")] UstawieniePortalu ustawieniePortalu)
        {
            if (id != ustawieniePortalu.IdUstawieniaPortalu)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(ustawieniePortalu);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!UstawieniePortaluExists(ustawieniePortalu.IdUstawieniaPortalu))
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

            return View(ustawieniePortalu);
        }

        // GET: UstawieniePortalu/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var ustawieniePortalu = await _context.UstawieniePortalu
                .FirstOrDefaultAsync(m => m.IdUstawieniaPortalu == id);

            if (ustawieniePortalu == null)
            {
                return NotFound();
            }

            return View(ustawieniePortalu);
        }

        // POST: UstawieniePortalu/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var ustawieniePortalu = await _context.UstawieniePortalu.FindAsync(id);

            if (ustawieniePortalu != null)
            {
                _context.UstawieniePortalu.Remove(ustawieniePortalu);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool UstawieniePortaluExists(int id)
        {
            return _context.UstawieniePortalu.Any(e => e.IdUstawieniaPortalu == id);
        }
    }
}
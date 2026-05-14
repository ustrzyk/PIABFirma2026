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
    public class PozycjaZamowieniaController : Controller
    {
        private readonly FirmaContext _context;

        public PozycjaZamowieniaController(FirmaContext context)
        {
            _context = context;
        }

        // GET: PozycjaZamowienia
        public async Task<IActionResult> Index()
        {
            var firmaContext = _context.PozycjaZamowienia
                .Include(p => p.Towar)
                .Include(p => p.Zamowienie);

            return View(await firmaContext.ToListAsync());
        }

        // GET: PozycjaZamowienia/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var pozycjaZamowienia = await _context.PozycjaZamowienia
                .Include(p => p.Towar)
                .Include(p => p.Zamowienie)
                .FirstOrDefaultAsync(m => m.IdPozycjiZamowienia == id);

            if (pozycjaZamowienia == null)
            {
                return NotFound();
            }

            return View(pozycjaZamowienia);
        }

        // GET: PozycjaZamowienia/Create
        public IActionResult Create()
        {
            ViewData["IdTowaru"] = new SelectList(
                _context.Towar.OrderBy(t => t.Nazwa),
                "IdTowaru",
                "Nazwa"
            );

            ViewData["IdZamowienia"] = new SelectList(
                _context.Zamowienie.OrderBy(z => z.NumerZamowienia),
                "IdZamowienia",
                "NumerZamowienia"
            );

            return View();
        }

        // POST: PozycjaZamowienia/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("IdPozycjiZamowienia,Ilosc,CenaJednostkowa,IdZamowienia,IdTowaru")] PozycjaZamowienia pozycjaZamowienia)
        {
            if (ModelState.IsValid)
            {
                pozycjaZamowienia.CenaJednostkowa = decimal.Round(pozycjaZamowienia.CenaJednostkowa, 2, MidpointRounding.AwayFromZero);
                _context.Add(pozycjaZamowienia);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            ViewData["IdTowaru"] = new SelectList(
                _context.Towar.OrderBy(t => t.Nazwa),
                "IdTowaru",
                "Nazwa",
                pozycjaZamowienia.IdTowaru
            );

            ViewData["IdZamowienia"] = new SelectList(
                _context.Zamowienie.OrderBy(z => z.NumerZamowienia),
                "IdZamowienia",
                "NumerZamowienia",
                pozycjaZamowienia.IdZamowienia
            );

            return View(pozycjaZamowienia);
        }

        // GET: PozycjaZamowienia/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var pozycjaZamowienia = await _context.PozycjaZamowienia.FindAsync(id);

            if (pozycjaZamowienia == null)
            {
                return NotFound();
            }

            ViewData["IdTowaru"] = new SelectList(
                _context.Towar.OrderBy(t => t.Nazwa),
                "IdTowaru",
                "Nazwa",
                pozycjaZamowienia.IdTowaru
            );

            ViewData["IdZamowienia"] = new SelectList(
                _context.Zamowienie.OrderBy(z => z.NumerZamowienia),
                "IdZamowienia",
                "NumerZamowienia",
                pozycjaZamowienia.IdZamowienia
            );

            return View(pozycjaZamowienia);
        }

        // POST: PozycjaZamowienia/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("IdPozycjiZamowienia,Ilosc,CenaJednostkowa,IdZamowienia,IdTowaru")] PozycjaZamowienia pozycjaZamowienia)
        {
            if (id != pozycjaZamowienia.IdPozycjiZamowienia)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    pozycjaZamowienia.CenaJednostkowa = decimal.Round(pozycjaZamowienia.CenaJednostkowa, 2, MidpointRounding.AwayFromZero);
                    _context.Update(pozycjaZamowienia);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PozycjaZamowieniaExists(pozycjaZamowienia.IdPozycjiZamowienia))
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

            ViewData["IdTowaru"] = new SelectList(
                _context.Towar.OrderBy(t => t.Nazwa),
                "IdTowaru",
                "Nazwa",
                pozycjaZamowienia.IdTowaru
            );

            ViewData["IdZamowienia"] = new SelectList(
                _context.Zamowienie.OrderBy(z => z.NumerZamowienia),
                "IdZamowienia",
                "NumerZamowienia",
                pozycjaZamowienia.IdZamowienia
            );

            return View(pozycjaZamowienia);
        }

        // GET: PozycjaZamowienia/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var pozycjaZamowienia = await _context.PozycjaZamowienia
                .Include(p => p.Towar)
                .Include(p => p.Zamowienie)
                .FirstOrDefaultAsync(m => m.IdPozycjiZamowienia == id);

            if (pozycjaZamowienia == null)
            {
                return NotFound();
            }

            return View(pozycjaZamowienia);
        }

        // POST: PozycjaZamowienia/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var pozycjaZamowienia = await _context.PozycjaZamowienia.FindAsync(id);

            if (pozycjaZamowienia != null)
            {
                _context.PozycjaZamowienia.Remove(pozycjaZamowienia);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool PozycjaZamowieniaExists(int id)
        {
            return _context.PozycjaZamowienia.Any(e => e.IdPozycjiZamowienia == id);
        }
    }
}
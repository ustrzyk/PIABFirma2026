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
    public class ZamowienieController : Controller
    {
        private readonly FirmaContext _context;

        public ZamowienieController(FirmaContext context)
        {
            _context = context;
        }

        // GET: Zamowienie
        public async Task<IActionResult> Index()
        {
            var firmaContext = _context.Zamowienie
                .Include(z => z.Klient);

            return View(await firmaContext.ToListAsync());
        }

        // GET: Zamowienie/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var zamowienie = await _context.Zamowienie
                .Include(z => z.Klient)
                .FirstOrDefaultAsync(m => m.IdZamowienia == id);

            if (zamowienie == null)
            {
                return NotFound();
            }

            return View(zamowienie);
        }

        // GET: Zamowienie/Create
        public IActionResult Create()
        {
            ViewData["IdKlienta"] = new SelectList(
                _context.Klient.OrderBy(k => k.Email),
                "IdKlienta",
                "Email"
            );

            return View();
        }

        // POST: Zamowienie/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("IdZamowienia,NumerZamowienia,DataZamowienia,Status,WartoscRazem,Ulica,NumerDomu,NumerLokalu,KodPocztowy,Miasto,IdKlienta")] Zamowienie zamowienie)
        {
            if (ModelState.IsValid)
            {
                zamowienie.WartoscRazem = decimal.Round(zamowienie.WartoscRazem, 2, MidpointRounding.AwayFromZero);
                _context.Add(zamowienie);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            ViewData["IdKlienta"] = new SelectList(
                _context.Klient.OrderBy(k => k.Email),
                "IdKlienta",
                "Email",
                zamowienie.IdKlienta
            );

            return View(zamowienie);
        }

        // GET: Zamowienie/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var zamowienie = await _context.Zamowienie.FindAsync(id);

            if (zamowienie == null)
            {
                return NotFound();
            }

            ViewData["IdKlienta"] = new SelectList(
                _context.Klient.OrderBy(k => k.Email),
                "IdKlienta",
                "Email",
                zamowienie.IdKlienta
            );

            return View(zamowienie);
        }

        // POST: Zamowienie/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("IdZamowienia,NumerZamowienia,DataZamowienia,Status,WartoscRazem,Ulica,NumerDomu,NumerLokalu,KodPocztowy,Miasto,IdKlienta")] Zamowienie zamowienie)
        {
            if (id != zamowienie.IdZamowienia)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    zamowienie.WartoscRazem = decimal.Round(zamowienie.WartoscRazem, 2, MidpointRounding.AwayFromZero);
                    _context.Update(zamowienie);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ZamowienieExists(zamowienie.IdZamowienia))
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

            ViewData["IdKlienta"] = new SelectList(
                _context.Klient.OrderBy(k => k.Email),
                "IdKlienta",
                "Email",
                zamowienie.IdKlienta
            );

            return View(zamowienie);
        }

        // GET: Zamowienie/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var zamowienie = await _context.Zamowienie
                .Include(z => z.Klient)
                .FirstOrDefaultAsync(m => m.IdZamowienia == id);

            if (zamowienie == null)
            {
                return NotFound();
            }

            return View(zamowienie);
        }

        // POST: Zamowienie/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var zamowienie = await _context.Zamowienie.FindAsync(id);

            if (zamowienie != null)
            {
                _context.Zamowienie.Remove(zamowienie);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ZamowienieExists(int id)
        {
            return _context.Zamowienie.Any(e => e.IdZamowienia == id);
        }
    }
}
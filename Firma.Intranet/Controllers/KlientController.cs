using Firma.Data.Data.Sklep;
using Firma.Intranet.Interfaces.Intranet;
using Microsoft.AspNetCore.Mvc;

namespace Firma.Intranet.Controllers
{
    public class KlientController : Controller
    {
        private readonly IKlientIntranetService _klientIntranetService;

        public KlientController(IKlientIntranetService klientIntranetService)
        {
            _klientIntranetService = klientIntranetService;
        }

        public async Task<IActionResult> Index()
        {
            var klienci = await _klientIntranetService.PobierzListe();

            return View(klienci);
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var klient = await _klientIntranetService.PobierzSzczegoly(id.Value);

            if (klient == null)
            {
                return NotFound();
            }

            return View(klient);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("IdKlienta,Imie,Nazwisko,Email,Telefon")] Klient klient)
        {
            if (ModelState.IsValid)
            {
                await _klientIntranetService.Dodaj(klient);

                return RedirectToAction(nameof(Index));
            }

            return View(klient);
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var klient = await _klientIntranetService.PobierzDoEdycji(id.Value);

            if (klient == null)
            {
                return NotFound();
            }

            return View(klient);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("IdKlienta,Imie,Nazwisko,Email,Telefon")] Klient klient)
        {
            if (id != klient.IdKlienta)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                var zapisano = await _klientIntranetService.Aktualizuj(id, klient);

                if (!zapisano)
                {
                    return NotFound();
                }

                return RedirectToAction(nameof(Index));
            }

            return View(klient);
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var klient = await _klientIntranetService.PobierzDoUsuniecia(id.Value);

            if (klient == null)
            {
                return NotFound();
            }

            return View(klient);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var usunieto = await _klientIntranetService.Usun(id);

            if (!usunieto)
            {
                var klient = await _klientIntranetService.PobierzDoUsuniecia(id);

                if (klient == null)
                {
                    return NotFound();
                }

                ModelState.AddModelError(
                    string.Empty,
                    "Nie można usunąć klienta, który ma przypisane zamówienia.");

                return View("Delete", klient);
            }

            return RedirectToAction(nameof(Index));
        }
    }
}
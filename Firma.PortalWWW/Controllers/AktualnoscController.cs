using Firma.Interfaces.CMS;
using Microsoft.AspNetCore.Mvc;

namespace Firma.PortalWWW.Controllers
{
    public class AktualnoscController : PortalControllerBase
    {
        public AktualnoscController(
            IAktualnoscService aktualnoscService,
            IStronaService stronaService)
            : base(stronaService, aktualnoscService)
        {
        }

        public async Task<IActionResult> Index(int id)
        {
            await PrzygotujDaneDoLayoutu();

            // Pobieram jedną aktualność
            var item = await _aktualnoscService.GetAktualnosc(id);

            if (item == null)
            {
                return NotFound();
            }

            return View(item);
        }
    }
}
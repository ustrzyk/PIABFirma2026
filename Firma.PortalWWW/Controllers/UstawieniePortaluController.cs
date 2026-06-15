using Firma.Interfaces.CMS;
using Microsoft.AspNetCore.Mvc;

namespace Firma.PortalWWW.Controllers
{
    public class UstawieniePortaluController : PortalControllerBase
    {
        public UstawieniePortaluController(
            IUstawieniePortaluService ustawieniePortaluService,
            IStronaService stronaService,
            IAktualnoscService aktualnoscService)
            : base(stronaService, aktualnoscService, ustawieniePortaluService)
        {
        }

        public async Task<IActionResult> Index()
        {
            await PrzygotujDaneDoLayoutu();

            // Pobieram ustawienia portalu
            var items = await _ustawieniePortaluService.GetUstawieniaPortalu();

            return View(items);
        }

        public async Task<IActionResult> Szczegoly(int id)
        {
            await PrzygotujDaneDoLayoutu();

            // Pobieram szczegóły ustawienia
            var item = await _ustawieniePortaluService.GetUstawieniePortalu(id);

            if (item == null)
            {
                return NotFound();
            }

            return View(item);
        }
    }
}
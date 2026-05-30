using Firma.Interfaces.CMS;
using Firma.Interfaces.Sklep;
using Microsoft.AspNetCore.Mvc;

namespace Firma.PortalWWW.Controllers
{
    public class SklepController : PortalControllerBase
    {
        private readonly ITowarService _towarService;

        public SklepController(
            ITowarService towarService,
            IStronaService stronaService,
            IAktualnoscService aktualnoscService)
            : base(stronaService, aktualnoscService)
        {
            _towarService = towarService;
        }

        public async Task<IActionResult> Index(int? id)
        {
            await PrzygotujDaneDoLayoutu();

            // Pobieram towary dla wybranej kategorii
            var items = await _towarService.GetTowaryDanegoRodzaju(id);

            return View(items);
        }

        public async Task<IActionResult> Szczegoly(int id)
        {
            await PrzygotujDaneDoLayoutu();

            // Pobieram szczegóły towaru
            var item = await _towarService.GetTowar(id);

            if (item == null)
            {
                return NotFound();
            }

            return View(item);
        }
    }
}
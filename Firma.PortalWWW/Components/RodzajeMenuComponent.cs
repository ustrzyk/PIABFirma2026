using Firma.Interfaces.Sklep;
using Microsoft.AspNetCore.Mvc;

namespace Firma.PortalWWW.Components
{
    public class RodzajeMenuComponent : ViewComponent
    {
        private readonly IRodzajService _rodzajService;

        public RodzajeMenuComponent(IRodzajService rodzajService)
        {
            _rodzajService = rodzajService;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var rodzaje = await _rodzajService.GetRodzaje();

            return View(rodzaje);
        }
    }
}
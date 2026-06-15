using Firma.Interfaces.CMS;
using Microsoft.AspNetCore.Mvc;

namespace Firma.PortalWWW.Controllers
{
    public abstract class PortalControllerBase : Controller
    {
        protected readonly IStronaService _stronaService;
        protected readonly IAktualnoscService _aktualnoscService;
        protected readonly IUstawieniePortaluService _ustawieniePortaluService;

        protected PortalControllerBase(
            IStronaService stronaService,
            IAktualnoscService aktualnoscService,
            IUstawieniePortaluService ustawieniePortaluService)
        {
            _stronaService = stronaService;
            _aktualnoscService = aktualnoscService;
            _ustawieniePortaluService = ustawieniePortaluService;
        }

        protected async Task PrzygotujDaneDoLayoutu()
        {
            // Pobieram strony do menu
            ViewBag.ModelStrony = await _stronaService.GetStronyByPozycja();

            // Pobieram aktualności do stopki
            ViewBag.ModelAktualnosci = await _aktualnoscService.GetAktualnoscByPozycjaTake(3);

            // Pobieram wygląd portalu
            ViewBag.WygladPortalu = await _ustawieniePortaluService.GetWygladPortalu();
        }
    }
}
using Firma.Interfaces.CMS;
using Microsoft.AspNetCore.Mvc;

namespace Firma.PortalWWW.Controllers
{
    public abstract class PortalControllerBase : Controller
    {
        protected readonly IStronaService _stronaService;
        protected readonly IAktualnoscService _aktualnoscService;

        protected PortalControllerBase(
            IStronaService stronaService,
            IAktualnoscService aktualnoscService)
        {
            _stronaService = stronaService;
            _aktualnoscService = aktualnoscService;
        }

        protected async Task PrzygotujDaneDoLayoutu()
        {
            // Pobieram strony do menu
            ViewBag.ModelStrony = await _stronaService.GetStronyByPozycja();

            // Pobieram aktualności do stopki
            ViewBag.ModelAktualnosci = await _aktualnoscService.GetAktualnoscByPozycjaTake(3);
        }
    }
}
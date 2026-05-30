using Firma.Interfaces.CMS;
using Firma.PortalWWW.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace Firma.PortalWWW.Controllers
{
    public class HomeController : PortalControllerBase
    {
        public HomeController(
            IStronaService stronaService,
            IAktualnoscService aktualnoscService)
            : base(stronaService, aktualnoscService)
        {
        }

        public async Task<IActionResult> Index(int? id)
        {
            await PrzygotujDaneDoLayoutu();

            // Pobieram stronę główną lub wybraną stronę
            var item = await _stronaService.GetStrona(id);

            return View(item);
        }

        public async Task<IActionResult> Privacy()
        {
            await PrzygotujDaneDoLayoutu();

            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel
            {
                RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier
            });
        }
    }
}
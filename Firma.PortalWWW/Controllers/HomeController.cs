using Firma.Interfaces.CMS;
using Firma.PortalWWW.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace Firma.PortalWWW.Controllers
{
    public class HomeController : Controller
    {
        private readonly IStronaService _stronaService;
        private readonly IAktualnoscService _aktualnoscService;

        public HomeController(
            IStronaService stronaService,
            IAktualnoscService aktualnoscService)
        {
            _stronaService = stronaService;
            _aktualnoscService = aktualnoscService;
        }

        public async Task<IActionResult> Index(int? id)
        {
            ViewBag.ModelStrony = await _stronaService.GetStronyByPozycja();
            ViewBag.ModelAktualnosci = await _aktualnoscService.GetAktualnoscByPozycjaTake(3);

            var item = await _stronaService.GetStrona(id);

            return View(item);
        }

        public async Task<IActionResult> Privacy()
        {
            ViewBag.ModelStrony = await _stronaService.GetStronyByPozycja();
            ViewBag.ModelAktualnosci = await _aktualnoscService.GetAktualnoscByPozycjaTake(3);

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
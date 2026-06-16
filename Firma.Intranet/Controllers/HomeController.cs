using Firma.Intranet.Interfaces.Intranet;
using Firma.Intranet.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace Firma.Intranet.Controllers
{
    public class HomeController : Controller
    {
        private readonly IZamowienieIntranetService _zamowienieIntranetService;
        private readonly IStanMagazynowyIntranetService _stanMagazynowyIntranetService;

        public HomeController(
            IZamowienieIntranetService zamowienieIntranetService,
            IStanMagazynowyIntranetService stanMagazynowyIntranetService)
        {
            _zamowienieIntranetService = zamowienieIntranetService;
            _stanMagazynowyIntranetService = stanMagazynowyIntranetService;
        }

        public async Task<IActionResult> Index()
        {
            ViewBag.WszystkieZamowienia = await _zamowienieIntranetService.PoliczWszystkieZamowienia();
            ViewBag.ZamowieniaWWW = await _zamowienieIntranetService.PoliczZamowieniaWWW();
            ViewBag.NoweZamowieniaWWW = await _zamowienieIntranetService.PoliczNoweZamowieniaWWW();
            ViewBag.ZamowieniaDoObslugi = await _zamowienieIntranetService.PoliczZamowieniaDoObslugi();

            ViewBag.WszystkieStany = await _stanMagazynowyIntranetService.PoliczWszystkieStany();
            ViewBag.AktywneStany = await _stanMagazynowyIntranetService.PoliczAktywneStany();
            ViewBag.NiskieStany = await _stanMagazynowyIntranetService.PoliczNiskieStany();

            return View();
        }

        public IActionResult Privacy()
        {
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
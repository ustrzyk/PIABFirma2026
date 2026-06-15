using Firma.Intranet.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Firma.Intranet.Controllers
{
    public class KontoController : Controller
    {
        private readonly SignInManager<IdentityUser> _signInManager;

        public KontoController(SignInManager<IdentityUser> signInManager)
        {
            _signInManager = signInManager;
        }

        [AllowAnonymous]
        public IActionResult Logowanie(string? returnUrl = null)
        {
            if (User.Identity != null && User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Index", "Home");
            }

            var model = new LogowanieViewModel
            {
                ReturnUrl = returnUrl
            };

            return View(model);
        }

        [AllowAnonymous]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Logowanie(LogowanieViewModel model, string? returnUrl = null)
        {
            model.ReturnUrl = returnUrl ?? model.ReturnUrl;

            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var wynik = await _signInManager.PasswordSignInAsync(
                model.Email,
                model.Haslo,
                model.ZapamietajMnie,
                lockoutOnFailure: false);

            if (wynik.Succeeded)
            {
                if (!string.IsNullOrWhiteSpace(model.ReturnUrl) && Url.IsLocalUrl(model.ReturnUrl))
                {
                    return Redirect(model.ReturnUrl);
                }

                return RedirectToAction("Index", "Home");
            }

            ModelState.AddModelError(string.Empty, "Niepoprawny e-mail lub hasło");

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Wyloguj()
        {
            await _signInManager.SignOutAsync();

            return RedirectToAction(nameof(Logowanie));
        }

        [AllowAnonymous]
        public IActionResult BrakDostepu()
        {
            return View();
        }
    }
}
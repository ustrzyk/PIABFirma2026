using Firma.Intranet.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Firma.Intranet.Controllers
{
    [Authorize(Roles = "Administrator")]
    public class UzytkownikController : Controller
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public UzytkownikController(
            UserManager<IdentityUser> userManager,
            RoleManager<IdentityRole> roleManager)
        {
            _userManager = userManager;
            _roleManager = roleManager;
        }

        public async Task<IActionResult> Index()
        {
            var aktualnyUzytkownikId = _userManager.GetUserId(User);
            var uzytkownicy = await _userManager.Users
                .OrderBy(u => u.Email)
                .ToListAsync();

            var model = new List<UzytkownikListaItemViewModel>();

            foreach (var uzytkownik in uzytkownicy)
            {
                var role = await _userManager.GetRolesAsync(uzytkownik);

                model.Add(new UzytkownikListaItemViewModel
                {
                    Id = uzytkownik.Id,
                    Email = uzytkownik.Email ?? string.Empty,
                    NazwaUzytkownika = uzytkownik.UserName ?? string.Empty,
                    Role = role,
                    CzyAktualnieZalogowany = uzytkownik.Id == aktualnyUzytkownikId
                });
            }

            return View(model);
        }

        public async Task<IActionResult> Create()
        {
            var model = new UzytkownikCreateViewModel
            {
                DostepneRole = await PobierzRole()
            };

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(UzytkownikCreateViewModel model)
        {
            model.DostepneRole = await PobierzRole();

            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var uzytkownik = new IdentityUser
            {
                UserName = model.Email,
                Email = model.Email,
                EmailConfirmed = true
            };

            var wynik = await _userManager.CreateAsync(uzytkownik, model.Haslo);

            if (!wynik.Succeeded)
            {
                DodajBledyIdentity(wynik);

                return View(model);
            }

            var wynikRoli = await _userManager.AddToRoleAsync(uzytkownik, model.Rola);

            if (!wynikRoli.Succeeded)
            {
                await _userManager.DeleteAsync(uzytkownik);
                DodajBledyIdentity(wynikRoli);

                return View(model);
            }

            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Edit(string id)
        {
            var uzytkownik = await _userManager.FindByIdAsync(id);

            if (uzytkownik == null)
            {
                return NotFound();
            }

            var role = await _userManager.GetRolesAsync(uzytkownik);
            var aktualnyUzytkownikId = _userManager.GetUserId(User);

            var model = new UzytkownikEditViewModel
            {
                Id = uzytkownik.Id,
                Email = uzytkownik.Email ?? string.Empty,
                Rola = role.FirstOrDefault() ?? string.Empty,
                CzyAktualnieZalogowany = uzytkownik.Id == aktualnyUzytkownikId,
                DostepneRole = await PobierzRole()
            };

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, UzytkownikEditViewModel model)
        {
            if (id != model.Id)
            {
                return NotFound();
            }

            model.DostepneRole = await PobierzRole();

            var uzytkownik = await _userManager.FindByIdAsync(id);

            if (uzytkownik == null)
            {
                return NotFound();
            }

            var aktualnyUzytkownikId = _userManager.GetUserId(User);
            model.CzyAktualnieZalogowany = uzytkownik.Id == aktualnyUzytkownikId;

            if (model.CzyAktualnieZalogowany && model.Rola != "Administrator")
            {
                ModelState.AddModelError(string.Empty, "Nie można odebrać roli Administrator aktualnie zalogowanemu użytkownikowi");
            }

            if (!ModelState.IsValid)
            {
                return View(model);
            }

            uzytkownik.Email = model.Email;
            uzytkownik.UserName = model.Email;
            uzytkownik.EmailConfirmed = true;

            var wynikAktualizacji = await _userManager.UpdateAsync(uzytkownik);

            if (!wynikAktualizacji.Succeeded)
            {
                DodajBledyIdentity(wynikAktualizacji);

                return View(model);
            }

            var aktualneRole = await _userManager.GetRolesAsync(uzytkownik);

            if (aktualneRole.Any())
            {
                var wynikUsunieciaRol = await _userManager.RemoveFromRolesAsync(uzytkownik, aktualneRole);

                if (!wynikUsunieciaRol.Succeeded)
                {
                    DodajBledyIdentity(wynikUsunieciaRol);

                    return View(model);
                }
            }

            var wynikDodaniaRoli = await _userManager.AddToRoleAsync(uzytkownik, model.Rola);

            if (!wynikDodaniaRoli.Succeeded)
            {
                DodajBledyIdentity(wynikDodaniaRoli);

                return View(model);
            }

            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> ResetHasla(string id)
        {
            var uzytkownik = await _userManager.FindByIdAsync(id);

            if (uzytkownik == null)
            {
                return NotFound();
            }

            var model = new ResetHaslaUzytkownikaViewModel
            {
                Id = uzytkownik.Id,
                Email = uzytkownik.Email ?? string.Empty
            };

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ResetHasla(string id, ResetHaslaUzytkownikaViewModel model)
        {
            if (id != model.Id)
            {
                return NotFound();
            }

            var uzytkownik = await _userManager.FindByIdAsync(id);

            if (uzytkownik == null)
            {
                return NotFound();
            }

            model.Email = uzytkownik.Email ?? string.Empty;

            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var token = await _userManager.GeneratePasswordResetTokenAsync(uzytkownik);
            var wynik = await _userManager.ResetPasswordAsync(uzytkownik, token, model.NoweHaslo);

            if (!wynik.Succeeded)
            {
                DodajBledyIdentity(wynik);

                return View(model);
            }

            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Delete(string id)
        {
            var uzytkownik = await _userManager.FindByIdAsync(id);

            if (uzytkownik == null)
            {
                return NotFound();
            }

            var aktualnyUzytkownikId = _userManager.GetUserId(User);
            var role = await _userManager.GetRolesAsync(uzytkownik);

            var model = new UsunUzytkownikaViewModel
            {
                Id = uzytkownik.Id,
                Email = uzytkownik.Email ?? string.Empty,
                Role = role,
                CzyAktualnieZalogowany = uzytkownik.Id == aktualnyUzytkownikId
            };

            return View(model);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            var uzytkownik = await _userManager.FindByIdAsync(id);

            if (uzytkownik == null)
            {
                return NotFound();
            }

            var aktualnyUzytkownikId = _userManager.GetUserId(User);

            if (uzytkownik.Id == aktualnyUzytkownikId)
            {
                ModelState.AddModelError(string.Empty, "Nie można usunąć aktualnie zalogowanego użytkownika");

                var role = await _userManager.GetRolesAsync(uzytkownik);

                var model = new UsunUzytkownikaViewModel
                {
                    Id = uzytkownik.Id,
                    Email = uzytkownik.Email ?? string.Empty,
                    Role = role,
                    CzyAktualnieZalogowany = true
                };

                return View(model);
            }

            var wynik = await _userManager.DeleteAsync(uzytkownik);

            if (!wynik.Succeeded)
            {
                DodajBledyIdentity(wynik);

                var role = await _userManager.GetRolesAsync(uzytkownik);

                var model = new UsunUzytkownikaViewModel
                {
                    Id = uzytkownik.Id,
                    Email = uzytkownik.Email ?? string.Empty,
                    Role = role
                };

                return View(model);
            }

            return RedirectToAction(nameof(Index));
        }

        private async Task<List<string>> PobierzRole()
        {
            await ZapewnijRole();

            return await _roleManager.Roles
                .Select(r => r.Name ?? string.Empty)
                .Where(nazwa => nazwa != string.Empty)
                .OrderBy(nazwa => nazwa)
                .ToListAsync();
        }

        private async Task ZapewnijRole()
        {
            var role = new[]
            {
                "Administrator",
                "Pracownik"
            };

            foreach (var nazwaRoli in role)
            {
                if (!await _roleManager.RoleExistsAsync(nazwaRoli))
                {
                    await _roleManager.CreateAsync(new IdentityRole(nazwaRoli));
                }
            }
        }

        private void DodajBledyIdentity(IdentityResult wynik)
        {
            foreach (var blad in wynik.Errors)
            {
                ModelState.AddModelError(string.Empty, blad.Description);
            }
        }
    }
}
using Firma.Intranet.Interfaces.Intranet;
using Firma.Intranet.Services.Data.Intranet;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Firma.Intranet.Services.Intranet
{
    public class UzytkownikIntranetService : IUzytkownikIntranetService
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public UzytkownikIntranetService(
            UserManager<IdentityUser> userManager,
            RoleManager<IdentityRole> roleManager)
        {
            _userManager = userManager;
            _roleManager = roleManager;
        }

        public async Task<List<UzytkownikListaItemDto>> PobierzListe(string? idAktualnegoUzytkownika)
        {
            await ZapewnijRole();

            var uzytkownicy = await _userManager.Users
                .OrderBy(u => u.Email)
                .ToListAsync();

            var wynik = new List<UzytkownikListaItemDto>();

            foreach (var uzytkownik in uzytkownicy)
            {
                var role = await _userManager.GetRolesAsync(uzytkownik);

                wynik.Add(new UzytkownikListaItemDto
                {
                    Id = uzytkownik.Id,
                    Email = uzytkownik.Email ?? string.Empty,
                    NazwaUzytkownika = uzytkownik.UserName ?? string.Empty,
                    Role = role,
                    CzyAktualnieZalogowany = uzytkownik.Id == idAktualnegoUzytkownika
                });
            }

            return wynik;
        }

        public async Task<List<string>> PobierzRole()
        {
            await ZapewnijRole();

            return await _roleManager.Roles
                .Select(r => r.Name ?? string.Empty)
                .Where(nazwa => nazwa != string.Empty)
                .OrderBy(nazwa => nazwa)
                .ToListAsync();
        }

        public async Task<OperacjaUzytkownikaWynikDto> Dodaj(string email, string haslo, string rola)
        {
            await ZapewnijRole();

            var przygotowanyEmail = email.Trim();

            var uzytkownik = new IdentityUser
            {
                UserName = przygotowanyEmail,
                Email = przygotowanyEmail,
                EmailConfirmed = true
            };

            var wynikUtworzenia = await _userManager.CreateAsync(uzytkownik, haslo);

            if (!wynikUtworzenia.Succeeded)
            {
                return ZamienWynikIdentity(wynikUtworzenia);
            }

            var wynikRoli = await _userManager.AddToRoleAsync(uzytkownik, rola);

            if (!wynikRoli.Succeeded)
            {
                await _userManager.DeleteAsync(uzytkownik);

                return ZamienWynikIdentity(wynikRoli);
            }

            return OperacjaUzytkownikaWynikDto.Powodzenie();
        }

        public async Task<UzytkownikEdycjaDto?> PobierzDoEdycji(string id, string? idAktualnegoUzytkownika)
        {
            await ZapewnijRole();

            var uzytkownik = await _userManager.FindByIdAsync(id);

            if (uzytkownik == null)
            {
                return null;
            }

            var role = await _userManager.GetRolesAsync(uzytkownik);

            return new UzytkownikEdycjaDto
            {
                Id = uzytkownik.Id,
                Email = uzytkownik.Email ?? string.Empty,
                Rola = role.FirstOrDefault() ?? string.Empty,
                CzyAktualnieZalogowany = uzytkownik.Id == idAktualnegoUzytkownika,
                DostepneRole = await PobierzRole()
            };
        }

        public async Task<OperacjaUzytkownikaWynikDto> Aktualizuj(
            string id,
            string email,
            string rola,
            string? idAktualnegoUzytkownika)
        {
            await ZapewnijRole();

            var uzytkownik = await _userManager.FindByIdAsync(id);

            if (uzytkownik == null)
            {
                return OperacjaUzytkownikaWynikDto.NieZnaleziono();
            }

            if (uzytkownik.Id == idAktualnegoUzytkownika && rola != "Administrator")
            {
                return OperacjaUzytkownikaWynikDto.Niepowodzenie(new[]
                {
                    "Nie można odebrać roli Administrator aktualnie zalogowanemu użytkownikowi"
                });
            }

            var przygotowanyEmail = email.Trim();

            uzytkownik.Email = przygotowanyEmail;
            uzytkownik.UserName = przygotowanyEmail;
            uzytkownik.EmailConfirmed = true;

            var wynikAktualizacji = await _userManager.UpdateAsync(uzytkownik);

            if (!wynikAktualizacji.Succeeded)
            {
                return ZamienWynikIdentity(wynikAktualizacji);
            }

            var aktualneRole = await _userManager.GetRolesAsync(uzytkownik);

            if (aktualneRole.Any())
            {
                var wynikUsunieciaRol = await _userManager.RemoveFromRolesAsync(uzytkownik, aktualneRole);

                if (!wynikUsunieciaRol.Succeeded)
                {
                    return ZamienWynikIdentity(wynikUsunieciaRol);
                }
            }

            var wynikDodaniaRoli = await _userManager.AddToRoleAsync(uzytkownik, rola);

            if (!wynikDodaniaRoli.Succeeded)
            {
                return ZamienWynikIdentity(wynikDodaniaRoli);
            }

            return OperacjaUzytkownikaWynikDto.Powodzenie();
        }

        public async Task<string?> PobierzEmail(string id)
        {
            var uzytkownik = await _userManager.FindByIdAsync(id);

            return uzytkownik?.Email;
        }

        public async Task<OperacjaUzytkownikaWynikDto> ResetujHaslo(string id, string noweHaslo)
        {
            var uzytkownik = await _userManager.FindByIdAsync(id);

            if (uzytkownik == null)
            {
                return OperacjaUzytkownikaWynikDto.NieZnaleziono();
            }

            var token = await _userManager.GeneratePasswordResetTokenAsync(uzytkownik);
            var wynik = await _userManager.ResetPasswordAsync(uzytkownik, token, noweHaslo);

            return ZamienWynikIdentity(wynik);
        }

        public async Task<UzytkownikUsuniecieDto?> PobierzDoUsuniecia(string id, string? idAktualnegoUzytkownika)
        {
            var uzytkownik = await _userManager.FindByIdAsync(id);

            if (uzytkownik == null)
            {
                return null;
            }

            var role = await _userManager.GetRolesAsync(uzytkownik);

            return new UzytkownikUsuniecieDto
            {
                Id = uzytkownik.Id,
                Email = uzytkownik.Email ?? string.Empty,
                Role = role,
                CzyAktualnieZalogowany = uzytkownik.Id == idAktualnegoUzytkownika
            };
        }

        public async Task<OperacjaUzytkownikaWynikDto> Usun(string id, string? idAktualnegoUzytkownika)
        {
            var uzytkownik = await _userManager.FindByIdAsync(id);

            if (uzytkownik == null)
            {
                return OperacjaUzytkownikaWynikDto.NieZnaleziono();
            }

            if (uzytkownik.Id == idAktualnegoUzytkownika)
            {
                return OperacjaUzytkownikaWynikDto.Niepowodzenie(new[]
                {
                    "Nie można usunąć aktualnie zalogowanego użytkownika"
                });
            }

            var wynik = await _userManager.DeleteAsync(uzytkownik);

            return ZamienWynikIdentity(wynik);
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

        private static OperacjaUzytkownikaWynikDto ZamienWynikIdentity(IdentityResult wynik)
        {
            if (wynik.Succeeded)
            {
                return OperacjaUzytkownikaWynikDto.Powodzenie();
            }

            return OperacjaUzytkownikaWynikDto.Niepowodzenie(
                wynik.Errors.Select(b => b.Description));
        }
    }
}
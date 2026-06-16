using Firma.Intranet.Interfaces.Intranet;
using Firma.Intranet.Services.Data.Intranet;
using Microsoft.AspNetCore.Identity;

namespace Firma.Intranet.Services.Intranet
{
    public class KontoIntranetService : IKontoIntranetService
    {
        private readonly SignInManager<IdentityUser> _signInManager;

        public KontoIntranetService(SignInManager<IdentityUser> signInManager)
        {
            _signInManager = signInManager;
        }

        public async Task<LogowanieWynikDto> Zaloguj(
            string email,
            string haslo,
            bool zapamietajMnie)
        {
            var przygotowanyEmail = email.Trim();

            var wynik = await _signInManager.PasswordSignInAsync(
                przygotowanyEmail,
                haslo,
                zapamietajMnie,
                lockoutOnFailure: false);

            if (wynik.Succeeded)
            {
                return LogowanieWynikDto.Sukces();
            }

            if (wynik.IsLockedOut)
            {
                return LogowanieWynikDto.Zablokowany();
            }

            return LogowanieWynikDto.Blad("Nieprawidłowy e-mail lub hasło.");
        }

        public async Task Wyloguj()
        {
            await _signInManager.SignOutAsync();
        }
    }
}
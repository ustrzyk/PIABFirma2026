using Firma.Data.Data;
using Firma.Interfaces.Sklep;
using Firma.Services.Abstrakcja;
using Firma.Services.Data.Dto.ZamowieniaPubliczne;
using Microsoft.EntityFrameworkCore;

namespace Firma.Services.Sklep
{
    public class StatusZamowieniaService : BaseService, IStatusZamowieniaService
    {
        public StatusZamowieniaService(FirmaContext context)
            : base(context)
        {
        }

        public async Task<StatusZamowieniaDto?> SprawdzStatus(string numerZamowienia, string email)
        {
            var numer = numerZamowienia.Trim().ToUpperInvariant();
            var emailKlienta = email.Trim().ToLowerInvariant();

            var zamowienie = await _context.Zamowienie
                .Include(z => z.Klient)
                .Include(z => z.PozycjaZamowienia)
                    .ThenInclude(p => p.Towar)
                .FirstOrDefaultAsync(z =>
                    z.NumerZamowienia == numer &&
                    z.Klient != null &&
                    z.Klient.Email.ToLower() == emailKlienta);

            if (zamowienie == null)
            {
                return null;
            }

            var adres = $"{zamowienie.Ulica} {zamowienie.NumerDomu}";

            if (!string.IsNullOrWhiteSpace(zamowienie.NumerLokalu))
            {
                adres += $"/{zamowienie.NumerLokalu}";
            }

            adres += $", {zamowienie.KodPocztowy} {zamowienie.Miasto}";

            return new StatusZamowieniaDto
            {
                NumerZamowienia = zamowienie.NumerZamowienia,
                DataZamowienia = zamowienie.DataZamowienia,
                Status = zamowienie.Status,
                WartoscRazem = zamowienie.WartoscRazem,
                ImieNazwisko = zamowienie.Klient != null
                    ? $"{zamowienie.Klient.Imie} {zamowienie.Klient.Nazwisko}"
                    : string.Empty,
                Email = zamowienie.Klient?.Email ?? string.Empty,
                Telefon = zamowienie.Klient?.Telefon ?? string.Empty,
                AdresDostawy = adres,
                Pozycje = zamowienie.PozycjaZamowienia
                    .OrderBy(p => p.IdPozycjiZamowienia)
                    .Select(p => new StatusZamowieniaPozycjaDto
                    {
                        KodTowaru = p.Towar != null ? p.Towar.Kod : string.Empty,
                        NazwaTowaru = p.Towar != null ? p.Towar.Nazwa : $"Towar ID: {p.IdTowaru}",
                        Ilosc = p.Ilosc,
                        CenaJednostkowa = p.CenaJednostkowa
                    })
                    .ToList()
            };
        }
    }
}
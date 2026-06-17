using Firma.Data.Data;
using Firma.Data.Data.Sklep;
using Firma.Interfaces.Sklep;
using Firma.Services.Abstrakcja;
using Firma.Services.Data.Dto.ZamowieniaPubliczne;
using Microsoft.EntityFrameworkCore;

namespace Firma.Services.Sklep
{
    public class KontoKlientaService : BaseService, IKontoKlientaService
    {
        public KontoKlientaService(FirmaContext context)
            : base(context)
        {
        }

        public async Task UtworzLubAktualizujKlienta(string email, string imie, string nazwisko, string telefon)
        {
            var emailKlienta = email.Trim().ToLowerInvariant();

            var klient = await _context.Klient
                .FirstOrDefaultAsync(k => k.Email.ToLower() == emailKlienta);

            if (klient == null)
            {
                _context.Klient.Add(new Klient
                {
                    Imie = imie.Trim(),
                    Nazwisko = nazwisko.Trim(),
                    Email = emailKlienta,
                    Telefon = telefon?.Trim() ?? string.Empty
                });

                await _context.SaveChangesAsync();

                return;
            }

            klient.Imie = imie.Trim();
            klient.Nazwisko = nazwisko.Trim();
            klient.Telefon = telefon?.Trim() ?? string.Empty;

            await _context.SaveChangesAsync();
        }

        public async Task<KontoKlientaDaneDto?> PobierzDaneKlienta(string email)
        {
            if (string.IsNullOrWhiteSpace(email))
            {
                return null;
            }

            var emailKlienta = email.Trim().ToLowerInvariant();

            return await _context.Klient
                .Where(k => k.Email.ToLower() == emailKlienta)
                .Select(k => new KontoKlientaDaneDto
                {
                    Imie = k.Imie,
                    Nazwisko = k.Nazwisko,
                    Email = k.Email,
                    Telefon = k.Telefon,
                    Ulica = k.Ulica,
                    NumerDomu = k.NumerDomu,
                    NumerLokalu = k.NumerLokalu,
                    KodPocztowy = k.KodPocztowy,
                    Miasto = k.Miasto
                })
                .FirstOrDefaultAsync();
        }

        public async Task<KontoKlientaDaneDto?> PobierzDaneKlientaPoZamowieniu(string numerZamowienia, string email)
        {
            if (string.IsNullOrWhiteSpace(numerZamowienia) || string.IsNullOrWhiteSpace(email))
            {
                return null;
            }

            var numer = numerZamowienia.Trim().ToUpperInvariant();
            var emailKlienta = email.Trim().ToLowerInvariant();

            return await _context.Zamowienie
                .Include(z => z.Klient)
                .Where(z =>
                    z.NumerZamowienia == numer &&
                    z.Klient != null &&
                    z.Klient.Email.ToLower() == emailKlienta)
                .Select(z => new KontoKlientaDaneDto
                {
                    Imie = z.Klient != null ? z.Klient.Imie : string.Empty,
                    Nazwisko = z.Klient != null ? z.Klient.Nazwisko : string.Empty,
                    Email = z.Klient != null ? z.Klient.Email : emailKlienta,
                    Telefon = z.Klient != null ? z.Klient.Telefon : string.Empty,
                    Ulica = z.Ulica,
                    NumerDomu = z.NumerDomu,
                    NumerLokalu = z.NumerLokalu,
                    KodPocztowy = z.KodPocztowy,
                    Miasto = z.Miasto
                })
                .FirstOrDefaultAsync();
        }

        public async Task AktualizujDaneKlienta(KontoKlientaDaneDto daneKlienta)
        {
            var emailKlienta = daneKlienta.Email.Trim().ToLowerInvariant();

            var klient = await _context.Klient
                .FirstOrDefaultAsync(k => k.Email.ToLower() == emailKlienta);

            if (klient == null)
            {
                _context.Klient.Add(new Klient
                {
                    Imie = daneKlienta.Imie.Trim(),
                    Nazwisko = daneKlienta.Nazwisko.Trim(),
                    Email = emailKlienta,
                    Telefon = daneKlienta.Telefon?.Trim() ?? string.Empty,
                    Ulica = daneKlienta.Ulica?.Trim() ?? string.Empty,
                    NumerDomu = daneKlienta.NumerDomu?.Trim() ?? string.Empty,
                    NumerLokalu = daneKlienta.NumerLokalu?.Trim() ?? string.Empty,
                    KodPocztowy = daneKlienta.KodPocztowy?.Trim() ?? string.Empty,
                    Miasto = daneKlienta.Miasto?.Trim() ?? string.Empty
                });

                await _context.SaveChangesAsync();

                return;
            }

            klient.Imie = daneKlienta.Imie.Trim();
            klient.Nazwisko = daneKlienta.Nazwisko.Trim();
            klient.Telefon = daneKlienta.Telefon?.Trim() ?? string.Empty;
            klient.Ulica = daneKlienta.Ulica?.Trim() ?? string.Empty;
            klient.NumerDomu = daneKlienta.NumerDomu?.Trim() ?? string.Empty;
            klient.NumerLokalu = daneKlienta.NumerLokalu?.Trim() ?? string.Empty;
            klient.KodPocztowy = daneKlienta.KodPocztowy?.Trim() ?? string.Empty;
            klient.Miasto = daneKlienta.Miasto?.Trim() ?? string.Empty;

            await _context.SaveChangesAsync();
        }

        public async Task<List<ZamowienieKlientaListaItemDto>> PobierzZamowieniaKlienta(string email)
        {
            var emailKlienta = email.Trim().ToLowerInvariant();

            return await _context.Zamowienie
                .Include(z => z.Klient)
                .Include(z => z.PozycjaZamowienia)
                .Where(z =>
                    z.Klient != null &&
                    z.Klient.Email.ToLower() == emailKlienta)
                .OrderByDescending(z => z.DataZamowienia)
                .ThenByDescending(z => z.IdZamowienia)
                .Select(z => new ZamowienieKlientaListaItemDto
                {
                    IdZamowienia = z.IdZamowienia,
                    NumerZamowienia = z.NumerZamowienia,
                    DataZamowienia = z.DataZamowienia,
                    Status = z.Status,
                    WartoscRazem = z.WartoscRazem,
                    LiczbaPozycji = z.PozycjaZamowienia.Count
                })
                .ToListAsync();
        }

        public async Task<StatusZamowieniaDto?> PobierzSzczegolyZamowieniaKlienta(string email, int idZamowienia)
        {
            var emailKlienta = email.Trim().ToLowerInvariant();

            var zamowienie = await _context.Zamowienie
                .Include(z => z.Klient)
                .Include(z => z.PozycjaZamowienia)
                    .ThenInclude(p => p.Towar)
                .FirstOrDefaultAsync(z =>
                    z.IdZamowienia == idZamowienia &&
                    z.Klient != null &&
                    z.Klient.Email.ToLower() == emailKlienta);

            if (zamowienie == null)
            {
                return null;
            }

            return ZamienNaDto(zamowienie);
        }

        private static StatusZamowieniaDto ZamienNaDto(Zamowienie zamowienie)
        {
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
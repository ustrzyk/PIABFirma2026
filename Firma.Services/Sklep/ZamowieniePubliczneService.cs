using Firma.Data.Data;
using Firma.Data.Data.Sklep;
using Firma.Interfaces.Sklep;
using Firma.Services.Abstrakcja;
using Firma.Services.Data.Dto.ZamowieniaPubliczne;
using Microsoft.EntityFrameworkCore;

namespace Firma.Services.Sklep
{
    public class ZamowieniePubliczneService : BaseService, IZamowieniePubliczneService
    {
        public ZamowieniePubliczneService(FirmaContext context)
            : base(context)
        {
        }

        public async Task<ZamowieniePubliczneWynikDto> ZlozZamowienie(DaneZamowieniaPublicznegoDto dane)
        {
            var pozycje = dane.Pozycje
                .Where(p => p.IdTowaru > 0 && p.Ilosc > 0)
                .GroupBy(p => p.IdTowaru)
                .Select(g => new PozycjaZamowieniaPublicznegoDto
                {
                    IdTowaru = g.Key,
                    Ilosc = g.Sum(x => x.Ilosc)
                })
                .ToList();

            if (!pozycje.Any())
            {
                return ZamowieniePubliczneWynikDto.Blad("Koszyk jest pusty.");
            }

            if (pozycje.Any(p => p.Ilosc > 20))
            {
                return ZamowieniePubliczneWynikDto.Blad("Maksymalna ilość jednego produktu w zamówieniu to 20 sztuk.");
            }

            await using var transakcja = await _context.Database.BeginTransactionAsync();

            var idTowarow = pozycje.Select(p => p.IdTowaru).ToList();

            var towary = await _context.Towar
                .Include(t => t.Rodzaj)
                .Include(t => t.Producent)
                .Include(t => t.StanMagazynowy)
                .Where(t => idTowarow.Contains(t.IdTowaru))
                .ToListAsync();

            decimal wartoscRazem = 0;

            foreach (var pozycja in pozycje)
            {
                var towar = towary.FirstOrDefault(t => t.IdTowaru == pozycja.IdTowaru);

                if (towar == null ||
                    !towar.CzyAktywny ||
                    towar.Rodzaj == null ||
                    !towar.Rodzaj.CzyAktywny ||
                    towar.Producent == null ||
                    !towar.Producent.CzyAktywny)
                {
                    return ZamowieniePubliczneWynikDto.Blad("Jeden z produktów w koszyku nie jest już dostępny.");
                }

                if (towar.StanMagazynowy == null ||
                    !towar.StanMagazynowy.CzyAktywny ||
                    towar.StanMagazynowy.IloscSztuk < pozycja.Ilosc)
                {
                    return ZamowieniePubliczneWynikDto.Blad($"Produkt „{towar.Nazwa}” nie ma wystarczającej ilości w magazynie.");
                }

                wartoscRazem += towar.Cena * pozycja.Ilosc;
            }

            var email = dane.Email.Trim().ToLower();

            var klient = await _context.Klient
                .FirstOrDefaultAsync(k => k.Email.ToLower() == email);

            if (klient == null)
            {
                klient = new Klient
                {
                    Imie = dane.Imie.Trim(),
                    Nazwisko = dane.Nazwisko.Trim(),
                    Email = email,
                    Telefon = dane.Telefon.Trim()
                };
            }
            else
            {
                klient.Telefon = dane.Telefon.Trim();
            }

            var numerZamowienia = $"WWW{DateTime.Now:yyMMddHHmmssfff}";

            var zamowienie = new Zamowienie
            {
                NumerZamowienia = numerZamowienia,
                DataZamowienia = DateTime.Now,
                Status = "Nowe",
                WartoscRazem = wartoscRazem,
                Ulica = dane.Ulica.Trim(),
                NumerDomu = dane.NumerDomu.Trim(),
                NumerLokalu = dane.NumerLokalu.Trim(),
                KodPocztowy = dane.KodPocztowy.Trim(),
                Miasto = dane.Miasto.Trim(),
                Klient = klient
            };

            foreach (var pozycja in pozycje)
            {
                var towar = towary.First(t => t.IdTowaru == pozycja.IdTowaru);

                zamowienie.PozycjaZamowienia.Add(new PozycjaZamowienia
                {
                    IdTowaru = towar.IdTowaru,
                    Ilosc = pozycja.Ilosc,
                    CenaJednostkowa = towar.Cena
                });

                if (towar.StanMagazynowy != null)
                {
                    towar.StanMagazynowy.IloscSztuk -= pozycja.Ilosc;
                }
            }

            _context.Zamowienie.Add(zamowienie);

            await _context.SaveChangesAsync();
            await transakcja.CommitAsync();

            return ZamowieniePubliczneWynikDto.Sukces(numerZamowienia);
        }
    }
}
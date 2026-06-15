using ClosedXML.Excel;
using Firma.Data.Data;
using Firma.Data.Data.Sklep;
using Firma.Intranet.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System.Globalization;

namespace Firma.Intranet.Services
{
    public class ZamowienieExcelImporter
    {
        private readonly FirmaContext _context;

        public ZamowienieExcelImporter(FirmaContext context)
        {
            _context = context;
        }

        public async Task<ImportZamowienExcelViewModel> Importuj(IFormFile? plik)
        {
            var wynik = new ImportZamowienExcelViewModel
            {
                CzyWykonanoImport = true
            };

            if (plik == null || plik.Length == 0)
            {
                wynik.Bledy.Add("Wybierz plik Excel do importu");
                return wynik;
            }

            var rozszerzenie = Path.GetExtension(plik.FileName).ToLowerInvariant();

            if (rozszerzenie != ".xlsx")
            {
                wynik.Bledy.Add("Dozwolony jest tylko plik .xlsx");
                return wynik;
            }

            using var stream = new MemoryStream();
            await plik.CopyToAsync(stream);
            stream.Position = 0;

            using var workbook = new XLWorkbook(stream);

            var worksheet = workbook.Worksheets.FirstOrDefault(w => w.Name == "Import");

            if (worksheet == null)
            {
                wynik.Bledy.Add("Brak arkusza Import");
                return wynik;
            }

            var wiersze = WczytajWiersze(worksheet, wynik);

            if (wynik.Bledy.Any())
            {
                return wynik;
            }

            if (!wiersze.Any())
            {
                wynik.Bledy.Add("Arkusz Import nie zawiera żadnych zamówień");
                return wynik;
            }

            await SprawdzDuplikaty(wiersze, wynik);

            if (wynik.Bledy.Any())
            {
                return wynik;
            }

            await ZapiszZamowienia(wiersze, wynik);

            return wynik;
        }

        private static List<WierszZamowieniaExcel> WczytajWiersze(IXLWorksheet worksheet, ImportZamowienExcelViewModel wynik)
        {
            var wiersze = new List<WierszZamowieniaExcel>();
            var ostatniWiersz = worksheet.LastRowUsed()?.RowNumber() ?? 1;

            for (var numerWiersza = 2; numerWiersza <= ostatniWiersz; numerWiersza++)
            {
                var row = worksheet.Row(numerWiersza);

                if (CzyPustyWiersz(row))
                {
                    continue;
                }

                var wiersz = new WierszZamowieniaExcel
                {
                    NumerWiersza = numerWiersza,
                    NumerZamowienia = PobierzTekst(row, 1),
                    DataZamowienia = PobierzDate(row.Cell(2)),
                    Status = PobierzTekst(row, 3),
                    WartoscRazem = PobierzDecimal(row.Cell(4)),
                    EmailKlienta = PobierzTekst(row, 5),
                    ImieKlienta = PobierzTekst(row, 6),
                    NazwiskoKlienta = PobierzTekst(row, 7),
                    TelefonKlienta = PobierzTekst(row, 8),
                    Ulica = PobierzTekst(row, 9),
                    NumerDomu = PobierzTekst(row, 10),
                    NumerLokalu = PobierzTekst(row, 11),
                    KodPocztowy = PobierzTekst(row, 12),
                    Miasto = PobierzTekst(row, 13)
                };

                SprawdzWiersz(wiersz, wynik);
                wiersze.Add(wiersz);
            }

            return wiersze;
        }

        private static void SprawdzWiersz(WierszZamowieniaExcel wiersz, ImportZamowienExcelViewModel wynik)
        {
            if (string.IsNullOrWhiteSpace(wiersz.NumerZamowienia))
            {
                wynik.Bledy.Add($"Wiersz {wiersz.NumerWiersza}: brak numeru zamówienia");
            }
            else if (wiersz.NumerZamowienia.Length > 20)
            {
                wynik.Bledy.Add($"Wiersz {wiersz.NumerWiersza}: numer zamówienia może mieć maksymalnie 20 znaków");
            }

            if (wiersz.DataZamowienia == null)
            {
                wynik.Bledy.Add($"Wiersz {wiersz.NumerWiersza}: niepoprawna data zamówienia");
            }

            if (string.IsNullOrWhiteSpace(wiersz.Status))
            {
                wynik.Bledy.Add($"Wiersz {wiersz.NumerWiersza}: brak statusu");
            }
            else if (wiersz.Status.Length > 20)
            {
                wynik.Bledy.Add($"Wiersz {wiersz.NumerWiersza}: status może mieć maksymalnie 20 znaków");
            }

            if (wiersz.WartoscRazem == null)
            {
                wynik.Bledy.Add($"Wiersz {wiersz.NumerWiersza}: niepoprawna wartość zamówienia");
            }

            if (string.IsNullOrWhiteSpace(wiersz.EmailKlienta))
            {
                wynik.Bledy.Add($"Wiersz {wiersz.NumerWiersza}: brak e-maila klienta");
            }

            if (string.IsNullOrWhiteSpace(wiersz.ImieKlienta))
            {
                wynik.Bledy.Add($"Wiersz {wiersz.NumerWiersza}: brak imienia klienta");
            }
            else if (wiersz.ImieKlienta.Length > 20)
            {
                wynik.Bledy.Add($"Wiersz {wiersz.NumerWiersza}: imię klienta może mieć maksymalnie 20 znaków");
            }

            if (string.IsNullOrWhiteSpace(wiersz.NazwiskoKlienta))
            {
                wynik.Bledy.Add($"Wiersz {wiersz.NumerWiersza}: brak nazwiska klienta");
            }
            else if (wiersz.NazwiskoKlienta.Length > 30)
            {
                wynik.Bledy.Add($"Wiersz {wiersz.NumerWiersza}: nazwisko klienta może mieć maksymalnie 30 znaków");
            }

            if (!string.IsNullOrWhiteSpace(wiersz.TelefonKlienta) && wiersz.TelefonKlienta.Length > 15)
            {
                wynik.Bledy.Add($"Wiersz {wiersz.NumerWiersza}: telefon może mieć maksymalnie 15 znaków");
            }

            if (string.IsNullOrWhiteSpace(wiersz.Ulica))
            {
                wynik.Bledy.Add($"Wiersz {wiersz.NumerWiersza}: brak ulicy");
            }
            else if (wiersz.Ulica.Length > 40)
            {
                wynik.Bledy.Add($"Wiersz {wiersz.NumerWiersza}: ulica może mieć maksymalnie 40 znaków");
            }

            if (string.IsNullOrWhiteSpace(wiersz.NumerDomu))
            {
                wynik.Bledy.Add($"Wiersz {wiersz.NumerWiersza}: brak numeru domu");
            }
            else if (wiersz.NumerDomu.Length > 10)
            {
                wynik.Bledy.Add($"Wiersz {wiersz.NumerWiersza}: numer domu może mieć maksymalnie 10 znaków");
            }

            if (!string.IsNullOrWhiteSpace(wiersz.NumerLokalu) && wiersz.NumerLokalu.Length > 10)
            {
                wynik.Bledy.Add($"Wiersz {wiersz.NumerWiersza}: numer lokalu może mieć maksymalnie 10 znaków");
            }

            if (string.IsNullOrWhiteSpace(wiersz.KodPocztowy))
            {
                wynik.Bledy.Add($"Wiersz {wiersz.NumerWiersza}: brak kodu pocztowego");
            }
            else if (wiersz.KodPocztowy.Length > 10)
            {
                wynik.Bledy.Add($"Wiersz {wiersz.NumerWiersza}: kod pocztowy może mieć maksymalnie 10 znaków");
            }

            if (string.IsNullOrWhiteSpace(wiersz.Miasto))
            {
                wynik.Bledy.Add($"Wiersz {wiersz.NumerWiersza}: brak miasta");
            }
            else if (wiersz.Miasto.Length > 30)
            {
                wynik.Bledy.Add($"Wiersz {wiersz.NumerWiersza}: miasto może mieć maksymalnie 30 znaków");
            }
        }

        private async Task SprawdzDuplikaty(List<WierszZamowieniaExcel> wiersze, ImportZamowienExcelViewModel wynik)
        {
            var duplikatyWPliku = wiersze
                .GroupBy(w => w.NumerZamowienia)
                .Where(g => g.Count() > 1)
                .Select(g => g.Key)
                .ToList();

            foreach (var numer in duplikatyWPliku)
            {
                wynik.Bledy.Add($"Numer zamówienia {numer} występuje w pliku więcej niż raz");
            }

            var numery = wiersze
                .Select(w => w.NumerZamowienia)
                .Distinct()
                .ToList();

            var istniejaceNumery = await _context.Zamowienie
                .Where(z => numery.Contains(z.NumerZamowienia))
                .Select(z => z.NumerZamowienia)
                .ToListAsync();

            foreach (var numer in istniejaceNumery)
            {
                wynik.Bledy.Add($"Zamówienie o numerze {numer} już istnieje w bazie");
            }
        }

        private async Task ZapiszZamowienia(List<WierszZamowieniaExcel> wiersze, ImportZamowienExcelViewModel wynik)
        {
            var emaile = wiersze
                .Select(w => w.EmailKlienta)
                .Distinct()
                .ToList();

            var klienci = await _context.Klient
                .Where(k => emaile.Contains(k.Email))
                .ToListAsync();

            var klienciPoEmailu = klienci.ToDictionary(k => k.Email, k => k);

            foreach (var wiersz in wiersze)
            {
                if (!klienciPoEmailu.TryGetValue(wiersz.EmailKlienta, out var klient))
                {
                    klient = new Klient
                    {
                        Imie = wiersz.ImieKlienta,
                        Nazwisko = wiersz.NazwiskoKlienta,
                        Email = wiersz.EmailKlienta,
                        Telefon = wiersz.TelefonKlienta
                    };

                    _context.Klient.Add(klient);
                    klienciPoEmailu.Add(klient.Email, klient);
                    wynik.LiczbaDodanychKlientow++;
                }

                var zamowienie = new Zamowienie
                {
                    NumerZamowienia = wiersz.NumerZamowienia,
                    DataZamowienia = wiersz.DataZamowienia,
                    Status = wiersz.Status,
                    WartoscRazem = decimal.Round(wiersz.WartoscRazem!.Value, 2, MidpointRounding.AwayFromZero),
                    Ulica = wiersz.Ulica,
                    NumerDomu = wiersz.NumerDomu,
                    NumerLokalu = wiersz.NumerLokalu,
                    KodPocztowy = wiersz.KodPocztowy,
                    Miasto = wiersz.Miasto,
                    Klient = klient
                };

                _context.Zamowienie.Add(zamowienie);
                wynik.LiczbaDodanychZamowien++;
            }

            await _context.SaveChangesAsync();
        }

        private static bool CzyPustyWiersz(IXLRow row)
        {
            for (var kolumna = 1; kolumna <= 13; kolumna++)
            {
                if (!row.Cell(kolumna).IsEmpty())
                {
                    return false;
                }
            }

            return true;
        }

        private static string PobierzTekst(IXLRow row, int kolumna)
        {
            return row.Cell(kolumna).GetString().Trim();
        }

        private static DateTime? PobierzDate(IXLCell cell)
        {
            if (cell.IsEmpty())
            {
                return null;
            }

            if (cell.TryGetValue<DateTime>(out var data))
            {
                return data.Date;
            }

            var tekst = cell.GetString().Trim();

            if (DateTime.TryParse(tekst, new CultureInfo("pl-PL"), DateTimeStyles.None, out data))
            {
                return data.Date;
            }

            if (DateTime.TryParse(tekst, CultureInfo.InvariantCulture, DateTimeStyles.None, out data))
            {
                return data.Date;
            }

            return null;
        }

        private static decimal? PobierzDecimal(IXLCell cell)
        {
            if (cell.IsEmpty())
            {
                return null;
            }

            if (cell.TryGetValue<decimal>(out var wartosc))
            {
                return wartosc;
            }

            var tekst = cell.GetString()
                .Trim()
                .Replace("zł", "", StringComparison.OrdinalIgnoreCase)
                .Replace(" ", "");

            if (decimal.TryParse(tekst, NumberStyles.Any, new CultureInfo("pl-PL"), out wartosc))
            {
                return wartosc;
            }

            if (decimal.TryParse(tekst, NumberStyles.Any, CultureInfo.InvariantCulture, out wartosc))
            {
                return wartosc;
            }

            return null;
        }

        private class WierszZamowieniaExcel
        {
            public int NumerWiersza { get; set; }

            public string NumerZamowienia { get; set; } = string.Empty;

            public DateTime? DataZamowienia { get; set; }

            public string Status { get; set; } = string.Empty;

            public decimal? WartoscRazem { get; set; }

            public string EmailKlienta { get; set; } = string.Empty;

            public string ImieKlienta { get; set; } = string.Empty;

            public string NazwiskoKlienta { get; set; } = string.Empty;

            public string TelefonKlienta { get; set; } = string.Empty;

            public string Ulica { get; set; } = string.Empty;

            public string NumerDomu { get; set; } = string.Empty;

            public string NumerLokalu { get; set; } = string.Empty;

            public string KodPocztowy { get; set; } = string.Empty;

            public string Miasto { get; set; } = string.Empty;
        }
    }
}
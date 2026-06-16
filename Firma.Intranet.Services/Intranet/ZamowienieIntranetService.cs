using Firma.Data.Data;
using Firma.Data.Data.Sklep;
using Firma.Intranet.Interfaces.Intranet;
using Firma.Intranet.Services.Data.Intranet;
using Microsoft.EntityFrameworkCore;

namespace Firma.Intranet.Services.Intranet
{
    public class ZamowienieIntranetService : IZamowienieIntranetService
    {
        private readonly FirmaContext _context;

        private static readonly HashSet<string> DozwoloneStatusy = new(StringComparer.OrdinalIgnoreCase)
        {
            "Nowe",
            "w trakcie",
            "wysłane",
            "zakończone",
            "anulowane"
        };

        public ZamowienieIntranetService(FirmaContext context)
        {
            _context = context;
        }

        public async Task<List<Zamowienie>> PobierzListe(string? zrodlo = null, string? status = null)
        {
            var zapytanie = _context.Zamowienie
                .Include(z => z.Klient)
                .AsQueryable();

            if (!string.IsNullOrWhiteSpace(zrodlo))
            {
                var zrodloFiltr = zrodlo.Trim().ToLowerInvariant();

                if (zrodloFiltr == "www")
                {
                    zapytanie = zapytanie.Where(z =>
                        z.NumerZamowienia != null &&
                        z.NumerZamowienia.StartsWith("WWW"));
                }
                else if (zrodloFiltr == "intranet")
                {
                    zapytanie = zapytanie.Where(z =>
                        z.NumerZamowienia == null ||
                        !z.NumerZamowienia.StartsWith("WWW"));
                }
            }

            if (!string.IsNullOrWhiteSpace(status))
            {
                var statusFiltr = status.Trim();

                zapytanie = zapytanie.Where(z => z.Status == statusFiltr);
            }

            return await zapytanie
                .OrderByDescending(z => z.DataZamowienia)
                .ThenByDescending(z => z.IdZamowienia)
                .ToListAsync();
        }

        public async Task<Zamowienie?> PobierzSzczegoly(int id)
        {
            return await _context.Zamowienie
                .Include(z => z.Klient)
                .Include(z => z.PozycjaZamowienia)
                    .ThenInclude(p => p.Towar)
                .FirstOrDefaultAsync(z => z.IdZamowienia == id);
        }

        public async Task<Zamowienie?> PobierzDoEdycji(int id)
        {
            return await _context.Zamowienie
                .FirstOrDefaultAsync(z => z.IdZamowienia == id);
        }

        public async Task Dodaj(Zamowienie zamowienie)
        {
            zamowienie.WartoscRazem = decimal.Round(
                zamowienie.WartoscRazem,
                2,
                MidpointRounding.AwayFromZero);

            _context.Zamowienie.Add(zamowienie);

            await _context.SaveChangesAsync();
        }

        public async Task<bool> Aktualizuj(int id, Zamowienie zamowienie)
        {
            if (id != zamowienie.IdZamowienia)
            {
                return false;
            }

            zamowienie.WartoscRazem = decimal.Round(
                zamowienie.WartoscRazem,
                2,
                MidpointRounding.AwayFromZero);

            _context.Update(zamowienie);

            try
            {
                await _context.SaveChangesAsync();

                return true;
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!await CzyIstnieje(zamowienie.IdZamowienia))
                {
                    return false;
                }

                throw;
            }
        }

        public async Task<bool> ZmienStatus(int id, string status)
        {
            var nowyStatus = status.Trim();

            if (!DozwoloneStatusy.Contains(nowyStatus))
            {
                return false;
            }

            var zamowienie = await _context.Zamowienie
                .FirstOrDefaultAsync(z => z.IdZamowienia == id);

            if (zamowienie == null)
            {
                return false;
            }

            zamowienie.Status = nowyStatus;

            await _context.SaveChangesAsync();

            return true;
        }

        public async Task<Zamowienie?> PobierzDoUsuniecia(int id)
        {
            return await _context.Zamowienie
                .Include(z => z.Klient)
                .FirstOrDefaultAsync(z => z.IdZamowienia == id);
        }

        public async Task Usun(int id)
        {
            var zamowienie = await _context.Zamowienie
                .FirstOrDefaultAsync(z => z.IdZamowienia == id);

            if (zamowienie != null)
            {
                _context.Zamowienie.Remove(zamowienie);

                await _context.SaveChangesAsync();
            }
        }

        public async Task<Zamowienie?> PobierzDoDokumentow(int id)
        {
            return await ZapytanieZamowienDoDokumentow()
                .FirstOrDefaultAsync(z => z.IdZamowienia == id);
        }

        public async Task<List<Zamowienie>> PobierzWszystkieDoDokumentow()
        {
            return await ZapytanieZamowienDoDokumentow()
                .OrderByDescending(z => z.DataZamowienia)
                .ThenByDescending(z => z.IdZamowienia)
                .ToListAsync();
        }

        public async Task<List<Zamowienie>> PobierzZaznaczoneDoDokumentow(int[] ids)
        {
            if (ids == null || ids.Length == 0)
            {
                return new List<Zamowienie>();
            }

            return await ZapytanieZamowienDoDokumentow()
                .Where(z => ids.Contains(z.IdZamowienia))
                .OrderByDescending(z => z.DataZamowienia)
                .ThenByDescending(z => z.IdZamowienia)
                .ToListAsync();
        }

        public async Task<List<KlientSelectItemDto>> PobierzKlientowDoSelectList()
        {
            return await _context.Klient
                .OrderBy(k => k.Email)
                .Select(k => new KlientSelectItemDto
                {
                    IdKlienta = k.IdKlienta,
                    Email = k.Email
                })
                .ToListAsync();
        }

        public async Task<int> PoliczWszystkieZamowienia()
        {
            return await _context.Zamowienie.CountAsync();
        }

        public async Task<int> PoliczZamowieniaWWW()
        {
            return await _context.Zamowienie.CountAsync(z =>
                z.NumerZamowienia != null &&
                z.NumerZamowienia.StartsWith("WWW"));
        }

        public async Task<int> PoliczNoweZamowieniaWWW()
        {
            return await _context.Zamowienie.CountAsync(z =>
                z.NumerZamowienia != null &&
                z.NumerZamowienia.StartsWith("WWW") &&
                z.Status == "Nowe");
        }

        public async Task<int> PoliczZamowieniaDoObslugi()
        {
            return await _context.Zamowienie.CountAsync(z =>
                z.Status == "Nowe" ||
                z.Status == "w trakcie");
        }

        private IQueryable<Zamowienie> ZapytanieZamowienDoDokumentow()
        {
            return _context.Zamowienie
                .Include(z => z.Klient)
                .Include(z => z.PozycjaZamowienia)
                    .ThenInclude(p => p.Towar);
        }

        private async Task<bool> CzyIstnieje(int id)
        {
            return await _context.Zamowienie
                .AnyAsync(z => z.IdZamowienia == id);
        }
    }
}
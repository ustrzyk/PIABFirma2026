using Firma.Data.Data;
using Firma.Data.Data.Sklep;
using Firma.Intranet.Interfaces.Intranet;
using Firma.Intranet.Services.Data.Intranet;
using Microsoft.EntityFrameworkCore;

namespace Firma.Intranet.Services.Intranet
{
    public class StanMagazynowyIntranetService : IStanMagazynowyIntranetService
    {
        private readonly FirmaContext _context;

        public StanMagazynowyIntranetService(FirmaContext context)
        {
            _context = context;
        }

        public async Task<List<StanMagazynowy>> PobierzListe(bool tylkoNiskie = false)
        {
            var zapytanie = _context.StanMagazynowy
                .Include(s => s.Towar)
                .AsQueryable();

            if (tylkoNiskie)
            {
                zapytanie = zapytanie.Where(s =>
                    s.CzyAktywny &&
                    s.IloscSztuk <= s.MinimalnaIlosc);
            }

            return await zapytanie
                .OrderBy(s => s.CzyAktywny && s.IloscSztuk <= s.MinimalnaIlosc ? 0 : 1)
                .ThenBy(s => s.Towar != null ? s.Towar.Nazwa : string.Empty)
                .ToListAsync();
        }

        public async Task<StanMagazynowy?> PobierzSzczegoly(int id)
        {
            return await _context.StanMagazynowy
                .Include(s => s.Towar)
                .FirstOrDefaultAsync(s => s.IdStanuMagazynowego == id);
        }

        public async Task<StanMagazynowy?> PobierzDoEdycji(int id)
        {
            return await _context.StanMagazynowy
                .FirstOrDefaultAsync(s => s.IdStanuMagazynowego == id);
        }

        public async Task Dodaj(StanMagazynowy stanMagazynowy)
        {
            PrzygotujDaneStanu(stanMagazynowy);

            _context.StanMagazynowy.Add(stanMagazynowy);

            await _context.SaveChangesAsync();
        }

        public async Task<bool> Aktualizuj(int id, StanMagazynowy stanMagazynowy)
        {
            if (id != stanMagazynowy.IdStanuMagazynowego)
            {
                return false;
            }

            var stanZBazy = await _context.StanMagazynowy
                .FirstOrDefaultAsync(s => s.IdStanuMagazynowego == id);

            if (stanZBazy == null)
            {
                return false;
            }

            stanZBazy.IloscSztuk = stanMagazynowy.IloscSztuk;
            stanZBazy.MinimalnaIlosc = stanMagazynowy.MinimalnaIlosc;
            stanZBazy.Lokalizacja = stanMagazynowy.Lokalizacja;
            stanZBazy.CzyAktywny = stanMagazynowy.CzyAktywny;
            stanZBazy.IdTowaru = stanMagazynowy.IdTowaru;

            PrzygotujDaneStanu(stanZBazy);

            try
            {
                await _context.SaveChangesAsync();

                return true;
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!await CzyIstnieje(stanMagazynowy.IdStanuMagazynowego))
                {
                    return false;
                }

                throw;
            }
        }

        public async Task<StanMagazynowy?> PobierzDoUsuniecia(int id)
        {
            return await _context.StanMagazynowy
                .Include(s => s.Towar)
                .FirstOrDefaultAsync(s => s.IdStanuMagazynowego == id);
        }

        public async Task Usun(int id)
        {
            var stanMagazynowy = await _context.StanMagazynowy
                .FirstOrDefaultAsync(s => s.IdStanuMagazynowego == id);

            if (stanMagazynowy != null)
            {
                _context.StanMagazynowy.Remove(stanMagazynowy);

                await _context.SaveChangesAsync();
            }
        }

        public async Task Aktywuj(int id)
        {
            var stanMagazynowy = await _context.StanMagazynowy
                .FirstOrDefaultAsync(s => s.IdStanuMagazynowego == id);

            if (stanMagazynowy == null)
            {
                return;
            }

            stanMagazynowy.CzyAktywny = true;

            await _context.SaveChangesAsync();
        }

        public async Task Dezaktywuj(int id)
        {
            var stanMagazynowy = await _context.StanMagazynowy
                .FirstOrDefaultAsync(s => s.IdStanuMagazynowego == id);

            if (stanMagazynowy == null)
            {
                return;
            }

            stanMagazynowy.CzyAktywny = false;

            await _context.SaveChangesAsync();
        }

        public async Task<bool> CzyTowarMaStanMagazynowy(int idTowaru, int? idStanuDoPominiecia = null)
        {
            var zapytanie = _context.StanMagazynowy
                .Where(s => s.IdTowaru == idTowaru);

            if (idStanuDoPominiecia.HasValue)
            {
                zapytanie = zapytanie
                    .Where(s => s.IdStanuMagazynowego != idStanuDoPominiecia.Value);
            }

            return await zapytanie.AnyAsync();
        }

        public async Task<List<TowarSelectItemDto>> PobierzTowaryDoSelectList(int? idAktualnegoTowaru = null)
        {
            return await _context.Towar
                .Where(t => t.CzyAktywny
                    || (idAktualnegoTowaru.HasValue && t.IdTowaru == idAktualnegoTowaru.Value))
                .OrderBy(t => t.Nazwa)
                .Select(t => new TowarSelectItemDto
                {
                    IdTowaru = t.IdTowaru,
                    Nazwa = t.Nazwa
                })
                .ToListAsync();
        }

        public async Task<int> PoliczWszystkieStany()
        {
            return await _context.StanMagazynowy.CountAsync();
        }

        public async Task<int> PoliczAktywneStany()
        {
            return await _context.StanMagazynowy
                .CountAsync(s => s.CzyAktywny);
        }

        public async Task<int> PoliczNiskieStany()
        {
            return await _context.StanMagazynowy
                .CountAsync(s =>
                    s.CzyAktywny &&
                    s.IloscSztuk <= s.MinimalnaIlosc);
        }

        private static void PrzygotujDaneStanu(StanMagazynowy stanMagazynowy)
        {
            stanMagazynowy.Lokalizacja = stanMagazynowy.Lokalizacja?.Trim() ?? string.Empty;
        }

        private async Task<bool> CzyIstnieje(int id)
        {
            return await _context.StanMagazynowy
                .AnyAsync(s => s.IdStanuMagazynowego == id);
        }
    }
}
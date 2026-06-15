using Firma.Data.Data;
using Firma.Data.Data.Sklep;
using Firma.Intranet.Interfaces.Intranet;
using Microsoft.EntityFrameworkCore;

namespace Firma.Intranet.Services.Intranet
{
    public class RodzajIntranetService : IRodzajIntranetService
    {
        private readonly FirmaContext _context;

        public RodzajIntranetService(FirmaContext context)
        {
            _context = context;
        }

        public async Task<List<Rodzaj>> PobierzListe()
        {
            return await _context.Rodzaj
                .OrderBy(r => r.Nazwa)
                .ToListAsync();
        }

        public async Task<Rodzaj?> PobierzSzczegoly(int id)
        {
            return await _context.Rodzaj
                .Include(r => r.Towar)
                .FirstOrDefaultAsync(r => r.IdRodzaju == id);
        }

        public async Task<Rodzaj?> PobierzDoEdycji(int id)
        {
            return await _context.Rodzaj
                .FirstOrDefaultAsync(r => r.IdRodzaju == id);
        }

        public async Task Dodaj(Rodzaj rodzaj)
        {
            PrzygotujDaneRodzaju(rodzaj);

            _context.Rodzaj.Add(rodzaj);

            await _context.SaveChangesAsync();
        }

        public async Task<bool> Aktualizuj(int id, Rodzaj rodzaj)
        {
            if (id != rodzaj.IdRodzaju)
            {
                return false;
            }

            var rodzajZBazy = await _context.Rodzaj
                .FirstOrDefaultAsync(r => r.IdRodzaju == id);

            if (rodzajZBazy == null)
            {
                return false;
            }

            rodzajZBazy.Nazwa = rodzaj.Nazwa;
            rodzajZBazy.Opis = rodzaj.Opis;
            rodzajZBazy.CzyAktywny = rodzaj.CzyAktywny;

            PrzygotujDaneRodzaju(rodzajZBazy);

            try
            {
                await _context.SaveChangesAsync();

                return true;
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!await CzyIstnieje(rodzaj.IdRodzaju))
                {
                    return false;
                }

                throw;
            }
        }

        public async Task<Rodzaj?> PobierzDoUsuniecia(int id)
        {
            return await _context.Rodzaj
                .Include(r => r.Towar)
                .FirstOrDefaultAsync(r => r.IdRodzaju == id);
        }

        public async Task UsunAlboDezaktywuj(int id)
        {
            var rodzaj = await _context.Rodzaj
                .Include(r => r.Towar)
                .FirstOrDefaultAsync(r => r.IdRodzaju == id);

            if (rodzaj == null)
            {
                return;
            }

            if (rodzaj.Towar.Any())
            {
                rodzaj.CzyAktywny = false;

                await _context.SaveChangesAsync();

                return;
            }

            _context.Rodzaj.Remove(rodzaj);

            await _context.SaveChangesAsync();
        }

        public async Task Aktywuj(int id)
        {
            var rodzaj = await _context.Rodzaj
                .FirstOrDefaultAsync(r => r.IdRodzaju == id);

            if (rodzaj == null)
            {
                return;
            }

            rodzaj.CzyAktywny = true;

            await _context.SaveChangesAsync();
        }

        public async Task Dezaktywuj(int id)
        {
            var rodzaj = await _context.Rodzaj
                .FirstOrDefaultAsync(r => r.IdRodzaju == id);

            if (rodzaj == null)
            {
                return;
            }

            rodzaj.CzyAktywny = false;

            await _context.SaveChangesAsync();
        }

        private static void PrzygotujDaneRodzaju(Rodzaj rodzaj)
        {
            rodzaj.Nazwa = rodzaj.Nazwa.Trim();
            rodzaj.Opis = rodzaj.Opis?.Trim() ?? string.Empty;
        }

        private async Task<bool> CzyIstnieje(int id)
        {
            return await _context.Rodzaj
                .AnyAsync(r => r.IdRodzaju == id);
        }
    }
}
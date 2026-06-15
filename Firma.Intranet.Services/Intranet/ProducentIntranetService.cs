using Firma.Data.Data;
using Firma.Data.Data.Sklep;
using Firma.Intranet.Interfaces.Intranet;
using Microsoft.EntityFrameworkCore;

namespace Firma.Intranet.Services.Intranet
{
    public class ProducentIntranetService : IProducentIntranetService
    {
        private readonly FirmaContext _context;

        public ProducentIntranetService(FirmaContext context)
        {
            _context = context;
        }

        public async Task<List<Producent>> PobierzListe()
        {
            return await _context.Producent
                .OrderBy(p => p.Nazwa)
                .ToListAsync();
        }

        public async Task<Producent?> PobierzSzczegoly(int id)
        {
            return await _context.Producent
                .Include(p => p.Towar)
                .FirstOrDefaultAsync(p => p.IdProducenta == id);
        }

        public async Task<Producent?> PobierzDoEdycji(int id)
        {
            return await _context.Producent
                .FirstOrDefaultAsync(p => p.IdProducenta == id);
        }

        public async Task Dodaj(Producent producent)
        {
            PrzygotujDaneProducenta(producent);

            _context.Producent.Add(producent);

            await _context.SaveChangesAsync();
        }

        public async Task<bool> Aktualizuj(int id, Producent producent)
        {
            if (id != producent.IdProducenta)
            {
                return false;
            }

            var producentZBazy = await _context.Producent
                .FirstOrDefaultAsync(p => p.IdProducenta == id);

            if (producentZBazy == null)
            {
                return false;
            }

            producentZBazy.Nazwa = producent.Nazwa;
            producentZBazy.Kraj = producent.Kraj;
            producentZBazy.StronaWWW = producent.StronaWWW;
            producentZBazy.Opis = producent.Opis;
            producentZBazy.CzyAktywny = producent.CzyAktywny;

            PrzygotujDaneProducenta(producentZBazy);

            try
            {
                await _context.SaveChangesAsync();

                return true;
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!await CzyIstnieje(producent.IdProducenta))
                {
                    return false;
                }

                throw;
            }
        }

        public async Task<Producent?> PobierzDoUsuniecia(int id)
        {
            return await _context.Producent
                .Include(p => p.Towar)
                .FirstOrDefaultAsync(p => p.IdProducenta == id);
        }

        public async Task UsunAlboDezaktywuj(int id)
        {
            var producent = await _context.Producent
                .Include(p => p.Towar)
                .FirstOrDefaultAsync(p => p.IdProducenta == id);

            if (producent == null)
            {
                return;
            }

            if (producent.Towar.Any())
            {
                producent.CzyAktywny = false;

                await _context.SaveChangesAsync();

                return;
            }

            _context.Producent.Remove(producent);

            await _context.SaveChangesAsync();
        }

        private static void PrzygotujDaneProducenta(Producent producent)
        {
            producent.Nazwa = producent.Nazwa.Trim();
            producent.Kraj = producent.Kraj?.Trim() ?? string.Empty;
            producent.StronaWWW = producent.StronaWWW?.Trim() ?? string.Empty;
            producent.Opis = producent.Opis?.Trim() ?? string.Empty;
        }

        private async Task<bool> CzyIstnieje(int id)
        {
            return await _context.Producent
                .AnyAsync(p => p.IdProducenta == id);
        }
    }
}
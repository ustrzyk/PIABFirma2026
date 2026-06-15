using Firma.Data.Data;
using Firma.Data.Data.CMS;
using Firma.Intranet.Interfaces.Intranet;
using Microsoft.EntityFrameworkCore;

namespace Firma.Intranet.Services.Intranet
{
    public class PromocjaIntranetService : IPromocjaIntranetService
    {
        private readonly FirmaContext _context;

        public PromocjaIntranetService(FirmaContext context)
        {
            _context = context;
        }

        public async Task<List<Promocja>> PobierzListe()
        {
            return await _context.Promocja
                .OrderByDescending(p => p.DataOd)
                .ThenBy(p => p.Tytul)
                .ToListAsync();
        }

        public async Task<Promocja?> PobierzSzczegoly(int id)
        {
            return await _context.Promocja
                .FirstOrDefaultAsync(p => p.IdPromocji == id);
        }

        public async Task<Promocja?> PobierzDoEdycji(int id)
        {
            return await _context.Promocja
                .FirstOrDefaultAsync(p => p.IdPromocji == id);
        }

        public async Task Dodaj(Promocja promocja)
        {
            PrzygotujDanePromocji(promocja);

            _context.Promocja.Add(promocja);

            await _context.SaveChangesAsync();
        }

        public async Task<bool> Aktualizuj(int id, Promocja promocja)
        {
            if (id != promocja.IdPromocji)
            {
                return false;
            }

            var promocjaZBazy = await _context.Promocja
                .FirstOrDefaultAsync(p => p.IdPromocji == id);

            if (promocjaZBazy == null)
            {
                return false;
            }

            promocjaZBazy.Tytul = promocja.Tytul;
            promocjaZBazy.Opis = promocja.Opis;
            promocjaZBazy.RabatProcentowy = promocja.RabatProcentowy;
            promocjaZBazy.DataOd = promocja.DataOd;
            promocjaZBazy.DataDo = promocja.DataDo;
            promocjaZBazy.CzyAktywny = promocja.CzyAktywny;

            PrzygotujDanePromocji(promocjaZBazy);

            try
            {
                await _context.SaveChangesAsync();

                return true;
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!await CzyIstnieje(promocja.IdPromocji))
                {
                    return false;
                }

                throw;
            }
        }

        public async Task<Promocja?> PobierzDoUsuniecia(int id)
        {
            return await _context.Promocja
                .FirstOrDefaultAsync(p => p.IdPromocji == id);
        }

        public async Task Usun(int id)
        {
            var promocja = await _context.Promocja
                .FirstOrDefaultAsync(p => p.IdPromocji == id);

            if (promocja == null)
            {
                return;
            }

            _context.Promocja.Remove(promocja);

            await _context.SaveChangesAsync();
        }

        public async Task Aktywuj(int id)
        {
            var promocja = await _context.Promocja
                .FirstOrDefaultAsync(p => p.IdPromocji == id);

            if (promocja == null)
            {
                return;
            }

            promocja.CzyAktywny = true;

            await _context.SaveChangesAsync();
        }

        public async Task Dezaktywuj(int id)
        {
            var promocja = await _context.Promocja
                .FirstOrDefaultAsync(p => p.IdPromocji == id);

            if (promocja == null)
            {
                return;
            }

            promocja.CzyAktywny = false;

            await _context.SaveChangesAsync();
        }

        private static void PrzygotujDanePromocji(Promocja promocja)
        {
            promocja.Tytul = promocja.Tytul.Trim();
            promocja.Opis = promocja.Opis?.Trim() ?? string.Empty;
        }

        private async Task<bool> CzyIstnieje(int id)
        {
            return await _context.Promocja
                .AnyAsync(p => p.IdPromocji == id);
        }
    }
}
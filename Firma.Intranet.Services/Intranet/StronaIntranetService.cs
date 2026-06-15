using Firma.Data.Data;
using Firma.Data.Data.CMS;
using Firma.Intranet.Interfaces.Intranet;
using Microsoft.EntityFrameworkCore;

namespace Firma.Intranet.Services.Intranet
{
    public class StronaIntranetService : IStronaIntranetService
    {
        private readonly FirmaContext _context;

        public StronaIntranetService(FirmaContext context)
        {
            _context = context;
        }

        public async Task<List<Strona>> PobierzListe()
        {
            return await _context.Strona
                .OrderBy(s => s.Pozycja)
                .ThenBy(s => s.Tytul)
                .ToListAsync();
        }

        public async Task<Strona?> PobierzSzczegoly(int id)
        {
            return await _context.Strona
                .FirstOrDefaultAsync(s => s.IdStrony == id);
        }

        public async Task<Strona?> PobierzDoEdycji(int id)
        {
            return await _context.Strona
                .FirstOrDefaultAsync(s => s.IdStrony == id);
        }

        public async Task Dodaj(Strona strona)
        {
            PrzygotujDaneStrony(strona);

            _context.Strona.Add(strona);

            await _context.SaveChangesAsync();
        }

        public async Task<bool> Aktualizuj(int id, Strona strona)
        {
            if (id != strona.IdStrony)
            {
                return false;
            }

            var stronaZBazy = await _context.Strona
                .FirstOrDefaultAsync(s => s.IdStrony == id);

            if (stronaZBazy == null)
            {
                return false;
            }

            stronaZBazy.LinkTytul = strona.LinkTytul;
            stronaZBazy.Tytul = strona.Tytul;
            stronaZBazy.Tresc = strona.Tresc;
            stronaZBazy.Pozycja = strona.Pozycja;
            stronaZBazy.CzyAktywny = strona.CzyAktywny;

            PrzygotujDaneStrony(stronaZBazy);

            try
            {
                await _context.SaveChangesAsync();

                return true;
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!await CzyIstnieje(strona.IdStrony))
                {
                    return false;
                }

                throw;
            }
        }

        public async Task<Strona?> PobierzDoUsuniecia(int id)
        {
            return await _context.Strona
                .FirstOrDefaultAsync(s => s.IdStrony == id);
        }

        public async Task Usun(int id)
        {
            var strona = await _context.Strona
                .FirstOrDefaultAsync(s => s.IdStrony == id);

            if (strona == null)
            {
                return;
            }

            _context.Strona.Remove(strona);

            await _context.SaveChangesAsync();
        }

        public async Task Aktywuj(int id)
        {
            var strona = await _context.Strona
                .FirstOrDefaultAsync(s => s.IdStrony == id);

            if (strona == null)
            {
                return;
            }

            strona.CzyAktywny = true;

            await _context.SaveChangesAsync();
        }

        public async Task Dezaktywuj(int id)
        {
            var strona = await _context.Strona
                .FirstOrDefaultAsync(s => s.IdStrony == id);

            if (strona == null)
            {
                return;
            }

            strona.CzyAktywny = false;

            await _context.SaveChangesAsync();
        }

        private static void PrzygotujDaneStrony(Strona strona)
        {
            strona.LinkTytul = strona.LinkTytul.Trim();
            strona.Tytul = strona.Tytul.Trim();
            strona.Tresc = strona.Tresc.Trim();
        }

        private async Task<bool> CzyIstnieje(int id)
        {
            return await _context.Strona
                .AnyAsync(s => s.IdStrony == id);
        }
    }
}
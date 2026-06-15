using Firma.Data.Data;
using Firma.Data.Data.CMS;
using Firma.Intranet.Interfaces.Intranet;
using Microsoft.EntityFrameworkCore;

namespace Firma.Intranet.Services.Intranet
{
    public class AktualnoscIntranetService : IAktualnoscIntranetService
    {
        private readonly FirmaContext _context;

        public AktualnoscIntranetService(FirmaContext context)
        {
            _context = context;
        }

        public async Task<List<Aktualnosc>> PobierzListe()
        {
            return await _context.Aktualnosc
                .OrderBy(a => a.Pozycja)
                .ThenBy(a => a.Tytul)
                .ToListAsync();
        }

        public async Task<Aktualnosc?> PobierzSzczegoly(int id)
        {
            return await _context.Aktualnosc
                .FirstOrDefaultAsync(a => a.IdAktualnosci == id);
        }

        public async Task<Aktualnosc?> PobierzDoEdycji(int id)
        {
            return await _context.Aktualnosc
                .FirstOrDefaultAsync(a => a.IdAktualnosci == id);
        }

        public async Task Dodaj(Aktualnosc aktualnosc)
        {
            PrzygotujDaneAktualnosci(aktualnosc);

            _context.Aktualnosc.Add(aktualnosc);

            await _context.SaveChangesAsync();
        }

        public async Task<bool> Aktualizuj(int id, Aktualnosc aktualnosc)
        {
            if (id != aktualnosc.IdAktualnosci)
            {
                return false;
            }

            var aktualnoscZBazy = await _context.Aktualnosc
                .FirstOrDefaultAsync(a => a.IdAktualnosci == id);

            if (aktualnoscZBazy == null)
            {
                return false;
            }

            aktualnoscZBazy.LinkTytul = aktualnosc.LinkTytul;
            aktualnoscZBazy.Tytul = aktualnosc.Tytul;
            aktualnoscZBazy.Tresc = aktualnosc.Tresc;
            aktualnoscZBazy.Pozycja = aktualnosc.Pozycja;
            aktualnoscZBazy.CzyAktywny = aktualnosc.CzyAktywny;

            PrzygotujDaneAktualnosci(aktualnoscZBazy);

            try
            {
                await _context.SaveChangesAsync();

                return true;
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!await CzyIstnieje(aktualnosc.IdAktualnosci))
                {
                    return false;
                }

                throw;
            }
        }

        public async Task<Aktualnosc?> PobierzDoUsuniecia(int id)
        {
            return await _context.Aktualnosc
                .FirstOrDefaultAsync(a => a.IdAktualnosci == id);
        }

        public async Task Usun(int id)
        {
            var aktualnosc = await _context.Aktualnosc
                .FirstOrDefaultAsync(a => a.IdAktualnosci == id);

            if (aktualnosc == null)
            {
                return;
            }

            _context.Aktualnosc.Remove(aktualnosc);

            await _context.SaveChangesAsync();
        }

        public async Task Aktywuj(int id)
        {
            var aktualnosc = await _context.Aktualnosc
                .FirstOrDefaultAsync(a => a.IdAktualnosci == id);

            if (aktualnosc == null)
            {
                return;
            }

            aktualnosc.CzyAktywny = true;

            await _context.SaveChangesAsync();
        }

        public async Task Dezaktywuj(int id)
        {
            var aktualnosc = await _context.Aktualnosc
                .FirstOrDefaultAsync(a => a.IdAktualnosci == id);

            if (aktualnosc == null)
            {
                return;
            }

            aktualnosc.CzyAktywny = false;

            await _context.SaveChangesAsync();
        }

        private static void PrzygotujDaneAktualnosci(Aktualnosc aktualnosc)
        {
            aktualnosc.LinkTytul = aktualnosc.LinkTytul.Trim();
            aktualnosc.Tytul = aktualnosc.Tytul.Trim();
            aktualnosc.Tresc = aktualnosc.Tresc.Trim();
        }

        private async Task<bool> CzyIstnieje(int id)
        {
            return await _context.Aktualnosc
                .AnyAsync(a => a.IdAktualnosci == id);
        }
    }
}
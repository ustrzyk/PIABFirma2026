using Firma.Data.Data;
using Firma.Data.Data.Sklep;
using Firma.Intranet.Interfaces.Intranet;
using Firma.Intranet.Services.Data.Intranet;
using Microsoft.EntityFrameworkCore;

namespace Firma.Intranet.Services.Intranet
{
    public class TowarIntranetService : ITowarIntranetService
    {
        private readonly FirmaContext _context;

        public TowarIntranetService(FirmaContext context)
        {
            _context = context;
        }

        public async Task<List<Towar>> PobierzListe()
        {
            return await _context.Towar
                .Include(t => t.Producent)
                .Include(t => t.Rodzaj)
                .OrderBy(t => t.Nazwa)
                .ToListAsync();
        }

        public async Task<Towar?> PobierzSzczegoly(int id)
        {
            return await _context.Towar
                .Include(t => t.Producent)
                .Include(t => t.Rodzaj)
                .Include(t => t.ZalacznikiTowaru)
                .FirstOrDefaultAsync(t => t.IdTowaru == id);
        }

        public async Task<Towar?> PobierzDoEdycji(int id)
        {
            return await _context.Towar
                .FirstOrDefaultAsync(t => t.IdTowaru == id);
        }

        public async Task Dodaj(Towar towar)
        {
            towar.Cena = decimal.Round(
                towar.Cena,
                2,
                MidpointRounding.AwayFromZero);

            _context.Towar.Add(towar);

            await _context.SaveChangesAsync();
        }

        public async Task<bool> Aktualizuj(int id, Towar towar)
        {
            if (id != towar.IdTowaru)
            {
                return false;
            }

            towar.Cena = decimal.Round(
                towar.Cena,
                2,
                MidpointRounding.AwayFromZero);

            _context.Update(towar);

            try
            {
                await _context.SaveChangesAsync();

                return true;
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!await CzyIstnieje(towar.IdTowaru))
                {
                    return false;
                }

                throw;
            }
        }

        public async Task<Towar?> PobierzDoUsuniecia(int id)
        {
            return await _context.Towar
                .Include(t => t.Producent)
                .Include(t => t.Rodzaj)
                .FirstOrDefaultAsync(t => t.IdTowaru == id);
        }

        public async Task Usun(int id, string folderUploadu)
        {
            var towar = await _context.Towar
                .Include(t => t.StanMagazynowy)
                .Include(t => t.ZalacznikiTowaru)
                .Include(t => t.PozycjaZamowienia)
                .FirstOrDefaultAsync(t => t.IdTowaru == id);

            if (towar != null)
            {
                UsunTowarAlboDezaktywuj(towar, folderUploadu);

                await _context.SaveChangesAsync();
            }
        }

        public async Task UsunZaznaczone(int[] ids, string folderUploadu)
        {
            if (ids == null || ids.Length == 0)
            {
                return;
            }

            var towary = await _context.Towar
                .Include(t => t.StanMagazynowy)
                .Include(t => t.ZalacznikiTowaru)
                .Include(t => t.PozycjaZamowienia)
                .Where(t => ids.Contains(t.IdTowaru))
                .ToListAsync();

            foreach (var towar in towary)
            {
                UsunTowarAlboDezaktywuj(towar, folderUploadu);
            }

            await _context.SaveChangesAsync();
        }

        public async Task DezaktywujZaznaczone(int[] ids)
        {
            if (ids == null || ids.Length == 0)
            {
                return;
            }

            var towary = await _context.Towar
                .Where(t => ids.Contains(t.IdTowaru))
                .ToListAsync();

            foreach (var towar in towary)
            {
                towar.CzyAktywny = false;
            }

            await _context.SaveChangesAsync();
        }

        public async Task AktywujZaznaczone(int[] ids)
        {
            if (ids == null || ids.Length == 0)
            {
                return;
            }

            var towary = await _context.Towar
                .Where(t => ids.Contains(t.IdTowaru))
                .ToListAsync();

            foreach (var towar in towary)
            {
                towar.CzyAktywny = true;
            }

            await _context.SaveChangesAsync();
        }

        public async Task<List<ProducentSelectItemDto>> PobierzProducentowDoSelectList()
        {
            return await _context.Producent
                .OrderBy(p => p.Nazwa)
                .Select(p => new ProducentSelectItemDto
                {
                    IdProducenta = p.IdProducenta,
                    Nazwa = p.Nazwa
                })
                .ToListAsync();
        }

        public async Task<List<RodzajSelectItemDto>> PobierzRodzajeDoSelectList()
        {
            return await _context.Rodzaj
                .OrderBy(r => r.Nazwa)
                .Select(r => new RodzajSelectItemDto
                {
                    IdRodzaju = r.IdRodzaju,
                    Nazwa = r.Nazwa
                })
                .ToListAsync();
        }

        private void UsunTowarAlboDezaktywuj(Towar towar, string folderUploadu)
        {
            if (towar.PozycjaZamowienia != null && towar.PozycjaZamowienia.Any())
            {
                towar.CzyAktywny = false;
                _context.Update(towar);

                return;
            }

            if (towar.ZalacznikiTowaru != null && towar.ZalacznikiTowaru.Any())
            {
                foreach (var zalacznik in towar.ZalacznikiTowaru)
                {
                    UsunPlik(folderUploadu, zalacznik.Sciezka);
                }

                _context.ZalacznikTowaru.RemoveRange(towar.ZalacznikiTowaru);
            }

            if (towar.StanMagazynowy != null)
            {
                _context.StanMagazynowy.Remove(towar.StanMagazynowy);
            }

            _context.Towar.Remove(towar);
        }

        private static void UsunPlik(string folderUploadu, string sciezka)
        {
            var nazwaPliku = sciezka
                .Split('/', StringSplitOptions.RemoveEmptyEntries)
                .LastOrDefault() ?? string.Empty;

            var sciezkaFizyczna = Path.Combine(folderUploadu, nazwaPliku);

            if (File.Exists(sciezkaFizyczna))
            {
                File.Delete(sciezkaFizyczna);
            }
        }

        private async Task<bool> CzyIstnieje(int id)
        {
            return await _context.Towar
                .AnyAsync(t => t.IdTowaru == id);
        }
    }
}
using Firma.Data.Data;
using Firma.Data.Data.Sklep;
using Firma.Intranet.Interfaces.Intranet;
using Firma.Intranet.Services.Data.Intranet;
using Microsoft.EntityFrameworkCore;

namespace Firma.Intranet.Services.Intranet
{
    public class PozycjaZamowieniaIntranetService : IPozycjaZamowieniaIntranetService
    {
        private readonly FirmaContext _context;

        public PozycjaZamowieniaIntranetService(FirmaContext context)
        {
            _context = context;
        }

        public async Task<List<PozycjaZamowienia>> PobierzListe()
        {
            return await _context.PozycjaZamowienia
                .Include(p => p.Towar)
                .Include(p => p.Zamowienie)
                .OrderByDescending(p => p.IdZamowienia)
                .ThenBy(p => p.IdPozycjiZamowienia)
                .ToListAsync();
        }

        public async Task<PozycjaZamowienia?> PobierzSzczegoly(int id)
        {
            return await _context.PozycjaZamowienia
                .Include(p => p.Towar)
                .Include(p => p.Zamowienie)
                .FirstOrDefaultAsync(p => p.IdPozycjiZamowienia == id);
        }

        public async Task<PozycjaZamowienia?> PobierzDoEdycji(int id)
        {
            return await _context.PozycjaZamowienia
                .FirstOrDefaultAsync(p => p.IdPozycjiZamowienia == id);
        }

        public async Task Dodaj(PozycjaZamowienia pozycjaZamowienia)
        {
            pozycjaZamowienia.CenaJednostkowa = decimal.Round(
                pozycjaZamowienia.CenaJednostkowa,
                2,
                MidpointRounding.AwayFromZero);

            _context.PozycjaZamowienia.Add(pozycjaZamowienia);

            await _context.SaveChangesAsync();

            await PrzeliczWartoscZamowienia(pozycjaZamowienia.IdZamowienia);
        }

        public async Task<bool> Aktualizuj(int id, PozycjaZamowienia pozycjaZamowienia)
        {
            if (id != pozycjaZamowienia.IdPozycjiZamowienia)
            {
                return false;
            }

            var pozycjaZBazy = await _context.PozycjaZamowienia
                .FirstOrDefaultAsync(p => p.IdPozycjiZamowienia == id);

            if (pozycjaZBazy == null)
            {
                return false;
            }

            var poprzednieIdZamowienia = pozycjaZBazy.IdZamowienia;

            pozycjaZBazy.Ilosc = pozycjaZamowienia.Ilosc;
            pozycjaZBazy.CenaJednostkowa = decimal.Round(
                pozycjaZamowienia.CenaJednostkowa,
                2,
                MidpointRounding.AwayFromZero);
            pozycjaZBazy.IdZamowienia = pozycjaZamowienia.IdZamowienia;
            pozycjaZBazy.IdTowaru = pozycjaZamowienia.IdTowaru;

            try
            {
                await _context.SaveChangesAsync();

                await PrzeliczWartoscZamowienia(pozycjaZBazy.IdZamowienia);

                if (poprzednieIdZamowienia != pozycjaZBazy.IdZamowienia)
                {
                    await PrzeliczWartoscZamowienia(poprzednieIdZamowienia);
                }

                return true;
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!await CzyIstnieje(pozycjaZamowienia.IdPozycjiZamowienia))
                {
                    return false;
                }

                throw;
            }
        }

        public async Task<PozycjaZamowienia?> PobierzDoUsuniecia(int id)
        {
            return await _context.PozycjaZamowienia
                .Include(p => p.Towar)
                .Include(p => p.Zamowienie)
                .FirstOrDefaultAsync(p => p.IdPozycjiZamowienia == id);
        }

        public async Task Usun(int id)
        {
            var pozycjaZamowienia = await _context.PozycjaZamowienia
                .FirstOrDefaultAsync(p => p.IdPozycjiZamowienia == id);

            if (pozycjaZamowienia != null)
            {
                var idZamowienia = pozycjaZamowienia.IdZamowienia;

                _context.PozycjaZamowienia.Remove(pozycjaZamowienia);

                await _context.SaveChangesAsync();

                await PrzeliczWartoscZamowienia(idZamowienia);
            }
        }

        public async Task<List<ZamowienieSelectItemDto>> PobierzZamowieniaDoSelectList()
        {
            return await _context.Zamowienie
                .OrderByDescending(z => z.DataZamowienia)
                .ThenBy(z => z.NumerZamowienia)
                .Select(z => new ZamowienieSelectItemDto
                {
                    IdZamowienia = z.IdZamowienia,
                    NumerZamowienia = z.NumerZamowienia
                })
                .ToListAsync();
        }

        public async Task<List<TowarSelectItemDto>> PobierzTowaryDoSelectList()
        {
            return await _context.Towar
                .OrderBy(t => t.Nazwa)
                .Select(t => new TowarSelectItemDto
                {
                    IdTowaru = t.IdTowaru,
                    Nazwa = t.Nazwa
                })
                .ToListAsync();
        }

        private async Task PrzeliczWartoscZamowienia(int idZamowienia)
        {
            var zamowienie = await _context.Zamowienie
                .FirstOrDefaultAsync(z => z.IdZamowienia == idZamowienia);

            if (zamowienie == null)
            {
                return;
            }

            var wartoscRazem = await _context.PozycjaZamowienia
                .Where(p => p.IdZamowienia == idZamowienia)
                .SumAsync(p => p.Ilosc * p.CenaJednostkowa);

            zamowienie.WartoscRazem = decimal.Round(
                wartoscRazem,
                2,
                MidpointRounding.AwayFromZero);

            await _context.SaveChangesAsync();
        }

        private async Task<bool> CzyIstnieje(int id)
        {
            return await _context.PozycjaZamowienia
                .AnyAsync(p => p.IdPozycjiZamowienia == id);
        }
    }
}
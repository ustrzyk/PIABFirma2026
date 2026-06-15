using Firma.Data.Data;
using Firma.Data.Data.CMS;
using Firma.Intranet.Interfaces.Intranet;
using Microsoft.EntityFrameworkCore;

namespace Firma.Intranet.Services.Intranet
{
    public class UstawieniePortaluIntranetService : IUstawieniePortaluIntranetService
    {
        private readonly FirmaContext _context;

        public UstawieniePortaluIntranetService(FirmaContext context)
        {
            _context = context;
        }

        public async Task<List<UstawieniePortalu>> PobierzListe()
        {
            return await _context.UstawieniePortalu
                .OrderBy(u => u.Klucz)
                .ToListAsync();
        }

        public async Task<UstawieniePortalu?> PobierzSzczegoly(int id)
        {
            return await _context.UstawieniePortalu
                .FirstOrDefaultAsync(u => u.IdUstawieniaPortalu == id);
        }

        public async Task<UstawieniePortalu?> PobierzDoEdycji(int id)
        {
            return await _context.UstawieniePortalu
                .FirstOrDefaultAsync(u => u.IdUstawieniaPortalu == id);
        }

        public async Task<bool> CzyKluczIstnieje(string klucz, int? idUstawieniaDoPominiecia = null)
        {
            var przygotowanyKlucz = PrzygotujKlucz(klucz);

            if (string.IsNullOrWhiteSpace(przygotowanyKlucz))
            {
                return false;
            }

            var zapytanie = _context.UstawieniePortalu
                .Where(u => u.Klucz.ToLower() == przygotowanyKlucz);

            if (idUstawieniaDoPominiecia.HasValue)
            {
                zapytanie = zapytanie
                    .Where(u => u.IdUstawieniaPortalu != idUstawieniaDoPominiecia.Value);
            }

            return await zapytanie.AnyAsync();
        }

        public async Task Dodaj(UstawieniePortalu ustawieniePortalu)
        {
            PrzygotujDaneUstawienia(ustawieniePortalu);

            _context.UstawieniePortalu.Add(ustawieniePortalu);

            await _context.SaveChangesAsync();
        }

        public async Task<bool> Aktualizuj(int id, UstawieniePortalu ustawieniePortalu)
        {
            if (id != ustawieniePortalu.IdUstawieniaPortalu)
            {
                return false;
            }

            var ustawienieZBazy = await _context.UstawieniePortalu
                .FirstOrDefaultAsync(u => u.IdUstawieniaPortalu == id);

            if (ustawienieZBazy == null)
            {
                return false;
            }

            ustawienieZBazy.Klucz = ustawieniePortalu.Klucz;
            ustawienieZBazy.Wartosc = ustawieniePortalu.Wartosc;
            ustawienieZBazy.Opis = ustawieniePortalu.Opis;
            ustawienieZBazy.CzyAktywny = ustawieniePortalu.CzyAktywny;

            PrzygotujDaneUstawienia(ustawienieZBazy);

            try
            {
                await _context.SaveChangesAsync();

                return true;
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!await CzyIstnieje(ustawieniePortalu.IdUstawieniaPortalu))
                {
                    return false;
                }

                throw;
            }
        }

        public async Task<UstawieniePortalu?> PobierzDoUsuniecia(int id)
        {
            return await _context.UstawieniePortalu
                .FirstOrDefaultAsync(u => u.IdUstawieniaPortalu == id);
        }

        public async Task Usun(int id)
        {
            var ustawieniePortalu = await _context.UstawieniePortalu
                .FirstOrDefaultAsync(u => u.IdUstawieniaPortalu == id);

            if (ustawieniePortalu == null)
            {
                return;
            }

            _context.UstawieniePortalu.Remove(ustawieniePortalu);

            await _context.SaveChangesAsync();
        }

        public async Task Aktywuj(int id)
        {
            var ustawieniePortalu = await _context.UstawieniePortalu
                .FirstOrDefaultAsync(u => u.IdUstawieniaPortalu == id);

            if (ustawieniePortalu == null)
            {
                return;
            }

            ustawieniePortalu.CzyAktywny = true;

            await _context.SaveChangesAsync();
        }

        public async Task Dezaktywuj(int id)
        {
            var ustawieniePortalu = await _context.UstawieniePortalu
                .FirstOrDefaultAsync(u => u.IdUstawieniaPortalu == id);

            if (ustawieniePortalu == null)
            {
                return;
            }

            ustawieniePortalu.CzyAktywny = false;

            await _context.SaveChangesAsync();
        }

        private static void PrzygotujDaneUstawienia(UstawieniePortalu ustawieniePortalu)
        {
            ustawieniePortalu.Klucz = PrzygotujKlucz(ustawieniePortalu.Klucz);
            ustawieniePortalu.Wartosc = ustawieniePortalu.Wartosc.Trim();
            ustawieniePortalu.Opis = ustawieniePortalu.Opis?.Trim() ?? string.Empty;
        }

        private static string PrzygotujKlucz(string klucz)
        {
            return klucz.Trim().ToLowerInvariant();
        }

        private async Task<bool> CzyIstnieje(int id)
        {
            return await _context.UstawieniePortalu
                .AnyAsync(u => u.IdUstawieniaPortalu == id);
        }
    }
}
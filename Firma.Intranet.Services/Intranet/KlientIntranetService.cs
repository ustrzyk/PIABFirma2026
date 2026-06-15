using Firma.Data.Data;
using Firma.Data.Data.Sklep;
using Firma.Intranet.Interfaces.Intranet;
using Microsoft.EntityFrameworkCore;

namespace Firma.Intranet.Services.Intranet
{
    public class KlientIntranetService : IKlientIntranetService
    {
        private readonly FirmaContext _context;

        public KlientIntranetService(FirmaContext context)
        {
            _context = context;
        }

        public async Task<List<Klient>> PobierzListe()
        {
            return await _context.Klient
                .OrderBy(k => k.Nazwisko)
                .ThenBy(k => k.Imie)
                .ToListAsync();
        }

        public async Task<Klient?> PobierzSzczegoly(int id)
        {
            return await _context.Klient
                .Include(k => k.Zamowienie)
                .FirstOrDefaultAsync(k => k.IdKlienta == id);
        }

        public async Task<Klient?> PobierzDoEdycji(int id)
        {
            return await _context.Klient
                .FirstOrDefaultAsync(k => k.IdKlienta == id);
        }

        public async Task<bool> CzyEmailIstnieje(string email, int? idKlientaDoPominiecia = null)
        {
            var przygotowanyEmail = PrzygotujEmail(email);

            if (string.IsNullOrWhiteSpace(przygotowanyEmail))
            {
                return false;
            }

            var zapytanie = _context.Klient
                .Where(k => k.Email.ToLower() == przygotowanyEmail);

            if (idKlientaDoPominiecia.HasValue)
            {
                zapytanie = zapytanie
                    .Where(k => k.IdKlienta != idKlientaDoPominiecia.Value);
            }

            return await zapytanie.AnyAsync();
        }

        public async Task Dodaj(Klient klient)
        {
            PrzygotujDaneKlienta(klient);

            _context.Klient.Add(klient);

            await _context.SaveChangesAsync();
        }

        public async Task<bool> Aktualizuj(int id, Klient klient)
        {
            if (id != klient.IdKlienta)
            {
                return false;
            }

            PrzygotujDaneKlienta(klient);

            _context.Update(klient);

            try
            {
                await _context.SaveChangesAsync();

                return true;
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!await CzyIstnieje(klient.IdKlienta))
                {
                    return false;
                }

                throw;
            }
        }

        public async Task<Klient?> PobierzDoUsuniecia(int id)
        {
            return await _context.Klient
                .Include(k => k.Zamowienie)
                .FirstOrDefaultAsync(k => k.IdKlienta == id);
        }

        public async Task<bool> Usun(int id)
        {
            var klient = await _context.Klient
                .Include(k => k.Zamowienie)
                .FirstOrDefaultAsync(k => k.IdKlienta == id);

            if (klient == null)
            {
                return false;
            }

            if (klient.Zamowienie.Any())
            {
                return false;
            }

            _context.Klient.Remove(klient);

            await _context.SaveChangesAsync();

            return true;
        }

        private static void PrzygotujDaneKlienta(Klient klient)
        {
            klient.Imie = klient.Imie.Trim();
            klient.Nazwisko = klient.Nazwisko.Trim();
            klient.Email = PrzygotujEmail(klient.Email);
            klient.Telefon = klient.Telefon?.Trim() ?? string.Empty;
        }

        private static string PrzygotujEmail(string email)
        {
            return email.Trim().ToLowerInvariant();
        }

        private async Task<bool> CzyIstnieje(int id)
        {
            return await _context.Klient
                .AnyAsync(k => k.IdKlienta == id);
        }
    }
}
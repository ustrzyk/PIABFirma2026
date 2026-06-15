using Firma.Data.Data;
using Firma.Interfaces.CMS;
using Firma.Services.Abstrakcja;
using Firma.Services.Data.Dto.CMS;
using Firma.Services.Data.Dto.UstawieniaPortalu;
using Microsoft.EntityFrameworkCore;

namespace Firma.Services.CMS
{
    public class UstawieniePortaluService : BaseService, IUstawieniePortaluService
    {
        public UstawieniePortaluService(FirmaContext context)
            : base(context)
        {
        }

        public async Task<IList<UstawieniePortaluListaItemDto>> GetUstawieniaPortalu()
        {
            // Pobieram ustawienia do listy
            var ustawienia = await _context.UstawieniePortalu
                .Where(u => u.CzyAktywny)
                .OrderBy(u => u.Klucz)
                .Select(u => new UstawieniePortaluListaItemDto
                {
                    IdUstawieniaPortalu = u.IdUstawieniaPortalu,
                    Klucz = u.Klucz,
                    Wartosc = u.Wartosc,
                    Opis = u.Opis
                })
                .ToListAsync();

            return ustawienia;
        }

        public async Task<UstawieniePortaluSzczegolyDto?> GetUstawieniePortalu(int idUstawieniaPortalu)
        {
            // Pobieram ustawienie do szczegółów
            var ustawienie = await _context.UstawieniePortalu
                .Where(u => u.CzyAktywny)
                .Where(u => u.IdUstawieniaPortalu == idUstawieniaPortalu)
                .Select(u => new UstawieniePortaluSzczegolyDto
                {
                    IdUstawieniaPortalu = u.IdUstawieniaPortalu,
                    Klucz = u.Klucz,
                    Wartosc = u.Wartosc,
                    Opis = u.Opis
                })
                .FirstOrDefaultAsync();

            return ustawienie;
        }

        public async Task<PortalWygladDto> GetWygladPortalu()
        {
            // Pobieram ustawienia wyglądu
            var ustawienia = await _context.UstawieniePortalu
                .Where(u => u.CzyAktywny)
                .ToDictionaryAsync(u => u.Klucz, u => u.Wartosc);

            var wyglad = new PortalWygladDto
            {
                NazwaPortalu = Pobierz(ustawienia, "NazwaPortalu", "Sklep 3D"),
                StopkaTekst = Pobierz(ustawienia, "StopkaTekst", "Sklep z drukarkami 3D"),
                StopkaAdres = Pobierz(ustawienia, "StopkaAdres", ""),
                StopkaEmail = Pobierz(ustawienia, "StopkaEmail", ""),
                StopkaTelefon = Pobierz(ustawienia, "StopkaTelefon", ""),
                StopkaFacebook = Pobierz(ustawienia, "StopkaFacebook", ""),
                KolorTlaPortalu = Pobierz(ustawienia, "KolorTlaPortalu", "#eef2f6"),
                KolorNawigacji = Pobierz(ustawienia, "KolorNawigacji", "#ffffff"),
                KolorStopki = Pobierz(ustawienia, "KolorStopki", "#f8f9fa"),
                KolorPrzyciskow = Pobierz(ustawienia, "KolorPrzyciskow", "#0d6efd"),
                KolorAkcentu = Pobierz(ustawienia, "KolorAkcentu", "#258cfb")
            };

            return wyglad;
        }

        private static string Pobierz(Dictionary<string, string> ustawienia, string klucz, string wartoscDomyslna)
        {
            // Pobieram wartość albo domyślną
            if (ustawienia.TryGetValue(klucz, out var wartosc) && !string.IsNullOrWhiteSpace(wartosc))
            {
                return wartosc;
            }

            return wartoscDomyslna;
        }
    }
}
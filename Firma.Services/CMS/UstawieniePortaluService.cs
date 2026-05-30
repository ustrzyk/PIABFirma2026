using Firma.Data.Data;
using Firma.Interfaces.CMS;
using Firma.Services.Abstrakcja;
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
    }
}
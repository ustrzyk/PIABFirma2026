using Firma.Data.Data;
using Firma.Interfaces.Sklep;
using Firma.Services.Abstrakcja;
using Firma.Services.Data.Dto.Rodzaje;
using Microsoft.EntityFrameworkCore;

namespace Firma.Services.Sklep
{
    public class RodzajService : BaseService, IRodzajService
    {
        public RodzajService(FirmaContext context)
            : base(context)
        {
        }

        public async Task<IList<RodzajMenuItemDto>> GetRodzaje()
        {
            // Pobieram aktywne rodzaje do menu
            var rodzaje = await _context.Rodzaj
                .Where(r => r.CzyAktywny)
                .OrderBy(r => r.Nazwa)
                .Select(r => new RodzajMenuItemDto
                {
                    IdRodzaju = r.IdRodzaju,
                    Nazwa = r.Nazwa,
                    Opis = r.Opis
                })
                .ToListAsync();

            return rodzaje;
        }
    }
}
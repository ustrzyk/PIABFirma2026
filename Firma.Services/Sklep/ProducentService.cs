using Firma.Data.Data;
using Firma.Data.Data.Sklep;
using Firma.Interfaces.Sklep;
using Firma.Services.Abstrakcja;
using Firma.Services.Data.Dto.Producenci;
using Microsoft.EntityFrameworkCore;

namespace Firma.Services.Sklep
{
    public class ProducentService : BaseService, IProducentService
    {
        public ProducentService(FirmaContext context)
            : base(context)
        {
        }

        public async Task<IList<ProducentListaItemDto>> GetProducenci()
        {
            // Pobieram producentów do listy
            var producenci = await _context.Producent
                .Where(p => p.CzyAktywny)
                .OrderBy(p => p.Nazwa)
                .Select(p => new ProducentListaItemDto
                {
                    IdProducenta = p.IdProducenta,
                    Nazwa = p.Nazwa,
                    Kraj = p.Kraj,
                    StronaWWW = p.StronaWWW,
                    Opis = p.Opis,
                    IloscTowarow = p.Towar.Count(t => t.CzyAktywny)
                })
                .ToListAsync();

            return producenci;
        }

        public async Task<Producent?> GetProducent(int idProducenta)
        {
            // Pobieram jednego producenta
            var producent = await _context.Producent
                .Where(p => p.CzyAktywny)
                .Include(p => p.Towar.Where(t => t.CzyAktywny))
                .FirstOrDefaultAsync(p => p.IdProducenta == idProducenta);

            return producent;
        }
    }
}
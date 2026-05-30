using Firma.Data.Data;
using Firma.Data.Data.CMS;
using Firma.Interfaces.CMS;
using Firma.Services.Abstrakcja;
using Firma.Services.Data.Dto.Promocje;
using Microsoft.EntityFrameworkCore;

namespace Firma.Services.CMS
{
    public class PromocjaService : BaseService, IPromocjaService
    {
        public PromocjaService(FirmaContext context)
            : base(context)
        {
        }

        public async Task<IList<PromocjaListaItemDto>> GetPromocje()
        {
            // Pobieram promocje do listy
            var promocje = await _context.Promocja
                .Where(p => p.CzyAktywny)
                .OrderByDescending(p => p.DataOd)
                .Select(p => new PromocjaListaItemDto
                {
                    IdPromocji = p.IdPromocji,
                    Tytul = p.Tytul,
                    Opis = p.Opis,
                    RabatProcentowy = p.RabatProcentowy,
                    DataOd = p.DataOd,
                    DataDo = p.DataDo
                })
                .ToListAsync();

            return promocje;
        }

        public async Task<Promocja?> GetPromocja(int idPromocji)
        {
            // Pobieram jedną promocję
            var promocja = await _context.Promocja
                .Where(p => p.CzyAktywny)
                .FirstOrDefaultAsync(p => p.IdPromocji == idPromocji);

            return promocja;
        }
    }
}
using Firma.Data.Data;
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

        public async Task<PromocjaSzczegolyDto?> GetPromocja(int idPromocji)
        {
            // Pobieram promocję do szczegółów
            var promocja = await _context.Promocja
                .Where(p => p.CzyAktywny)
                .Where(p => p.IdPromocji == idPromocji)
                .Select(p => new PromocjaSzczegolyDto
                {
                    IdPromocji = p.IdPromocji,
                    Tytul = p.Tytul,
                    Opis = p.Opis,
                    RabatProcentowy = p.RabatProcentowy,
                    DataOd = p.DataOd,
                    DataDo = p.DataDo
                })
                .FirstOrDefaultAsync();

            return promocja;
        }
    }
}
using Firma.Data.Data;
using Firma.Interfaces.Sklep;
using Firma.Services.Abstrakcja;
using Firma.Services.Data.Dto.Producenci;
using Firma.Services.Data.Dto.Towary;
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

        public async Task<ProducentSzczegolyDto?> GetProducent(int idProducenta)
        {
            // Pobieram producenta do szczegółów
            var producent = await _context.Producent
                .Where(p => p.CzyAktywny)
                .Where(p => p.IdProducenta == idProducenta)
                .Select(p => new ProducentSzczegolyDto
                {
                    IdProducenta = p.IdProducenta,
                    Nazwa = p.Nazwa,
                    Kraj = p.Kraj,
                    StronaWWW = p.StronaWWW,
                    Opis = p.Opis
                })
                .FirstOrDefaultAsync();

            if (producent == null)
            {
                return null;
            }

            // Pobieram towary producenta
            producent.Towary = await _context.Towar
                .Where(t =>
                    t.CzyAktywny &&
                    t.IdProducenta == idProducenta &&
                    t.Rodzaj != null &&
                    t.Rodzaj.CzyAktywny &&
                    t.Producent != null &&
                    t.Producent.CzyAktywny)
                .OrderBy(t => t.Nazwa)
                .Select(t => new TowarListaItemDto
                {
                    IdTowaru = t.IdTowaru,
                    Kod = t.Kod,
                    Nazwa = t.Nazwa,
                    Cena = t.Cena,
                    FotoUrl = t.FotoUrl,
                    Opis = t.Opis,
                    Rodzaj = t.Rodzaj != null ? t.Rodzaj.Nazwa : "",
                    Producent = t.Producent != null ? t.Producent.Nazwa : "",
                    IloscSztuk = t.StanMagazynowy != null ? t.StanMagazynowy.IloscSztuk : null
                })
                .ToListAsync();

            return producent;
        }
    }
}
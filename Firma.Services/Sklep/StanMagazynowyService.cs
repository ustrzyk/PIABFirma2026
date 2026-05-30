using Firma.Data.Data;
using Firma.Data.Data.Sklep;
using Firma.Interfaces.Sklep;
using Firma.Services.Abstrakcja;
using Firma.Services.Data.Dto.StanyMagazynowe;
using Microsoft.EntityFrameworkCore;

namespace Firma.Services.Sklep
{
    public class StanMagazynowyService : BaseService, IStanMagazynowyService
    {
        public StanMagazynowyService(FirmaContext context)
            : base(context)
        {
        }

        public async Task<IList<StanMagazynowyListaItemDto>> GetStanyMagazynowe()
        {
            // Pobieram stany magazynowe do listy
            var stany = await _context.StanMagazynowy
                .Where(s =>
                    s.CzyAktywny &&
                    s.Towar != null &&
                    s.Towar.CzyAktywny)
                .OrderBy(s => s.Towar != null ? s.Towar.Nazwa : "")
                .Select(s => new StanMagazynowyListaItemDto
                {
                    IdStanuMagazynowego = s.IdStanuMagazynowego,
                    NazwaTowaru = s.Towar != null ? s.Towar.Nazwa : "",
                    KodTowaru = s.Towar != null ? s.Towar.Kod : "",
                    Rodzaj = s.Towar != null && s.Towar.Rodzaj != null ? s.Towar.Rodzaj.Nazwa : "",
                    Producent = s.Towar != null && s.Towar.Producent != null ? s.Towar.Producent.Nazwa : "",
                    IloscSztuk = s.IloscSztuk,
                    MinimalnaIlosc = s.MinimalnaIlosc,
                    Lokalizacja = s.Lokalizacja
                })
                .ToListAsync();

            return stany;
        }

        public async Task<StanMagazynowy?> GetStanMagazynowy(int idStanuMagazynowego)
        {
            // Pobieram jeden stan magazynowy
            var stan = await _context.StanMagazynowy
                .Include(s => s.Towar)
                    .ThenInclude(t => t.Rodzaj)
                .Include(s => s.Towar)
                    .ThenInclude(t => t.Producent)
                .FirstOrDefaultAsync(s =>
                    s.IdStanuMagazynowego == idStanuMagazynowego &&
                    s.CzyAktywny &&
                    s.Towar != null &&
                    s.Towar.CzyAktywny);

            return stan;
        }
    }
}
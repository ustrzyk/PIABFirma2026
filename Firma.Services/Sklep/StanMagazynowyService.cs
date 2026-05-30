using Firma.Data.Data;
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

        public async Task<StanMagazynowySzczegolyDto?> GetStanMagazynowy(int idStanuMagazynowego)
        {
            // Pobieram stan magazynowy do szczegółów
            var stan = await _context.StanMagazynowy
                .Where(s =>
                    s.IdStanuMagazynowego == idStanuMagazynowego &&
                    s.CzyAktywny &&
                    s.Towar != null &&
                    s.Towar.CzyAktywny &&
                    s.Towar.Rodzaj != null &&
                    s.Towar.Rodzaj.CzyAktywny &&
                    s.Towar.Producent != null &&
                    s.Towar.Producent.CzyAktywny)
                .Select(s => new StanMagazynowySzczegolyDto
                {
                    IdStanuMagazynowego = s.IdStanuMagazynowego,
                    IloscSztuk = s.IloscSztuk,
                    MinimalnaIlosc = s.MinimalnaIlosc,
                    Lokalizacja = s.Lokalizacja,
                    IdTowaru = s.Towar != null ? s.Towar.IdTowaru : 0,
                    KodTowaru = s.Towar != null ? s.Towar.Kod : "",
                    NazwaTowaru = s.Towar != null ? s.Towar.Nazwa : "",
                    CenaTowaru = s.Towar != null ? s.Towar.Cena : 0,
                    OpisTowaru = s.Towar != null ? s.Towar.Opis : "",
                    Rodzaj = s.Towar != null && s.Towar.Rodzaj != null ? s.Towar.Rodzaj.Nazwa : "",
                    Producent = s.Towar != null && s.Towar.Producent != null ? s.Towar.Producent.Nazwa : "",
                    FotoUrl = s.Towar != null ? s.Towar.FotoUrl : ""
                })
                .FirstOrDefaultAsync();

            return stan;
        }
    }
}
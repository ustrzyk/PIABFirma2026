using Firma.Data.Data;
using Firma.Interfaces.Sklep;
using Firma.Services.Abstrakcja;
using Firma.Services.Data.Dto.Towary;
using Microsoft.EntityFrameworkCore;

namespace Firma.Services.Sklep
{
    public class TowarService : BaseService, ITowarService
    {
        public TowarService(FirmaContext context)
            : base(context)
        {
        }

        public async Task<TowarSzczegolyDto?> GetTowar(int idTowaru)
        {
            // Pobieram towar do szczegółów
            var towar = await _context.Towar
                .Where(t =>
                    t.IdTowaru == idTowaru &&
                    t.CzyAktywny &&
                    t.Rodzaj != null &&
                    t.Rodzaj.CzyAktywny &&
                    t.Producent != null &&
                    t.Producent.CzyAktywny)
                .Select(t => new TowarSzczegolyDto
                {
                    IdTowaru = t.IdTowaru,
                    Kod = t.Kod,
                    Nazwa = t.Nazwa,
                    Cena = t.Cena,
                    FotoUrl = t.FotoUrl,
                    Opis = t.Opis,
                    Rodzaj = t.Rodzaj != null ? t.Rodzaj.Nazwa : "",
                    Producent = t.Producent != null ? t.Producent.Nazwa : "",
                    KrajProducenta = t.Producent != null ? t.Producent.Kraj : "",
                    StronaWWWProducenta = t.Producent != null ? t.Producent.StronaWWW : "",
                    IloscSztuk = t.StanMagazynowy != null ? t.StanMagazynowy.IloscSztuk : null,
                    MinimalnaIlosc = t.StanMagazynowy != null ? t.StanMagazynowy.MinimalnaIlosc : null,
                    Lokalizacja = t.StanMagazynowy != null ? t.StanMagazynowy.Lokalizacja : ""
                })
                .FirstOrDefaultAsync();

            return towar;
        }

        public async Task<IList<TowarListaItemDto>> GetTowaryDanegoRodzaju(int? idRodzaju)
        {
            // Przygotowuję zapytanie listy towarów
            var towary = _context.Towar
                .Where(t =>
                    t.CzyAktywny &&
                    t.Rodzaj != null &&
                    t.Rodzaj.CzyAktywny &&
                    t.Producent != null &&
                    t.Producent.CzyAktywny)
                .AsQueryable();

            if (idRodzaju != null)
            {
                // Filtruję po rodzaju
                towary = towary.Where(t => t.IdRodzaju == idRodzaju);
            }

            var wynik = await towary
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

            return wynik;
        }

        public async Task<IList<TowarListaItemDto>> GetTowary()
        {
            // Pobieram towary do DTO
            var towary = await _context.Towar
                .Where(t =>
                    t.CzyAktywny &&
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

            return towary;
        }
    }
}
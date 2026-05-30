using System;
using System.Collections.Generic;
using System.Text;

using Firma.Data.Data;
using Firma.Data.Data.Sklep;
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

        public async Task<Towar?> GetTowar(int idTowaru)
        {
            // Pobiera jeden aktywny towar razem z powiązaniami
            var towar = await _context.Towar
                .Include(t => t.Rodzaj)
                .Include(t => t.Producent)
                .Include(t => t.StanMagazynowy)
                .FirstOrDefaultAsync(t =>
                    t.IdTowaru == idTowaru &&
                    t.CzyAktywny &&
                    t.Rodzaj != null &&
                    t.Rodzaj.CzyAktywny &&
                    t.Producent != null &&
                    t.Producent.CzyAktywny);

            return towar;
        }

        public async Task<IList<Towar>> GetTowaryDanegoRodzaju(int? idRodzaju)
        {
            // Przygotowuje zapytanie do aktywnych towarów
            var towary = _context.Towar
                .Include(t => t.Rodzaj)
                .Include(t => t.Producent)
                .Include(t => t.StanMagazynowy)
                .Where(t =>
                    t.CzyAktywny &&
                    t.Rodzaj != null &&
                    t.Rodzaj.CzyAktywny &&
                    t.Producent != null &&
                    t.Producent.CzyAktywny)
                .AsQueryable();

            // Jeżeli wybrano kategorię, filtruje po rodzaju
            if (idRodzaju != null)
            {
                towary = towary.Where(t => t.IdRodzaju == idRodzaju);
            }

            var wynik = await towary
                .OrderBy(t => t.Nazwa)
                .ToListAsync();

            return wynik;
        }

        public async Task<IList<TowarListaItemDto>> GetTowary()
        {
            // Pobiera towary do prostego DTo
            var towary = await _context.Towar
                .Include(t => t.Rodzaj)
                .Include(t => t.Producent)
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
                    Rodzaj = t.Rodzaj != null ? t.Rodzaj.Nazwa : "",
                    Producent = t.Producent != null ? t.Producent.Nazwa : ""
                })
                .ToListAsync();

            return towary;
        }
    }
}
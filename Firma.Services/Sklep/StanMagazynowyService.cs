using System;
using System.Collections.Generic;
using System.Text;

using Firma.Data.Data;
using Firma.Data.Data.Sklep;
using Firma.Interfaces.Sklep;
using Firma.Services.Abstrakcja;
using Microsoft.EntityFrameworkCore;

namespace Firma.Services.Sklep
{
    public class StanMagazynowyService : BaseService, IStanMagazynowyService
    {
        public StanMagazynowyService(FirmaContext context)
            : base(context)
        {
        }

        public async Task<IList<StanMagazynowy>> GetStanyMagazynowe()
        {
            // Pobieram aktywne stany magazynowe
            var stany = await _context.StanMagazynowy
                .Where(s =>
                    s.CzyAktywny &&
                    s.Towar != null &&
                    s.Towar.CzyAktywny)
                .Include(s => s.Towar)
                    .ThenInclude(t => t.Rodzaj)
                .Include(s => s.Towar)
                    .ThenInclude(t => t.Producent)
                .OrderBy(s => s.Towar != null ? s.Towar.Nazwa : "")
                .ToListAsync();

            return stany;
        }

        public async Task<StanMagazynowy?> GetStanMagazynowy(int idStanuMagazynowego)
        {
            // Pobieram jeden aktywny stan magazynowy
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

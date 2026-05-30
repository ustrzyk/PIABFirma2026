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
    public class ProducentService : BaseService, IProducentService
    {
        public ProducentService(FirmaContext context)
            : base(context)
        {
        }

        public async Task<IList<Producent>> GetProducenci()
        {
            // Pobieram aktywnych producentów
            var producenci = await _context.Producent
                .Where(p => p.CzyAktywny)
                .Include(p => p.Towar)
                .OrderBy(p => p.Nazwa)
                .ToListAsync();

            return producenci;
        }

        public async Task<Producent?> GetProducent(int idProducenta)
        {
            // Pobieram jednego aktywnego producenta
            var producent = await _context.Producent
                .Where(p => p.CzyAktywny)
                .Include(p => p.Towar.Where(t => t.CzyAktywny))
                .FirstOrDefaultAsync(p => p.IdProducenta == idProducenta);

            return producent;
        }
    }
}
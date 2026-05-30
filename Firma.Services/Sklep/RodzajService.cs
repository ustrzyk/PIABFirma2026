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
    public class RodzajService : BaseService, IRodzajService
    {
        public RodzajService(FirmaContext context)
            : base(context)
        {
        }

        public async Task<IList<Rodzaj>> GetRodzaje()
        {
            // Pobiera aktywne rodzaje produktów do menu kategori
            var rodzaje = await _context.Rodzaj
                .Where(r => r.CzyAktywny)
                .OrderBy(r => r.Nazwa)
                .ToListAsync();

            return rodzaje;
        }
    }
}
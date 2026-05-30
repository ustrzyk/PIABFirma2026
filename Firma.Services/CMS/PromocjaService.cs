using System;
using System.Collections.Generic;
using System.Text;

using Firma.Data.Data;
using Firma.Data.Data.CMS;
using Firma.Interfaces.CMS;
using Firma.Services.Abstrakcja;
using Microsoft.EntityFrameworkCore;

namespace Firma.Services.CMS
{
    public class PromocjaService : BaseService, IPromocjaService
    {
        public PromocjaService(FirmaContext context)
            : base(context)
        {
        }

        public async Task<IList<Promocja>> GetPromocje()
        {
            // Pobieram aktywne promocje
            var promocje = await _context.Promocja
                .Where(p => p.CzyAktywny)
                .OrderByDescending(p => p.DataOd)
                .ToListAsync();

            return promocje;
        }

        public async Task<Promocja?> GetPromocja(int idPromocji)
        {
            // Pobieram jedną aktywną promocję
            var promocja = await _context.Promocja
                .Where(p => p.CzyAktywny)
                .FirstOrDefaultAsync(p => p.IdPromocji == idPromocji);

            return promocja;
        }
    }
}
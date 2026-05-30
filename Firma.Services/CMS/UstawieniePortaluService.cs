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
    public class UstawieniePortaluService : BaseService, IUstawieniePortaluService
    {
        public UstawieniePortaluService(FirmaContext context)
            : base(context)
        {
        }

        public async Task<IList<UstawieniePortalu>> GetUstawieniaPortalu()
        {
            // Pobieram aktywne ustawienia portalu
            var ustawienia = await _context.UstawieniePortalu
                .Where(u => u.CzyAktywny)
                .OrderBy(u => u.Klucz)
                .ToListAsync();

            return ustawienia;
        }

        public async Task<UstawieniePortalu?> GetUstawieniePortalu(int idUstawieniaPortalu)
        {
            // Pobieram jedno aktywne ustawienie portalu
            var ustawienie = await _context.UstawieniePortalu
                .Where(u => u.CzyAktywny)
                .FirstOrDefaultAsync(u => u.IdUstawieniaPortalu == idUstawieniaPortalu);

            return ustawienie;
        }
    }
}
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
    public class StronaService : BaseService, IStronaService
    {
        public StronaService(FirmaContext context)
            : base(context)
        {
        }

        public async Task<Strona?> GetStrona(int? idStrony)
        {
            // Jeżeli id jest puste, pobiera pierwszą aktywną stronę według pozycji
            if (idStrony == null)
            {
                return await _context.Strona
                    .Where(s => s.CzyAktywny)
                    .OrderBy(s => s.Pozycja)
                    .FirstOrDefaultAsync();
            }

            // Pobiera aktywną stronę o podanym id
            var strona = await _context.Strona
                .Where(s => s.CzyAktywny)
                .FirstOrDefaultAsync(s => s.IdStrony == idStrony);

            // Jeżeli ktoś poda id strony nieaktywnej albo nieistniejącej
            // pokazuje pierwszą aktywną stronę
            if (strona == null)
            {
                strona = await _context.Strona
                    .Where(s => s.CzyAktywny)
                    .OrderBy(s => s.Pozycja)
                    .FirstOrDefaultAsync();
            }

            return strona;
        }

        public async Task<IList<Strona>> GetStronyByPozycja()
        {
            // Pobiera aktywne strony do menu
            var strony = await _context.Strona
                .Where(s => s.CzyAktywny)
                .OrderBy(s => s.Pozycja)
                .ToListAsync();

            return strony;
        }
    }
}
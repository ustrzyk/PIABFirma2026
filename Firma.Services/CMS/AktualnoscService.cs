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
    public class AktualnoscService : BaseService, IAktualnoscService
    {
        public AktualnoscService(FirmaContext context)
            : base(context)
        {
        }

        public async Task<Aktualnosc?> GetAktualnosc(int idAktualnosci)
        {
            // Pobiera jedną aktywną aktualność
            var aktualnosc = await _context.Aktualnosc
                .Where(a => a.CzyAktywny)
                .FirstOrDefaultAsync(a => a.IdAktualnosci == idAktualnosci);

            return aktualnosc;
        }

        public async Task<IList<Aktualnosc>> GetAktualnoscByPozycjaTake(int ilePobrac)
        {
            // Pobiera aktywne aktualności do layoutu
            var aktualnosci = await _context.Aktualnosc
                .Where(a => a.CzyAktywny)
                .OrderByDescending(a => a.Pozycja)
                .Take(ilePobrac)
                .ToListAsync();

            return aktualnosci;
        }
    }
}
using Firma.Data.Data;
using Firma.Interfaces.CMS;
using Firma.Services.Abstrakcja;
using Firma.Services.Data.Dto.CMS;
using Microsoft.EntityFrameworkCore;

namespace Firma.Services.CMS
{
    public class StronaService : BaseService, IStronaService
    {
        public StronaService(FirmaContext context)
            : base(context)
        {
        }

        public async Task<StronaSzczegolyDto?> GetStrona(int? idStrony)
        {
            if (idStrony == null)
            {
                // Pobieram pierwszą aktywną stronę
                return await _context.Strona
                    .Where(s => s.CzyAktywny)
                    .OrderBy(s => s.Pozycja)
                    .Select(s => new StronaSzczegolyDto
                    {
                        IdStrony = s.IdStrony,
                        LinkTytul = s.LinkTytul,
                        Tytul = s.Tytul,
                        Tresc = s.Tresc,
                        Pozycja = s.Pozycja
                    })
                    .FirstOrDefaultAsync();
            }

            // Pobieram wybraną aktywną stronę
            var strona = await _context.Strona
                .Where(s => s.CzyAktywny)
                .Where(s => s.IdStrony == idStrony)
                .Select(s => new StronaSzczegolyDto
                {
                    IdStrony = s.IdStrony,
                    LinkTytul = s.LinkTytul,
                    Tytul = s.Tytul,
                    Tresc = s.Tresc,
                    Pozycja = s.Pozycja
                })
                .FirstOrDefaultAsync();

            if (strona == null)
            {
                // Wracam do pierwszej aktywnej strony
                strona = await _context.Strona
                    .Where(s => s.CzyAktywny)
                    .OrderBy(s => s.Pozycja)
                    .Select(s => new StronaSzczegolyDto
                    {
                        IdStrony = s.IdStrony,
                        LinkTytul = s.LinkTytul,
                        Tytul = s.Tytul,
                        Tresc = s.Tresc,
                        Pozycja = s.Pozycja
                    })
                    .FirstOrDefaultAsync();
            }

            return strona;
        }

        public async Task<IList<StronaMenuItemDto>> GetStronyByPozycja()
        {
            // Pobieram strony do menu
            var strony = await _context.Strona
                .Where(s => s.CzyAktywny)
                .OrderBy(s => s.Pozycja)
                .Select(s => new StronaMenuItemDto
                {
                    IdStrony = s.IdStrony,
                    LinkTytul = s.LinkTytul,
                    Tytul = s.Tytul,
                    Pozycja = s.Pozycja
                })
                .ToListAsync();

            return strony;
        }
    }
}
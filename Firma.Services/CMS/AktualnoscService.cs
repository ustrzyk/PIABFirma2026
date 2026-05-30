using Firma.Data.Data;
using Firma.Interfaces.CMS;
using Firma.Services.Abstrakcja;
using Firma.Services.Data.Dto.CMS;
using Microsoft.EntityFrameworkCore;

namespace Firma.Services.CMS
{
    public class AktualnoscService : BaseService, IAktualnoscService
    {
        public AktualnoscService(FirmaContext context)
            : base(context)
        {
        }

        public async Task<AktualnoscSzczegolyDto?> GetAktualnosc(int idAktualnosci)
        {
            // Pobieram aktualność do szczegółów
            var aktualnosc = await _context.Aktualnosc
                .Where(a => a.CzyAktywny)
                .Where(a => a.IdAktualnosci == idAktualnosci)
                .Select(a => new AktualnoscSzczegolyDto
                {
                    IdAktualnosci = a.IdAktualnosci,
                    LinkTytul = a.LinkTytul,
                    Tytul = a.Tytul,
                    Tresc = a.Tresc,
                    Pozycja = a.Pozycja
                })
                .FirstOrDefaultAsync();

            return aktualnosc;
        }

        public async Task<IList<AktualnoscListaItemDto>> GetAktualnoscByPozycjaTake(int ilePobrac)
        {
            // Pobieram aktualności do layoutu
            var aktualnosci = await _context.Aktualnosc
                .Where(a => a.CzyAktywny)
                .OrderByDescending(a => a.Pozycja)
                .Take(ilePobrac)
                .Select(a => new AktualnoscListaItemDto
                {
                    IdAktualnosci = a.IdAktualnosci,
                    LinkTytul = a.LinkTytul,
                    Tytul = a.Tytul,
                    Tresc = a.Tresc,
                    Pozycja = a.Pozycja
                })
                .ToListAsync();

            return aktualnosci;
        }
    }
}
using Firma.Services.Data.Dto.CMS;

namespace Firma.Interfaces.CMS
{
    public interface IAktualnoscService
    {
        // Pobieram aktualności do layoutu
        Task<IList<AktualnoscListaItemDto>> GetAktualnoscByPozycjaTake(int ilePobrac);

        // Pobieram aktualność do szczegółów
        Task<AktualnoscSzczegolyDto?> GetAktualnosc(int idAktualnosci);
    }
}
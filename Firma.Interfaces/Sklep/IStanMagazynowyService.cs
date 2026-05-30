using Firma.Services.Data.Dto.StanyMagazynowe;

namespace Firma.Interfaces.Sklep
{
    public interface IStanMagazynowyService
    {
        // Pobieram stany magazynowe do listy
        Task<IList<StanMagazynowyListaItemDto>> GetStanyMagazynowe();

        // Pobieram stan magazynowy do szczegółów
        Task<StanMagazynowySzczegolyDto?> GetStanMagazynowy(int idStanuMagazynowego);
    }
}
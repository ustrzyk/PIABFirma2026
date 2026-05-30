using Firma.Data.Data.Sklep;
using Firma.Services.Data.Dto.StanyMagazynowe;

namespace Firma.Interfaces.Sklep
{
    public interface IStanMagazynowyService
    {
        // Pobieram stany magazynowe do listy
        Task<IList<StanMagazynowyListaItemDto>> GetStanyMagazynowe();

        // Pobieram jeden aktywny stan magazynowy
        Task<StanMagazynowy?> GetStanMagazynowy(int idStanuMagazynowego);
    }
}
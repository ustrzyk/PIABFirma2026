using Firma.Data.Data.Sklep;
using Firma.Services.Data.Dto.Producenci;

namespace Firma.Interfaces.Sklep
{
    public interface IProducentService
    {
        // Pobieram producentów do listy
        Task<IList<ProducentListaItemDto>> GetProducenci();

        // Pobieram jednego aktywnego producenta
        Task<Producent?> GetProducent(int idProducenta);
    }
}
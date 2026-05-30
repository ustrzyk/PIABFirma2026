using Firma.Services.Data.Dto.Producenci;

namespace Firma.Interfaces.Sklep
{
    public interface IProducentService
    {
        // Pobieram producentów do listy
        Task<IList<ProducentListaItemDto>> GetProducenci();

        // Pobieram producenta do szczegółów
        Task<ProducentSzczegolyDto?> GetProducent(int idProducenta);
    }
}
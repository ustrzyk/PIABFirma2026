using Firma.Services.Data.Dto.Promocje;

namespace Firma.Interfaces.CMS
{
    public interface IPromocjaService
    {
        // Pobieram promocje do listy
        Task<IList<PromocjaListaItemDto>> GetPromocje();

        // Pobieram promocję do szczegółów
        Task<PromocjaSzczegolyDto?> GetPromocja(int idPromocji);
    }
}
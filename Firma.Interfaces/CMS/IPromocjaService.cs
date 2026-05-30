using Firma.Data.Data.CMS;
using Firma.Services.Data.Dto.Promocje;

namespace Firma.Interfaces.CMS
{
    public interface IPromocjaService
    {
        // Pobieram promocje do listy
        Task<IList<PromocjaListaItemDto>> GetPromocje();

        // Pobieram jedną aktywną promocję
        Task<Promocja?> GetPromocja(int idPromocji);
    }
}
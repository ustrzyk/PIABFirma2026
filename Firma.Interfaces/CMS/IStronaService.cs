using Firma.Services.Data.Dto.CMS;

namespace Firma.Interfaces.CMS
{
    public interface IStronaService
    {
        // Pobieram strony do menu
        Task<IList<StronaMenuItemDto>> GetStronyByPozycja();

        // Pobieram stronę do szczegółów
        Task<StronaSzczegolyDto?> GetStrona(int? idStrony);
    }
}
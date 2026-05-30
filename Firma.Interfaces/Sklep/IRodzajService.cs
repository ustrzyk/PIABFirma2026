using Firma.Services.Data.Dto.Rodzaje;

namespace Firma.Interfaces.Sklep
{
    public interface IRodzajService
    {
        // Pobieram aktywne rodzaje do menu
        Task<IList<RodzajMenuItemDto>> GetRodzaje();
    }
}
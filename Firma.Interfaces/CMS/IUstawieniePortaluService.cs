using Firma.Services.Data.Dto.UstawieniaPortalu;

namespace Firma.Interfaces.CMS
{
    public interface IUstawieniePortaluService
    {
        // Pobieram ustawienia portalu do listy
        Task<IList<UstawieniePortaluListaItemDto>> GetUstawieniaPortalu();

        // Pobieram ustawienie portalu do szczegółów
        Task<UstawieniePortaluSzczegolyDto?> GetUstawieniePortalu(int idUstawieniaPortalu);
    }
}
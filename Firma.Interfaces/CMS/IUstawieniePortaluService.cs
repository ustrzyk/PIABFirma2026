using Firma.Data.Data.CMS;
using Firma.Services.Data.Dto.UstawieniaPortalu;

namespace Firma.Interfaces.CMS
{
    public interface IUstawieniePortaluService
    {
        // Pobieram ustawienia portalu do listy
        Task<IList<UstawieniePortaluListaItemDto>> GetUstawieniaPortalu();

        // Pobieram jedno aktywne ustawienie portalu
        Task<UstawieniePortalu?> GetUstawieniePortalu(int idUstawieniaPortalu);
    }
}
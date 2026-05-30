using Firma.Data.Data.Sklep;
using Firma.Services.Data.Dto.Towary;

namespace Firma.Interfaces.Sklep
{
    public interface ITowarService
    {
        // Pobieram jeden aktywny towar
        Task<Towar?> GetTowar(int idTowaru);

        // Pobieram aktywne towary dla listy sklepu
        Task<IList<TowarListaItemDto>> GetTowaryDanegoRodzaju(int? idRodzaju);

        // Pobieram aktywne towary do DTO
        Task<IList<TowarListaItemDto>> GetTowary();
    }
}
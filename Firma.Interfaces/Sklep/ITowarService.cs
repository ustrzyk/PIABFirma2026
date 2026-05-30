using System;
using System.Collections.Generic;
using System.Text;

using Firma.Data.Data.Sklep;
using Firma.Services.Data.Dto.Towary;

namespace Firma.Interfaces.Sklep
{
    public interface ITowarService
    {
        // Pobiera jeden aktywny towar po id
        Task<Towar?> GetTowar(int idTowaru);

        // Pobiera aktywne towary danego rodzaju
        Task<IList<Towar>> GetTowaryDanegoRodzaju(int? idRodzaju);

        // Pobiera aktywne towary do prostego DTO
        Task<IList<TowarListaItemDto>> GetTowary();
    }
}
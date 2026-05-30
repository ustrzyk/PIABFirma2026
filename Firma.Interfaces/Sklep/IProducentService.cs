using System;
using System.Collections.Generic;
using System.Text;

using Firma.Data.Data.Sklep;

namespace Firma.Interfaces.Sklep
{
    public interface IProducentService
    {
        // Pobieram aktywnych producentów
        Task<IList<Producent>> GetProducenci();

        // Pobieram jednego aktywnego producenta
        Task<Producent?> GetProducent(int idProducenta);
    }
}
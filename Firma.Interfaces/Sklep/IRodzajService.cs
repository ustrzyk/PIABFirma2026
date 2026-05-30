using System;
using System.Collections.Generic;
using System.Text;

using Firma.Data.Data.Sklep;

namespace Firma.Interfaces.Sklep
{
    public interface IRodzajService
    {
        // Pobiera aktywne rodzaje towarów do menu kategorii
        Task<IList<Rodzaj>> GetRodzaje();
    }
}
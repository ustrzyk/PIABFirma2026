using System;
using System.Collections.Generic;
using System.Text;

using Firma.Data.Data.CMS;

namespace Firma.Interfaces.CMS
{
    public interface IStronaService
    {
        // Pobiera aktywne strony do menu portalu
        Task<IList<Strona>> GetStronyByPozycja();

        // Pobiera jedną aktywną stronę
        Task<Strona?> GetStrona(int? idStrony);
    }
}

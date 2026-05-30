using System;
using System.Collections.Generic;
using System.Text;

using Firma.Data.Data.CMS;

namespace Firma.Interfaces.CMS
{
    public interface IUstawieniePortaluService
    {
        // Pobieram aktywne ustawienia portalu
        Task<IList<UstawieniePortalu>> GetUstawieniaPortalu();

        // Pobieram jedno aktywne ustawienie portalu
        Task<UstawieniePortalu?> GetUstawieniePortalu(int idUstawieniaPortalu);
    }
}
using System;
using System.Collections.Generic;
using System.Text;

using Firma.Data.Data.Sklep;

namespace Firma.Interfaces.Sklep
{
    public interface IStanMagazynowyService
    {
        // Pobieram aktywne stany magazynowe
        Task<IList<StanMagazynowy>> GetStanyMagazynowe();

        // Pobieram jeden aktywny stan magazynowy
        Task<StanMagazynowy?> GetStanMagazynowy(int idStanuMagazynowego);
    }
}
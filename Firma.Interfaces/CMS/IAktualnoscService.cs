using System;
using System.Collections.Generic;
using System.Text;

using Firma.Data.Data.CMS;

namespace Firma.Interfaces.CMS
{
    public interface IAktualnoscService
    {
        // Pobiera najnowsze aktywne aktualności do layoutu
        Task<IList<Aktualnosc>> GetAktualnoscByPozycjaTake(int ilePobrac);

        // Pobiera jedną aktywną aktualność
        Task<Aktualnosc?> GetAktualnosc(int idAktualnosci);
    }
}

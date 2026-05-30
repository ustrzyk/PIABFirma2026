using System;
using System.Collections.Generic;
using System.Text;

using Firma.Data.Data.CMS;

namespace Firma.Interfaces.CMS
{
    public interface IPromocjaService
    {
        // Pobieram aktywne promocje
        Task<IList<Promocja>> GetPromocje();

        // Pobieram jedną aktywną promocję
        Task<Promocja?> GetPromocja(int idPromocji);
    }
}

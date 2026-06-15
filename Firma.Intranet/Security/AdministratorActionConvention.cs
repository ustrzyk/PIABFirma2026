using Microsoft.AspNetCore.Mvc.ApplicationModels;
using Microsoft.AspNetCore.Mvc.Authorization;

namespace Firma.Intranet.Security
{
    public class AdministratorActionConvention : IActionModelConvention
    {
        private static readonly HashSet<string> AkcjeAdministratora = new HashSet<string>(StringComparer.OrdinalIgnoreCase)
        {
            "Delete",
            "Aktywuj",
            "Dezaktywuj",
            "UsunZaznaczone",
            "DezaktywujZaznaczone",
            "AktywujZaznaczone",
            "ImportExcel",
            "PobierzSzablonImportuExcel"
        };

        public void Apply(ActionModel action)
        {
            if (AkcjeAdministratora.Contains(action.ActionName))
            {
                action.Filters.Add(new AuthorizeFilter("AdministratorOnly"));
            }
        }
    }
}
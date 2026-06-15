using Microsoft.AspNetCore.Http;

namespace Firma.Intranet.Models
{
    public class ImportZamowienExcelViewModel
    {
        public IFormFile? Plik { get; set; }

        public bool CzyWykonanoImport { get; set; }

        public int LiczbaDodanychZamowien { get; set; }

        public int LiczbaDodanychKlientow { get; set; }

        public List<string> Bledy { get; set; } = new List<string>();
    }
}
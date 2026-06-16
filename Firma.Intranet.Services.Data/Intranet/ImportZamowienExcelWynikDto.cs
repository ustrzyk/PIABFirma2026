namespace Firma.Intranet.Services.Data.Intranet
{
    public class ImportZamowienExcelWynikDto
    {
        public bool CzyWykonanoImport { get; set; }

        public int LiczbaDodanychZamowien { get; set; }

        public int LiczbaDodanychKlientow { get; set; }

        public List<string> Bledy { get; set; } = new List<string>();
    }
}
namespace Firma.Services.Data.Dto.ZamowieniaPubliczne
{
    public class ZamowienieKlientaListaItemDto
    {
        public int IdZamowienia { get; set; }

        public string NumerZamowienia { get; set; } = string.Empty;

        public DateTime? DataZamowienia { get; set; }

        public string Status { get; set; } = string.Empty;

        public decimal WartoscRazem { get; set; }

        public int LiczbaPozycji { get; set; }
    }
}
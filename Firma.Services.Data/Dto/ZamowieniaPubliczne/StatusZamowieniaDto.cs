namespace Firma.Services.Data.Dto.ZamowieniaPubliczne
{
    public class StatusZamowieniaDto
    {
        public string NumerZamowienia { get; set; } = string.Empty;

        public DateTime? DataZamowienia { get; set; }

        public string Status { get; set; } = string.Empty;

        public decimal WartoscRazem { get; set; }

        public string ImieNazwisko { get; set; } = string.Empty;

        public string Email { get; set; } = string.Empty;

        public string Telefon { get; set; } = string.Empty;

        public string AdresDostawy { get; set; } = string.Empty;

        public IList<StatusZamowieniaPozycjaDto> Pozycje { get; set; } =
            new List<StatusZamowieniaPozycjaDto>();
    }
}
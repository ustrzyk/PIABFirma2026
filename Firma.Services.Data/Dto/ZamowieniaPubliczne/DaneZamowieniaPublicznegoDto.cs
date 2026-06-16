namespace Firma.Services.Data.Dto.ZamowieniaPubliczne
{
    public class DaneZamowieniaPublicznegoDto
    {
        public string Imie { get; set; } = string.Empty;

        public string Nazwisko { get; set; } = string.Empty;

        public string Email { get; set; } = string.Empty;

        public string Telefon { get; set; } = string.Empty;

        public string Ulica { get; set; } = string.Empty;

        public string NumerDomu { get; set; } = string.Empty;

        public string NumerLokalu { get; set; } = string.Empty;

        public string KodPocztowy { get; set; } = string.Empty;

        public string Miasto { get; set; } = string.Empty;

        public IList<PozycjaZamowieniaPublicznegoDto> Pozycje { get; set; } =
            new List<PozycjaZamowieniaPublicznegoDto>();
    }
}
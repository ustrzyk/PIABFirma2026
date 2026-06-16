namespace Firma.Services.Data.Dto.ZamowieniaPubliczne
{
    public class StatusZamowieniaPozycjaDto
    {
        public string KodTowaru { get; set; } = string.Empty;

        public string NazwaTowaru { get; set; } = string.Empty;

        public int Ilosc { get; set; }

        public decimal CenaJednostkowa { get; set; }

        public decimal Wartosc
        {
            get
            {
                return Ilosc * CenaJednostkowa;
            }
        }
    }
}
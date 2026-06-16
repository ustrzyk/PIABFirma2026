namespace Firma.Services.Data.Dto.ZamowieniaPubliczne
{
    public class ZamowieniePubliczneWynikDto
    {
        public bool CzySukces { get; set; }

        public string NumerZamowienia { get; set; } = string.Empty;

        public string KomunikatBledu { get; set; } = string.Empty;

        public static ZamowieniePubliczneWynikDto Sukces(string numerZamowienia)
        {
            return new ZamowieniePubliczneWynikDto
            {
                CzySukces = true,
                NumerZamowienia = numerZamowienia
            };
        }

        public static ZamowieniePubliczneWynikDto Blad(string komunikat)
        {
            return new ZamowieniePubliczneWynikDto
            {
                CzySukces = false,
                KomunikatBledu = komunikat
            };
        }
    }
}
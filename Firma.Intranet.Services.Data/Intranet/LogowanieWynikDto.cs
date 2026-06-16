namespace Firma.Intranet.Services.Data.Intranet
{
    public class LogowanieWynikDto
    {
        public bool CzySukces { get; set; }

        public bool CzyZablokowany { get; set; }

        public string KomunikatBledu { get; set; } = string.Empty;

        public static LogowanieWynikDto Sukces()
        {
            return new LogowanieWynikDto
            {
                CzySukces = true
            };
        }

        public static LogowanieWynikDto Blad(string komunikat)
        {
            return new LogowanieWynikDto
            {
                CzySukces = false,
                KomunikatBledu = komunikat
            };
        }

        public static LogowanieWynikDto Zablokowany()
        {
            return new LogowanieWynikDto
            {
                CzySukces = false,
                CzyZablokowany = true,
                KomunikatBledu = "Konto jest tymczasowo zablokowane."
            };
        }
    }
}
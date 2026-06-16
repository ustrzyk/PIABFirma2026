namespace Firma.Intranet.Services.Data.Intranet
{
    public class OperacjaUzytkownikaWynikDto
    {
        public bool CzySukces { get; set; }

        public bool CzyZnaleziono { get; set; } = true;

        public List<string> Bledy { get; set; } = new List<string>();

        public static OperacjaUzytkownikaWynikDto Powodzenie()
        {
            return new OperacjaUzytkownikaWynikDto
            {
                CzySukces = true,
                CzyZnaleziono = true
            };
        }

        public static OperacjaUzytkownikaWynikDto Niepowodzenie(IEnumerable<string> bledy)
        {
            return new OperacjaUzytkownikaWynikDto
            {
                CzySukces = false,
                CzyZnaleziono = true,
                Bledy = bledy.ToList()
            };
        }

        public static OperacjaUzytkownikaWynikDto NieZnaleziono()
        {
            return new OperacjaUzytkownikaWynikDto
            {
                CzySukces = false,
                CzyZnaleziono = false
            };
        }
    }
}
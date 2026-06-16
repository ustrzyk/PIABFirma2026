namespace Firma.Intranet.Services.Data.Intranet
{
    public class UzytkownikEdycjaDto
    {
        public string Id { get; set; } = string.Empty;

        public string Email { get; set; } = string.Empty;

        public string Rola { get; set; } = string.Empty;

        public bool CzyAktualnieZalogowany { get; set; }

        public List<string> DostepneRole { get; set; } = new List<string>();
    }
}
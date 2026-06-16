namespace Firma.Intranet.Services.Data.Intranet
{
    public class UzytkownikUsuniecieDto
    {
        public string Id { get; set; } = string.Empty;

        public string Email { get; set; } = string.Empty;

        public IList<string> Role { get; set; } = new List<string>();

        public bool CzyAktualnieZalogowany { get; set; }
    }
}
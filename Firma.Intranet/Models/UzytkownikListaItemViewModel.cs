namespace Firma.Intranet.Models
{
    public class UzytkownikListaItemViewModel
    {
        public string Id { get; set; } = string.Empty;

        public string Email { get; set; } = string.Empty;

        public string NazwaUzytkownika { get; set; } = string.Empty;

        public IList<string> Role { get; set; } = new List<string>();

        public bool CzyAktualnieZalogowany { get; set; }
    }
}
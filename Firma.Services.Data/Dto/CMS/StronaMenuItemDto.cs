namespace Firma.Services.Data.Dto.CMS
{
    public class StronaMenuItemDto
    {
        public int IdStrony { get; set; }

        public string LinkTytul { get; set; } = string.Empty;

        public string Tytul { get; set; } = string.Empty;

        public int Pozycja { get; set; }
    }
}
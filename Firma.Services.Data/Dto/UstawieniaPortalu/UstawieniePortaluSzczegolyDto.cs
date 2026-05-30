namespace Firma.Services.Data.Dto.UstawieniaPortalu
{
    public class UstawieniePortaluSzczegolyDto
    {
        public int IdUstawieniaPortalu { get; set; }

        public string Klucz { get; set; } = string.Empty;

        public string Wartosc { get; set; } = string.Empty;

        public string Opis { get; set; } = string.Empty;
    }
}
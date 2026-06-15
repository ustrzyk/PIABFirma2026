namespace Firma.Intranet.Services.Data.Intranet
{
    public class PlikZalacznikaDto
    {
        public Stream Stream { get; set; } = Stream.Null;

        public string NazwaOryginalna { get; set; } = string.Empty;

        public string ContentType { get; set; } = string.Empty;

        public long Rozmiar { get; set; }
    }
}
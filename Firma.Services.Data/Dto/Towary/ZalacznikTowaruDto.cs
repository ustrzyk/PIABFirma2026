using System;

namespace Firma.Services.Data.Dto.Towary
{
    public class ZalacznikTowaruDto
    {
        public int IdZalacznikaTowaru { get; set; }

        public string NazwaOryginalna { get; set; } = string.Empty;

        public string Sciezka { get; set; } = string.Empty;

        public string TypPliku { get; set; } = string.Empty;

        public long Rozmiar { get; set; }

        public string Opis { get; set; } = string.Empty;

        public DateTime DataDodania { get; set; }

        public string RozmiarTekst
        {
            get
            {
                return $"{Rozmiar / 1024.0:N1} KB";
            }
        }

        public bool CzyObraz
        {
            get
            {
                return TypPliku.Contains("image", StringComparison.OrdinalIgnoreCase);
            }
        }

        public bool CzyPdf
        {
            get
            {
                return TypPliku.Contains("pdf", StringComparison.OrdinalIgnoreCase);
            }
        }
    }
}
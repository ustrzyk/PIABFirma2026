namespace Firma.Services.Data.Dto.StanyMagazynowe
{
    public class StanMagazynowySzczegolyDto
    {
        public int IdStanuMagazynowego { get; set; }

        public int IloscSztuk { get; set; }

        public int MinimalnaIlosc { get; set; }

        public string Lokalizacja { get; set; } = string.Empty;

        public int IdTowaru { get; set; }

        public string KodTowaru { get; set; } = string.Empty;

        public string NazwaTowaru { get; set; } = string.Empty;

        public decimal CenaTowaru { get; set; }

        public string OpisTowaru { get; set; } = string.Empty;

        public string Rodzaj { get; set; } = string.Empty;

        public string Producent { get; set; } = string.Empty;

        public string FotoUrl { get; set; } = string.Empty;

        public bool CzyDostepny
        {
            get
            {
                return IloscSztuk > 0;
            }
        }

        public bool CzyMaloTowaru
        {
            get
            {
                return IloscSztuk <= MinimalnaIlosc;
            }
        }
    }
}
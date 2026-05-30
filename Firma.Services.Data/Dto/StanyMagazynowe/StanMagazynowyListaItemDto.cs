using System;
using System.Collections.Generic;
using System.Text;

namespace Firma.Services.Data.Dto.StanyMagazynowe
{
    public class StanMagazynowyListaItemDto
    {
        public int IdStanuMagazynowego { get; set; }

        public string NazwaTowaru { get; set; } = string.Empty;

        public string KodTowaru { get; set; } = string.Empty;

        public string Rodzaj { get; set; } = string.Empty;

        public string Producent { get; set; } = string.Empty;

        public int IloscSztuk { get; set; }

        public int MinimalnaIlosc { get; set; }

        public string Lokalizacja { get; set; } = string.Empty;

        public bool CzyMaloTowaru
        {
            get
            {
                return IloscSztuk <= MinimalnaIlosc;
            }
        }
    }
}
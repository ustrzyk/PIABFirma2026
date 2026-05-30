using System;
using System.Collections.Generic;
using System.Text;

namespace Firma.Services.Data.Dto.Towary
{
    public class TowarSzczegolyDto
    {
        public int IdTowaru { get; set; }

        public string Kod { get; set; } = string.Empty;

        public string Nazwa { get; set; } = string.Empty;

        public decimal Cena { get; set; }

        public string FotoUrl { get; set; } = string.Empty;

        public string Opis { get; set; } = string.Empty;

        public string Rodzaj { get; set; } = string.Empty;

        public string Producent { get; set; } = string.Empty;

        public string KrajProducenta { get; set; } = string.Empty;

        public string StronaWWWProducenta { get; set; } = string.Empty;

        public int? IloscSztuk { get; set; }

        public int? MinimalnaIlosc { get; set; }

        public string Lokalizacja { get; set; } = string.Empty;

        public bool CzyDostepny
        {
            get
            {
                return IloscSztuk != null && IloscSztuk > 0;
            }
        }

        public bool CzyMaloTowaru
        {
            get
            {
                return IloscSztuk != null &&
                       MinimalnaIlosc != null &&
                       IloscSztuk <= MinimalnaIlosc;
            }
        }
    }
}
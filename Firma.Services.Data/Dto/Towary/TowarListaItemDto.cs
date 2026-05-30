using System;
using System.Collections.Generic;
using System.Text;

namespace Firma.Services.Data.Dto.Towary
{
    public class TowarListaItemDto
    {
        public int IdTowaru { get; set; }

        public string Kod { get; set; } = string.Empty;

        public string Nazwa { get; set; } = string.Empty;

        public decimal Cena { get; set; }

        public string FotoUrl { get; set; } = string.Empty;

        public string Opis { get; set; } = string.Empty;

        public string Rodzaj { get; set; } = string.Empty;

        public string Producent { get; set; } = string.Empty;

        public int? IloscSztuk { get; set; }

        public bool CzyDostepny
        {
            get
            {
                return IloscSztuk != null && IloscSztuk > 0;
            }
        }
    }
}
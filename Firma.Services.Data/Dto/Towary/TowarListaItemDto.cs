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

        public string Rodzaj { get; set; } = string.Empty;

        public string Producent { get; set; } = string.Empty;
    }
}
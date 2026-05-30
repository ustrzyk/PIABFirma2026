using System;
using System.Collections.Generic;
using System.Text;

namespace Firma.Services.Data.Dto.Producenci
{
    public class ProducentListaItemDto
    {
        public int IdProducenta { get; set; }

        public string Nazwa { get; set; } = string.Empty;

        public string Kraj { get; set; } = string.Empty;

        public string StronaWWW { get; set; } = string.Empty;

        public string Opis { get; set; } = string.Empty;

        public int IloscTowarow { get; set; }
    }
}
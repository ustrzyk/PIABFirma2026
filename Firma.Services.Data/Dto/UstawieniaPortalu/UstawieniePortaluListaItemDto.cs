using System;
using System.Collections.Generic;
using System.Text;

namespace Firma.Services.Data.Dto.UstawieniaPortalu
{
    public class UstawieniePortaluListaItemDto
    {
        public int IdUstawieniaPortalu { get; set; }

        public string Klucz { get; set; } = string.Empty;

        public string Wartosc { get; set; } = string.Empty;

        public string Opis { get; set; } = string.Empty;
    }
}
using System;
using System.Collections.Generic;
using System.Text;

namespace Firma.Services.Data.Dto.Promocje
{
    public class PromocjaListaItemDto
    {
        public int IdPromocji { get; set; }

        public string Tytul { get; set; } = string.Empty;

        public string Opis { get; set; } = string.Empty;

        public int RabatProcentowy { get; set; }

        public DateTime? DataOd { get; set; }

        public DateTime? DataDo { get; set; }
    }
}
using System;
using System.Collections.Generic;
using System.Text;

namespace Firma.Services.Data.Dto.CMS
{
    public class PortalWygladDto
    {
        public string NazwaPortalu { get; set; } = "Sklep 3D";

        public string StopkaTekst { get; set; } = "Sklep z drukarkami 3D";

        public string StopkaAdres { get; set; } = string.Empty;

        public string StopkaEmail { get; set; } = string.Empty;

        public string StopkaTelefon { get; set; } = string.Empty;

        public string StopkaFacebook { get; set; } = string.Empty;

        public string KolorTlaPortalu { get; set; } = "#eef2f6";

        public string KolorNawigacji { get; set; } = "#ffffff";

        public string KolorStopki { get; set; } = "#f8f9fa";

        public string KolorPrzyciskow { get; set; } = "#0d6efd";

        public string KolorAkcentu { get; set; } = "#258cfb";
    }
}
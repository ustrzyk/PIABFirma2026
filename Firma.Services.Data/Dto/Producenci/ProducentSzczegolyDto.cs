using Firma.Services.Data.Dto.Towary;

namespace Firma.Services.Data.Dto.Producenci
{
    public class ProducentSzczegolyDto
    {
        public int IdProducenta { get; set; }

        public string Nazwa { get; set; } = string.Empty;

        public string Kraj { get; set; } = string.Empty;

        public string StronaWWW { get; set; } = string.Empty;

        public string Opis { get; set; } = string.Empty;

        public IList<TowarListaItemDto> Towary { get; set; } = new List<TowarListaItemDto>();

        public int IloscTowarow
        {
            get
            {
                return Towary.Count;
            }
        }
    }
}
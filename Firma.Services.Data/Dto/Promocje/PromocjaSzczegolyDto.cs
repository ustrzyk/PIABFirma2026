namespace Firma.Services.Data.Dto.Promocje
{
    public class PromocjaSzczegolyDto
    {
        public int IdPromocji { get; set; }

        public string Tytul { get; set; } = string.Empty;

        public string Opis { get; set; } = string.Empty;

        public int RabatProcentowy { get; set; }

        public DateTime? DataOd { get; set; }

        public DateTime? DataDo { get; set; }

        public bool CzyAktualna
        {
            get
            {
                var dzisiaj = DateTime.Today;

                var dataOdOk = DataOd == null || DataOd.Value.Date <= dzisiaj;
                var dataDoOk = DataDo == null || DataDo.Value.Date >= dzisiaj;

                return dataOdOk && dataDoOk;
            }
        }
    }
}
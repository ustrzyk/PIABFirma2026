using Firma.Intranet.Services.Data.Intranet;

namespace Firma.Intranet.Interfaces.Intranet
{
    public interface IKontoIntranetService
    {
        Task<LogowanieWynikDto> Zaloguj(
            string email,
            string haslo,
            bool zapamietajMnie);

        Task Wyloguj();
    }
}
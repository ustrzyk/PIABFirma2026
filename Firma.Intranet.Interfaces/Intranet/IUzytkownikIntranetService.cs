using Firma.Intranet.Services.Data.Intranet;

namespace Firma.Intranet.Interfaces.Intranet
{
    public interface IUzytkownikIntranetService
    {
        Task<List<UzytkownikListaItemDto>> PobierzListe(string? idAktualnegoUzytkownika);

        Task<List<string>> PobierzRole();

        Task<OperacjaUzytkownikaWynikDto> Dodaj(string email, string haslo, string rola);

        Task<UzytkownikEdycjaDto?> PobierzDoEdycji(string id, string? idAktualnegoUzytkownika);

        Task<OperacjaUzytkownikaWynikDto> Aktualizuj(
            string id,
            string email,
            string rola,
            string? idAktualnegoUzytkownika);

        Task<string?> PobierzEmail(string id);

        Task<OperacjaUzytkownikaWynikDto> ResetujHaslo(string id, string noweHaslo);

        Task<UzytkownikUsuniecieDto?> PobierzDoUsuniecia(string id, string? idAktualnegoUzytkownika);

        Task<OperacjaUzytkownikaWynikDto> Usun(string id, string? idAktualnegoUzytkownika);
    }
}
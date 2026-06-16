using Firma.Services.Data.Dto.ZamowieniaPubliczne;

namespace Firma.Interfaces.Sklep
{
    public interface IKontoKlientaService
    {
        Task UtworzLubAktualizujKlienta(string email, string imie, string nazwisko, string telefon);

        Task<List<ZamowienieKlientaListaItemDto>> PobierzZamowieniaKlienta(string email);

        Task<StatusZamowieniaDto?> PobierzSzczegolyZamowieniaKlienta(string email, int idZamowienia);
    }
}
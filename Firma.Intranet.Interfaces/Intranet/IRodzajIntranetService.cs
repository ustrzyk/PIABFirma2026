using Firma.Data.Data.Sklep;

namespace Firma.Intranet.Interfaces.Intranet
{
    public interface IRodzajIntranetService
    {
        Task<List<Rodzaj>> PobierzListe();

        Task<Rodzaj?> PobierzSzczegoly(int id);

        Task<Rodzaj?> PobierzDoEdycji(int id);

        Task Dodaj(Rodzaj rodzaj);

        Task<bool> Aktualizuj(int id, Rodzaj rodzaj);

        Task<Rodzaj?> PobierzDoUsuniecia(int id);

        Task UsunAlboDezaktywuj(int id);

        Task Aktywuj(int id);

        Task Dezaktywuj(int id);
    }
}
using Firma.Data.Data.Sklep;

namespace Firma.Intranet.Interfaces.Intranet
{
    public interface IProducentIntranetService
    {
        Task<List<Producent>> PobierzListe();

        Task<Producent?> PobierzSzczegoly(int id);

        Task<Producent?> PobierzDoEdycji(int id);

        Task Dodaj(Producent producent);

        Task<bool> Aktualizuj(int id, Producent producent);

        Task<Producent?> PobierzDoUsuniecia(int id);

        Task UsunAlboDezaktywuj(int id);

        Task Aktywuj(int id);

        Task Dezaktywuj(int id);
    }
}
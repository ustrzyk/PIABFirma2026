using Firma.Data.Data.CMS;

namespace Firma.Intranet.Interfaces.Intranet
{
    public interface IStronaIntranetService
    {
        Task<List<Strona>> PobierzListe();

        Task<Strona?> PobierzSzczegoly(int id);

        Task<Strona?> PobierzDoEdycji(int id);

        Task Dodaj(Strona strona);

        Task<bool> Aktualizuj(int id, Strona strona);

        Task<Strona?> PobierzDoUsuniecia(int id);

        Task Usun(int id);

        Task Aktywuj(int id);

        Task Dezaktywuj(int id);
    }
}
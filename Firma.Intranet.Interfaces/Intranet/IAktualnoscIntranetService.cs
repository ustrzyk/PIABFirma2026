using Firma.Data.Data.CMS;

namespace Firma.Intranet.Interfaces.Intranet
{
    public interface IAktualnoscIntranetService
    {
        Task<List<Aktualnosc>> PobierzListe();

        Task<Aktualnosc?> PobierzSzczegoly(int id);

        Task<Aktualnosc?> PobierzDoEdycji(int id);

        Task Dodaj(Aktualnosc aktualnosc);

        Task<bool> Aktualizuj(int id, Aktualnosc aktualnosc);

        Task<Aktualnosc?> PobierzDoUsuniecia(int id);

        Task Usun(int id);

        Task Aktywuj(int id);

        Task Dezaktywuj(int id);
    }
}
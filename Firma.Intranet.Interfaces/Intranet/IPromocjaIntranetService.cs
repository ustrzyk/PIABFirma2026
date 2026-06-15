using Firma.Data.Data.CMS;

namespace Firma.Intranet.Interfaces.Intranet
{
    public interface IPromocjaIntranetService
    {
        Task<List<Promocja>> PobierzListe();

        Task<Promocja?> PobierzSzczegoly(int id);

        Task<Promocja?> PobierzDoEdycji(int id);

        Task Dodaj(Promocja promocja);

        Task<bool> Aktualizuj(int id, Promocja promocja);

        Task<Promocja?> PobierzDoUsuniecia(int id);

        Task Usun(int id);

        Task Aktywuj(int id);

        Task Dezaktywuj(int id);
    }
}
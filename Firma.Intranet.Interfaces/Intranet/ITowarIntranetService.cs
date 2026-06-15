using Firma.Data.Data.Sklep;
using Firma.Intranet.Services.Data.Intranet;

namespace Firma.Intranet.Interfaces.Intranet
{
    public interface ITowarIntranetService
    {
        Task<List<Towar>> PobierzListe();

        Task<Towar?> PobierzSzczegoly(int id);

        Task<Towar?> PobierzDoEdycji(int id);

        Task Dodaj(Towar towar);

        Task<bool> Aktualizuj(int id, Towar towar);

        Task<Towar?> PobierzDoUsuniecia(int id);

        Task Usun(int id, string folderUploadu);

        Task UsunZaznaczone(int[] ids, string folderUploadu);

        Task Dezaktywuj(int id);

        Task Aktywuj(int id);

        Task DezaktywujZaznaczone(int[] ids);

        Task AktywujZaznaczone(int[] ids);

        Task<List<ProducentSelectItemDto>> PobierzProducentowDoSelectList();

        Task<List<RodzajSelectItemDto>> PobierzRodzajeDoSelectList();
    }
}
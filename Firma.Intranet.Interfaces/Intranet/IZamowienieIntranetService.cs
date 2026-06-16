using Firma.Data.Data.Sklep;
using Firma.Intranet.Services.Data.Intranet;

namespace Firma.Intranet.Interfaces.Intranet
{
    public interface IZamowienieIntranetService
    {
        Task<List<Zamowienie>> PobierzListe();

        Task<Zamowienie?> PobierzSzczegoly(int id);

        Task<Zamowienie?> PobierzDoEdycji(int id);

        Task Dodaj(Zamowienie zamowienie);

        Task<bool> Aktualizuj(int id, Zamowienie zamowienie);

        Task<bool> ZmienStatus(int id, string status);

        Task<Zamowienie?> PobierzDoUsuniecia(int id);

        Task Usun(int id);

        Task<Zamowienie?> PobierzDoDokumentow(int id);

        Task<List<Zamowienie>> PobierzWszystkieDoDokumentow();

        Task<List<Zamowienie>> PobierzZaznaczoneDoDokumentow(int[] ids);

        Task<List<KlientSelectItemDto>> PobierzKlientowDoSelectList();
    }
}
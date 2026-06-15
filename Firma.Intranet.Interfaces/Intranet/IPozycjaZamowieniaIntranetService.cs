using Firma.Data.Data.Sklep;
using Firma.Intranet.Services.Data.Intranet;

namespace Firma.Intranet.Interfaces.Intranet
{
    public interface IPozycjaZamowieniaIntranetService
    {
        Task<List<PozycjaZamowienia>> PobierzListe();

        Task<PozycjaZamowienia?> PobierzSzczegoly(int id);

        Task<PozycjaZamowienia?> PobierzDoEdycji(int id);

        Task Dodaj(PozycjaZamowienia pozycjaZamowienia);

        Task<bool> Aktualizuj(int id, PozycjaZamowienia pozycjaZamowienia);

        Task<PozycjaZamowienia?> PobierzDoUsuniecia(int id);

        Task Usun(int id);

        Task<List<ZamowienieSelectItemDto>> PobierzZamowieniaDoSelectList();

        Task<List<TowarSelectItemDto>> PobierzTowaryDoSelectList();
    }
}
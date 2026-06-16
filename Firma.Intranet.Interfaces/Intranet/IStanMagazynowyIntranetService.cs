using Firma.Data.Data.Sklep;
using Firma.Intranet.Services.Data.Intranet;

namespace Firma.Intranet.Interfaces.Intranet
{
    public interface IStanMagazynowyIntranetService
    {
        Task<List<StanMagazynowy>> PobierzListe(bool tylkoNiskie = false);

        Task<StanMagazynowy?> PobierzSzczegoly(int id);

        Task<StanMagazynowy?> PobierzDoEdycji(int id);

        Task Dodaj(StanMagazynowy stanMagazynowy);

        Task<bool> Aktualizuj(int id, StanMagazynowy stanMagazynowy);

        Task<StanMagazynowy?> PobierzDoUsuniecia(int id);

        Task Usun(int id);

        Task Aktywuj(int id);

        Task Dezaktywuj(int id);

        Task<bool> CzyTowarMaStanMagazynowy(int idTowaru, int? idStanuDoPominiecia = null);

        Task<List<TowarSelectItemDto>> PobierzTowaryDoSelectList(int? idAktualnegoTowaru = null);

        Task<int> PoliczWszystkieStany();

        Task<int> PoliczAktywneStany();

        Task<int> PoliczNiskieStany();
    }
}
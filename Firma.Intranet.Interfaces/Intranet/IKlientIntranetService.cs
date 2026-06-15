using Firma.Data.Data.Sklep;

namespace Firma.Intranet.Interfaces.Intranet
{
    public interface IKlientIntranetService
    {
        Task<List<Klient>> PobierzListe();

        Task<Klient?> PobierzSzczegoly(int id);

        Task<Klient?> PobierzDoEdycji(int id);

        Task<bool> CzyEmailIstnieje(string email, int? idKlientaDoPominiecia = null);

        Task Dodaj(Klient klient);

        Task<bool> Aktualizuj(int id, Klient klient);

        Task<Klient?> PobierzDoUsuniecia(int id);

        Task<bool> Usun(int id);
    }
}
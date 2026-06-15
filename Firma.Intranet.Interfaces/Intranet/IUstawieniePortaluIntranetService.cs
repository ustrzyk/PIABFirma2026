using Firma.Data.Data.CMS;

namespace Firma.Intranet.Interfaces.Intranet
{
    public interface IUstawieniePortaluIntranetService
    {
        Task<List<UstawieniePortalu>> PobierzListe();

        Task<UstawieniePortalu?> PobierzSzczegoly(int id);

        Task<UstawieniePortalu?> PobierzDoEdycji(int id);

        Task<bool> CzyKluczIstnieje(string klucz, int? idUstawieniaDoPominiecia = null);

        Task Dodaj(UstawieniePortalu ustawieniePortalu);

        Task<bool> Aktualizuj(int id, UstawieniePortalu ustawieniePortalu);

        Task<UstawieniePortalu?> PobierzDoUsuniecia(int id);

        Task Usun(int id);

        Task Aktywuj(int id);

        Task Dezaktywuj(int id);
    }
}
using Firma.Data.Data.Sklep;
using Firma.Intranet.Services.Data.Intranet;

namespace Firma.Intranet.Interfaces.Intranet
{
    public interface IZalacznikTowaruIntranetService
    {
        Task<List<ZalacznikTowaru>> PobierzListe();

        Task<ZalacznikTowaru?> PobierzSzczegoly(int id);

        Task<ZalacznikTowaru?> PobierzDoEdycji(int id);

        Task<ZalacznikTowaru?> PobierzDoUsuniecia(int id);

        Task<ZalacznikTowaru?> PobierzDoPobrania(int id);

        Task Dodaj(int idTowaru, string opis, PlikZalacznikaDto plik, string folderUploadu);

        Task<bool> Aktualizuj(int id, int idTowaru, string opis, PlikZalacznikaDto? plik, string folderUploadu);

        Task Usun(int id, string folderUploadu);

        Task UsunZaznaczone(int[] ids, string folderUploadu);

        Task<List<TowarSelectItemDto>> PobierzTowaryDoSelectList();

        string PobierzSciezkeFizyczna(string folderUploadu, string sciezka);
    }
}
using Firma.Services.Data.Dto.ZamowieniaPubliczne;

namespace Firma.Interfaces.Sklep
{
    public interface IZamowieniePubliczneService
    {
        Task<ZamowieniePubliczneWynikDto> ZlozZamowienie(DaneZamowieniaPublicznegoDto dane);
    }
}
using Firma.Services.Data.Dto.ZamowieniaPubliczne;

namespace Firma.Interfaces.Sklep
{
    public interface IStatusZamowieniaService
    {
        Task<StatusZamowieniaDto?> SprawdzStatus(string numerZamowienia, string email);
    }
}
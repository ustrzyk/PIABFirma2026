using System.ComponentModel.DataAnnotations;
using Firma.Services.Data.Dto.ZamowieniaPubliczne;

namespace Firma.PortalWWW.Models
{
    public class StatusZamowieniaViewModel
    {
        [Required(ErrorMessage = "Numer zamówienia jest wymagany")]
        [Display(Name = "Numer zamówienia")]
        public string NumerZamowienia { get; set; } = string.Empty;

        [Required(ErrorMessage = "E-mail jest wymagany")]
        [EmailAddress(ErrorMessage = "Podaj poprawny adres e-mail")]
        [Display(Name = "E-mail użyty w zamówieniu")]
        public string Email { get; set; } = string.Empty;

        public StatusZamowieniaDto? Wynik { get; set; }
    }
}
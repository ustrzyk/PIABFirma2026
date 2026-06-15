using System.ComponentModel.DataAnnotations;

namespace Firma.Intranet.Models
{
    public class LogowanieViewModel
    {
        [Required(ErrorMessage = "Podaj adres e-mail")]
        [EmailAddress(ErrorMessage = "Podaj poprawny adres e-mail")]
        [Display(Name = "E-mail")]
        public string Email { get; set; } = string.Empty;

        [Required(ErrorMessage = "Podaj hasło")]
        [DataType(DataType.Password)]
        [Display(Name = "Hasło")]
        public string Haslo { get; set; } = string.Empty;

        [Display(Name = "Zapamiętaj mnie")]
        public bool ZapamietajMnie { get; set; }

        public string? ReturnUrl { get; set; }
    }
}
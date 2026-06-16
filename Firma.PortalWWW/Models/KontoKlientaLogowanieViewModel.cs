using System.ComponentModel.DataAnnotations;

namespace Firma.PortalWWW.Models
{
    public class KontoKlientaLogowanieViewModel
    {
        [Required(ErrorMessage = "E-mail jest wymagany")]
        [EmailAddress(ErrorMessage = "Podaj poprawny adres e-mail")]
        [Display(Name = "E-mail")]
        public string Email { get; set; } = string.Empty;

        [Required(ErrorMessage = "Hasło jest wymagane")]
        [DataType(DataType.Password)]
        [Display(Name = "Hasło")]
        public string Haslo { get; set; } = string.Empty;

        [Display(Name = "Zapamiętaj mnie")]
        public bool ZapamietajMnie { get; set; }

        public string? ReturnUrl { get; set; }
    }
}
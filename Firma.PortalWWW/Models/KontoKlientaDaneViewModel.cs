using System.ComponentModel.DataAnnotations;

namespace Firma.PortalWWW.Models
{
    public class KontoKlientaDaneViewModel
    {
        [Required(ErrorMessage = "Imię jest wymagane")]
        [MaxLength(20, ErrorMessage = "Imię może zawierać maksymalnie 20 znaków")]
        [Display(Name = "Imię")]
        public string Imie { get; set; } = string.Empty;

        [Required(ErrorMessage = "Nazwisko jest wymagane")]
        [MaxLength(30, ErrorMessage = "Nazwisko może zawierać maksymalnie 30 znaków")]
        [Display(Name = "Nazwisko")]
        public string Nazwisko { get; set; } = string.Empty;

        [Required(ErrorMessage = "E-mail jest wymagany")]
        [EmailAddress(ErrorMessage = "Podaj poprawny adres e-mail")]
        [Display(Name = "E-mail")]
        public string Email { get; set; } = string.Empty;

        [MaxLength(15, ErrorMessage = "Telefon może zawierać maksymalnie 15 znaków")]
        [Display(Name = "Telefon")]
        public string Telefon { get; set; } = string.Empty;

        [MaxLength(80, ErrorMessage = "Ulica może zawierać maksymalnie 80 znaków")]
        [Display(Name = "Ulica")]
        public string Ulica { get; set; } = string.Empty;

        [MaxLength(10, ErrorMessage = "Numer domu może zawierać maksymalnie 10 znaków")]
        [Display(Name = "Numer domu")]
        public string NumerDomu { get; set; } = string.Empty;

        [MaxLength(10, ErrorMessage = "Numer lokalu może zawierać maksymalnie 10 znaków")]
        [Display(Name = "Numer lokalu")]
        public string NumerLokalu { get; set; } = string.Empty;

        [MaxLength(10, ErrorMessage = "Kod pocztowy może zawierać maksymalnie 10 znaków")]
        [Display(Name = "Kod pocztowy")]
        public string KodPocztowy { get; set; } = string.Empty;

        [MaxLength(40, ErrorMessage = "Miasto może zawierać maksymalnie 40 znaków")]
        [Display(Name = "Miasto")]
        public string Miasto { get; set; } = string.Empty;
    }
}
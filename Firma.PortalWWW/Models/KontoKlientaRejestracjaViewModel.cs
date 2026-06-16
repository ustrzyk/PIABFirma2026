using System.ComponentModel.DataAnnotations;

namespace Firma.PortalWWW.Models
{
    public class KontoKlientaRejestracjaViewModel
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
        [MaxLength(256, ErrorMessage = "E-mail może zawierać maksymalnie 256 znaków")]
        [Display(Name = "E-mail")]
        public string Email { get; set; } = string.Empty;

        [MaxLength(15, ErrorMessage = "Telefon może zawierać maksymalnie 15 znaków")]
        [Display(Name = "Telefon")]
        public string Telefon { get; set; } = string.Empty;

        [Required(ErrorMessage = "Hasło jest wymagane")]
        [DataType(DataType.Password)]
        [Display(Name = "Hasło")]
        public string Haslo { get; set; } = string.Empty;

        [Required(ErrorMessage = "Powtórz hasło")]
        [DataType(DataType.Password)]
        [Compare(nameof(Haslo), ErrorMessage = "Hasła muszą być takie same")]
        [Display(Name = "Powtórz hasło")]
        public string PowtorzHaslo { get; set; } = string.Empty;
    }
}
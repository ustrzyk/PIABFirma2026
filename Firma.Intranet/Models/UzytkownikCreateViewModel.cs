using System.ComponentModel.DataAnnotations;

namespace Firma.Intranet.Models
{
    public class UzytkownikCreateViewModel
    {
        [Required(ErrorMessage = "Podaj e-mail")]
        [EmailAddress(ErrorMessage = "Podaj poprawny e-mail")]
        [Display(Name = "E-mail")]
        public string Email { get; set; } = string.Empty;

        [Required(ErrorMessage = "Podaj hasło")]
        [MinLength(8, ErrorMessage = "Hasło musi mieć minimum 8 znaków")]
        [DataType(DataType.Password)]
        [Display(Name = "Hasło")]
        public string Haslo { get; set; } = string.Empty;

        [Required(ErrorMessage = "Wybierz rolę")]
        [Display(Name = "Rola")]
        public string Rola { get; set; } = "Pracownik";

        public List<string> DostepneRole { get; set; } = new List<string>();
    }
}
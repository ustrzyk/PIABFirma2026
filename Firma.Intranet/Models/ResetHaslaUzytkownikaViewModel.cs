using System.ComponentModel.DataAnnotations;

namespace Firma.Intranet.Models
{
    public class ResetHaslaUzytkownikaViewModel
    {
        public string Id { get; set; } = string.Empty;

        public string Email { get; set; } = string.Empty;

        [Required(ErrorMessage = "Podaj nowe hasło")]
        [MinLength(8, ErrorMessage = "Hasło musi mieć minimum 8 znaków")]
        [DataType(DataType.Password)]
        [Display(Name = "Nowe hasło")]
        public string NoweHaslo { get; set; } = string.Empty;
    }
}
using System.ComponentModel.DataAnnotations;

namespace Firma.Intranet.Models
{
    public class UzytkownikEditViewModel
    {
        public string Id { get; set; } = string.Empty;

        [Required(ErrorMessage = "Podaj e-mail")]
        [EmailAddress(ErrorMessage = "Podaj poprawny e-mail")]
        [Display(Name = "E-mail")]
        public string Email { get; set; } = string.Empty;

        [Required(ErrorMessage = "Wybierz rolę")]
        [Display(Name = "Rola")]
        public string Rola { get; set; } = string.Empty;

        public bool CzyAktualnieZalogowany { get; set; }

        public List<string> DostepneRole { get; set; } = new List<string>();
    }
}
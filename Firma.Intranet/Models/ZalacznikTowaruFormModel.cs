using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace Firma.Intranet.Models
{
    public class ZalacznikTowaruFormModel
    {
        public int IdZalacznikaTowaru { get; set; }

        [Required(ErrorMessage = "Wybierz towar")]
        [Display(Name = "Towar")]
        public int IdTowaru { get; set; }

        [Display(Name = "Plik")]
        public IFormFile? Plik { get; set; }

        [MaxLength(300, ErrorMessage = "Opis może zawierać maksymalnie 300 znaków")]
        [Display(Name = "Opis")]
        public string Opis { get; set; } = string.Empty;
    }
}
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Firma.Data.Data.CMS
{
    public class Strona
    {
        [Key]
        public int IdStrony { get; set; }

        [Required(ErrorMessage = "Tytuł odnośnika jest wymagany")]
        [MaxLength(20, ErrorMessage = "Tytuł odnośnika może zawierać maksymalnie 20 znaków")]
        [Display(Name = "Tytuł odnośnika")]
        public required string LinkTytul { get; set; }

        [Required(ErrorMessage = "Tytuł strony jest wymagany")]
        [MaxLength(80, ErrorMessage = "Tytuł strony może zawierać maksymalnie 80 znaków")]
        [Display(Name = "Tytuł strony")]
        public required string Tytul { get; set; }

        [Required(ErrorMessage = "Treść strony jest wymagana")]
        [Column(TypeName = "nvarchar(MAX)")]
        [Display(Name = "Treść")]
        public required string Tresc { get; set; }

        [Range(1, 20, ErrorMessage = "Pozycja musi być liczbą od 1 do 20")]
        [Display(Name = "Pozycja wyświetlania")]
        public int Pozycja { get; set; }

        [Display(Name = "Czy aktywny")]
        public bool CzyAktywny { get; set; } = true;
    }
}

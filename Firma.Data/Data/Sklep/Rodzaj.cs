using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Firma.Data.Data.Sklep
{
    public class Rodzaj
    {
        [Key]
        public int IdRodzaju { get; set; }

        [Required(ErrorMessage = "Nazwa rodzaju jest wymagana")]
        [MaxLength(25, ErrorMessage = "Nazwa rodzaju może zawierać maksymalnie 25 znaków")]
        [Display(Name = "Nazwa rodzaju")]
        public required string Nazwa { get; set; }

        [MaxLength(120, ErrorMessage = "Opis rodzaju może zawierać maksymalnie 120 znaków")]
        [Display(Name = "Opis rodzaju")]
        public string Opis { get; set; } = string.Empty;

        [Display(Name = "Czy aktywny")]
        public bool CzyAktywny { get; set; } = true;


        // Powiązanie 1:N - jeden rodzaj może mieć wiele towarów
        public ICollection<Towar> Towar { get; } = new List<Towar>();
    }
}

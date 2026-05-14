using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Firma.Data.Data.Sklep
{
    public class Producent
    {
        [Key]
        public int IdProducenta { get; set; }

        [Required(ErrorMessage = "Nazwa producenta jest wymagana")]
        [MaxLength(40, ErrorMessage = "Nazwa producenta może zawierać maksymalnie 40 znaków")]
        [Display(Name = "Nazwa producenta")]
        public required string Nazwa { get; set; }

        [MaxLength(30, ErrorMessage = "Kraj może zawierać maksymalnie 30 znaków")]
        [Display(Name = "Kraj")]
        public string Kraj { get; set; } = string.Empty;

        [Url(ErrorMessage = "Podaj poprawny adres strony WWW")]
        [Display(Name = "Strona WWW")]
        public string StronaWWW { get; set; } = string.Empty;

        [MaxLength(200, ErrorMessage = "Opis może zawierać maksymalnie 200 znaków")]
        [Display(Name = "Opis")]
        public string Opis { get; set; } = string.Empty;

        [Display(Name = "Czy aktywny")]
        public bool CzyAktywny { get; set; } = true;


        // Powiązanie 1:N - jeden producent może mieć wiele towarów
        public ICollection<Towar> Towar { get; } = new List<Towar>();
    }
}

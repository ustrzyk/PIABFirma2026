using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Firma.Data.Data.Sklep
{
    public class Zamowienie
    {
        [Key]
        public int IdZamowienia { get; set; }

        [Required(ErrorMessage = "Numer zamówienia jest wymagany")]
        [MaxLength(20, ErrorMessage = "Numer zamówienia może zawierać maksymalnie 20 znaków")]
        [Display(Name = "Numer zamówienia")]
        public required string NumerZamowienia { get; set; }

        [Required(ErrorMessage = "Data zamówienia jest wymagana")]
        [DataType(DataType.Date)]
        [Display(Name = "Data zamówienia")]
        public DateTime? DataZamowienia { get; set; }

        [Required(ErrorMessage = "Status zamówienia jest wymagany")]
        [MaxLength(20, ErrorMessage = "Status może zawierać maksymalnie 20 znaków")]
        [Display(Name = "Status")]
        public required string Status { get; set; }

        [Required(ErrorMessage = "Wartość zamówienia jest wymagana")]
        [Column(TypeName = "money")]
        [Display(Name = "Wartość razem")]
        [DisplayFormat(DataFormatString = "{0:N2}", ApplyFormatInEditMode = true)]
        public decimal WartoscRazem { get; set; }

        [Required(ErrorMessage = "Ulica jest wymagana")]
        [MaxLength(40, ErrorMessage = "Ulica może zawierać maksymalnie 40 znaków")]
        [Display(Name = "Ulica")]
        public required string Ulica { get; set; }

        [Required(ErrorMessage = "Numer domu jest wymagany")]
        [MaxLength(10, ErrorMessage = "Numer domu może zawierać maksymalnie 10 znaków")]
        [Display(Name = "Numer domu")]
        public required string NumerDomu { get; set; }

        [MaxLength(10, ErrorMessage = "Numer lokalu może zawierać maksymalnie 10 znaków")]
        [Display(Name = "Numer lokalu")]
        public string NumerLokalu { get; set; } = string.Empty;

        [Required(ErrorMessage = "Kod pocztowy jest wymagany")]
        [MaxLength(10, ErrorMessage = "Kod pocztowy może zawierać maksymalnie 10 znaków")]
        [Display(Name = "Kod pocztowy")]
        public required string KodPocztowy { get; set; }

        [Required(ErrorMessage = "Miasto jest wymagane")]
        [MaxLength(30, ErrorMessage = "Miasto może zawierać maksymalnie 30 znaków")]
        [Display(Name = "Miasto")]
        public required string Miasto { get; set; }


        // Klucz obcy i powiązania

        // Powiązanie N:1 - wiele zamówień może należeć do jednego klienta
        [ForeignKey("Klient")]
        [Display(Name = "Klient")]
        public int IdKlienta { get; set; }
        public Klient? Klient { get; set; }

        // Powiązanie 1:N - jedno zamówienie może mieć wiele pozycji zamówienia
        public ICollection<PozycjaZamowienia> PozycjaZamowienia { get; } = new List<PozycjaZamowienia>();
    }
}

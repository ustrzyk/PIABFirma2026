using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Firma.Data.Data.Sklep
{
    public class Klient
    {
        [Key]
        public int IdKlienta { get; set; }

        [Required(ErrorMessage = "Imię jest wymagane")]
        [MaxLength(20, ErrorMessage = "Imię może zawierać maksymalnie 20 znaków")]
        [Display(Name = "Imię")]
        public required string Imie { get; set; }

        [Required(ErrorMessage = "Nazwisko jest wymagane")]
        [MaxLength(30, ErrorMessage = "Nazwisko może zawierać maksymalnie 30 znaków")]
        [Display(Name = "Nazwisko")]
        public required string Nazwisko { get; set; }

        [Required(ErrorMessage = "Adres e-mail jest wymagany")]
        [EmailAddress(ErrorMessage = "Podaj poprawny adres e-mail")]
        [Display(Name = "E-mail")]
        public required string Email { get; set; }

        [MaxLength(15, ErrorMessage = "Telefon może zawierać maksymalnie 15 znaków")]
        [Display(Name = "Telefon")]
        public string Telefon { get; set; } = string.Empty;


        // Powiązanie 1:N - jeden klient może mieć wiele zamówień
        public ICollection<Zamowienie> Zamowienie { get; } = new List<Zamowienie>();
    }
}

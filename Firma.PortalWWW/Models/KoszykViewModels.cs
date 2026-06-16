using System.ComponentModel.DataAnnotations;

namespace Firma.PortalWWW.Models
{
    public class KoszykViewModel
    {
        public IList<KoszykPozycjaViewModel> Pozycje { get; set; } =
            new List<KoszykPozycjaViewModel>();

        public decimal Suma
        {
            get
            {
                return Pozycje.Sum(p => p.Wartosc);
            }
        }

        public int LiczbaSztuk
        {
            get
            {
                return Pozycje.Sum(p => p.Ilosc);
            }
        }
    }

    public class KoszykPozycjaViewModel
    {
        public int IdTowaru { get; set; }

        public string Kod { get; set; } = string.Empty;

        public string Nazwa { get; set; } = string.Empty;

        public string FotoUrl { get; set; } = string.Empty;

        public string Producent { get; set; } = string.Empty;

        public string Rodzaj { get; set; } = string.Empty;

        public decimal Cena { get; set; }

        public int Ilosc { get; set; }

        public int? DostepnaIlosc { get; set; }

        public decimal Wartosc
        {
            get
            {
                return Cena * Ilosc;
            }
        }
    }

    public class DaneZamowieniaViewModel
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

        [Required(ErrorMessage = "Ulica jest wymagana")]
        [MaxLength(40, ErrorMessage = "Ulica może zawierać maksymalnie 40 znaków")]
        [Display(Name = "Ulica")]
        public string Ulica { get; set; } = string.Empty;

        [Required(ErrorMessage = "Numer domu jest wymagany")]
        [MaxLength(10, ErrorMessage = "Numer domu może zawierać maksymalnie 10 znaków")]
        [Display(Name = "Numer domu")]
        public string NumerDomu { get; set; } = string.Empty;

        [MaxLength(10, ErrorMessage = "Numer lokalu może zawierać maksymalnie 10 znaków")]
        [Display(Name = "Numer lokalu")]
        public string NumerLokalu { get; set; } = string.Empty;

        [Required(ErrorMessage = "Kod pocztowy jest wymagany")]
        [MaxLength(10, ErrorMessage = "Kod pocztowy może zawierać maksymalnie 10 znaków")]
        [Display(Name = "Kod pocztowy")]
        public string KodPocztowy { get; set; } = string.Empty;

        [Required(ErrorMessage = "Miasto jest wymagane")]
        [MaxLength(30, ErrorMessage = "Miasto może zawierać maksymalnie 30 znaków")]
        [Display(Name = "Miasto")]
        public string Miasto { get; set; } = string.Empty;
    }
}
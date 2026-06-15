using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Firma.Data.Data.Sklep
{
    public class Towar
    {
        [Key]
        public int IdTowaru { get; set; }

        [Required(ErrorMessage = "Kod towaru jest wymagany")]
        [MaxLength(20, ErrorMessage = "Kod towaru może zawierać maksymalnie 20 znaków")]
        [Display(Name = "Kod towaru")]
        public required string Kod { get; set; }

        [Required(ErrorMessage = "Nazwa towaru jest wymagana")]
        [MaxLength(60, ErrorMessage = "Nazwa towaru może zawierać maksymalnie 60 znaków")]
        [Display(Name = "Nazwa towaru")]
        public required string Nazwa { get; set; }

        [Required(ErrorMessage = "Cena towaru jest wymagana")]
        [Column(TypeName = "money")]
        [Display(Name = "Cena")]
        [DisplayFormat(DataFormatString = "{0:N2}", ApplyFormatInEditMode = true)]
        public decimal Cena { get; set; }

        [Required(ErrorMessage = "Adres zdjęcia jest wymagany")]
        [Url(ErrorMessage = "Podaj poprawny adres URL zdjęcia")]
        [Display(Name = "Adres zdjęcia")]
        public required string FotoUrl { get; set; }

        [MaxLength(500, ErrorMessage = "Opis może zawierać maksymalnie 500 znaków")]
        [Display(Name = "Opis")]
        public string Opis { get; set; } = string.Empty;

        [Display(Name = "Czy aktywny")]
        public bool CzyAktywny { get; set; } = true;

        // Klucze obce i powiązania

        // Powiązanie N:1 - wiele towarów należy do jednego rodzaju
        [ForeignKey("Rodzaj")]
        [Display(Name = "Rodzaj")]
        public int IdRodzaju { get; set; }
        public Rodzaj? Rodzaj { get; set; }

        // Powiązanie N:1 - wiele towarów może należeć do jednego producenta
        [ForeignKey("Producent")]
        [Display(Name = "Producent")]
        public int IdProducenta { get; set; }
        public Producent? Producent { get; set; }

        // Powiązanie 1:1 - jeden towar ma jeden stan magazynowy
        public StanMagazynowy? StanMagazynowy { get; set; }

        // Powiązanie 1:N - jeden towar może występować w wielu pozycjach zamówienia
        public ICollection<PozycjaZamowienia> PozycjaZamowienia { get; } = new List<PozycjaZamowienia>();

        // Powiązanie 1:N - jeden towar może mieć wiele załączników
        public ICollection<ZalacznikTowaru> ZalacznikiTowaru { get; } = new List<ZalacznikTowaru>();
    }
}
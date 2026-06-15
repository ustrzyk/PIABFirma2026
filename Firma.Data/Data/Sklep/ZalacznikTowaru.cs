using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Firma.Data.Data.Sklep
{
    public class ZalacznikTowaru
    {
        [Key]
        public int IdZalacznikaTowaru { get; set; }

        [Required(ErrorMessage = "Nazwa pliku jest wymagana")]
        [MaxLength(150, ErrorMessage = "Nazwa pliku może zawierać maksymalnie 150 znaków")]
        [Display(Name = "Nazwa pliku")]
        public required string NazwaPliku { get; set; }

        [Required(ErrorMessage = "Oryginalna nazwa pliku jest wymagana")]
        [MaxLength(150, ErrorMessage = "Oryginalna nazwa pliku może zawierać maksymalnie 150 znaków")]
        [Display(Name = "Oryginalna nazwa")]
        public required string NazwaOryginalna { get; set; }

        [Required(ErrorMessage = "Ścieżka pliku jest wymagana")]
        [MaxLength(300, ErrorMessage = "Ścieżka może zawierać maksymalnie 300 znaków")]
        [Display(Name = "Ścieżka")]
        public required string Sciezka { get; set; }

        [Required(ErrorMessage = "Typ pliku jest wymagany")]
        [MaxLength(100, ErrorMessage = "Typ pliku może zawierać maksymalnie 100 znaków")]
        [Display(Name = "Typ pliku")]
        public required string TypPliku { get; set; }

        [Display(Name = "Rozmiar pliku")]
        public long Rozmiar { get; set; }

        [MaxLength(300, ErrorMessage = "Opis może zawierać maksymalnie 300 znaków")]
        [Display(Name = "Opis")]
        public string Opis { get; set; } = string.Empty;

        [Display(Name = "Data dodania")]
        public DateTime DataDodania { get; set; } = DateTime.Now;

        [Display(Name = "Czy aktywny")]
        public bool CzyAktywny { get; set; } = true;

        [ForeignKey("Towar")]
        [Display(Name = "Towar")]
        public int IdTowaru { get; set; }

        public Towar? Towar { get; set; }
    }
}
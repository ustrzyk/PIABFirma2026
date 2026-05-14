using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Firma.Data.Data.Sklep
{
    public class StanMagazynowy
    {
        [Key]
        public int IdStanuMagazynowego { get; set; }

        [Range(0, 200, ErrorMessage = "Ilość sztuk musi być liczbą od 0 do 200")]
        [Display(Name = "Ilość sztuk")]
        public int IloscSztuk { get; set; }

        [Range(0, 20, ErrorMessage = "Minimalna ilość musi być liczbą od 0 do 20")]
        [Display(Name = "Minimalna ilość")]
        public int MinimalnaIlosc { get; set; }

        [MaxLength(30, ErrorMessage = "Lokalizacja może zawierać maksymalnie 30 znaków")]
        [Display(Name = "Lokalizacja w magazynie")]
        public string Lokalizacja { get; set; } = string.Empty;

        [Display(Name = "Czy aktywny")]
        public bool CzyAktywny { get; set; } = true;


        // Klucz obcy i powiązanie

        // Powiązanie 1:1 - jeden stan magazynowy dotyczy jednego towaru
        [ForeignKey("Towar")]
        [Display(Name = "Towar")]
        public int IdTowaru { get; set; }
        public Towar? Towar { get; set; }
    }
}

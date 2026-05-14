using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Firma.Data.Data.Sklep
{
    public class PozycjaZamowienia
    {
        [Key]
        public int IdPozycjiZamowienia { get; set; }

        [Range(1, 20, ErrorMessage = "Ilość musi być liczbą od 1 do 20")]
        [Display(Name = "Ilość")]
        public int Ilosc { get; set; }

        [Required(ErrorMessage = "Cena jednostkowa jest wymagana")]
        [Column(TypeName = "money")]
        [Display(Name = "Cena jednostkowa")]
        [DisplayFormat(DataFormatString = "{0:N2}", ApplyFormatInEditMode = true)]
        public decimal CenaJednostkowa { get; set; }

        
        // Klucze obce i powiązania

        // Powiązanie N:1 - wiele pozycji zamówienia należy do jednego zamówienia
        [ForeignKey("Zamowienie")]
        [Display(Name = "Zamówienie")]
        public int IdZamowienia { get; set; }
        public Zamowienie? Zamowienie { get; set; }

        // Powiązanie N:1 - wiele pozycji zamówienia może wskazywać na jeden towar
        [ForeignKey("Towar")]
        [Display(Name = "Towar")]
        public int IdTowaru { get; set; }
        public Towar? Towar { get; set; }
    }
}

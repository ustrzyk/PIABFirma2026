using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Firma.Data.Data.CMS
{
    public class UstawieniePortalu
    {
        [Key]
        public int IdUstawieniaPortalu { get; set; }

        [Required(ErrorMessage = "Klucz ustawienia jest wymagany")]
        [MaxLength(40, ErrorMessage = "Klucz może zawierać maksymalnie 40 znaków")]
        [Display(Name = "Klucz")]
        public required string Klucz { get; set; }

        [Required(ErrorMessage = "Wartość ustawienia jest wymagana")]
        [Column(TypeName = "nvarchar(MAX)")]
        [Display(Name = "Wartość")]
        public required string Wartosc { get; set; }

        [MaxLength(120, ErrorMessage = "Opis może zawierać maksymalnie 120 znaków")]
        [Display(Name = "Opis")]
        public string Opis { get; set; } = string.Empty;

        [Display(Name = "Czy aktywny")]
        public bool CzyAktywny { get; set; } = true;
    }
}

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Firma.Data.Data.CMS
{
    public class Promocja
    {
        [Key]
        public int IdPromocji { get; set; }

        [Required(ErrorMessage = "Tytuł promocji jest wymagany")]
        [MaxLength(60, ErrorMessage = "Tytuł promocji może zawierać maksymalnie 60 znaków")]
        [Display(Name = "Tytuł promocji")]
        public required string Tytul { get; set; }

        [Column(TypeName = "nvarchar(MAX)")]
        [Display(Name = "Opis promocji")]
        public string Opis { get; set; } = string.Empty;

        [Range(1, 50, ErrorMessage = "Rabat musi mieścić się w przedziale od 1 do 50")]
        [Display(Name = "Rabat procentowy")]
        public int RabatProcentowy { get; set; }

        [Required(ErrorMessage = "Data rozpoczęcia promocji jest wymagana")]
        [DataType(DataType.Date)]
        [Display(Name = "Data od")]
        public DateTime? DataOd { get; set; }

        [Required(ErrorMessage = "Data zakończenia promocji jest wymagana")]
        [DataType(DataType.Date)]
        [Display(Name = "Data do")]
        public DateTime? DataDo { get; set; }

        [Display(Name = "Czy aktywny")]
        public bool CzyAktywny { get; set; } = true;
    }
}

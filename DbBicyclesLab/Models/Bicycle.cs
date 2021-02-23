using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

#nullable disable

namespace DbBicyclesLab.Models
{
    public partial class Bicycle
    {
        public int Id { get; set; }
        [Display(Name = "Опис")]
        [Required(ErrorMessage = ErrorMessages.Required)]
        [StringLength(1500, ErrorMessage = ErrorMessages.StringLength)]
        public string Description { get; set; }
        [Display(Name = "Розмір-Колір-Модель")]
        [Required(ErrorMessage = ErrorMessages.Required)]
        public int? SizeColorModelId { get; set; }

        [Display(Name = "Розмір-Колір-Модель")]
        public virtual SizeColorModel SizeColorModel { get; set; }
    }
}

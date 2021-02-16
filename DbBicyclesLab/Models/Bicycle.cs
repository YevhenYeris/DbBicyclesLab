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
        [Required(ErrorMessage = "Поле не може бути порожнім")]
        public string Description { get; set; }
        public int? SizeColorModelId { get; set; }

        [Display(Name = "Розмір-Колір-Модель")]
        [Required(ErrorMessage = "Поле не може бути порожнім")]
        public virtual SizeColorModel SizeColorModel { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

#nullable disable

namespace DbBicyclesLab.Models
{
    public partial class SizeColorModel
    {
        public SizeColorModel()
        {
            Bicycles = new HashSet<Bicycle>();
        }

        public int Id { get; set; }
        [Display(Name = "Розмір")]
        [Required(ErrorMessage = ErrorMessages.Required)]
        public int SizeId { get; set; }
        [Display(Name = "Колір")]
        [Required(ErrorMessage = ErrorMessages.Required)]
        public int ColorId { get; set; }
        [Display(Name = "Модель")]
        [Required(ErrorMessage = ErrorMessages.Required)]
        public int ModelId { get; set; }

        [Display(Name = "Колір")]
        public virtual Color Color { get; set; }
        [Display(Name = "Модель")]
        public virtual BicycleModel Model { get; set; }
        [Display(Name = "Розмір")]
        public virtual Size Size { get; set; }
        public virtual ICollection<Bicycle> Bicycles { get; set; }
    }
}

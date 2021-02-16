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
        public int SizeId { get; set; }
        public int ColorId { get; set; }
        public int ModelId { get; set; }

        [Display(Name = "Колір")]
        [Required(ErrorMessage = "Поле не може бути порожнім")]
        public virtual Color Color { get; set; }
        [Display(Name = "Модель")]
        [Required(ErrorMessage = "Поле не може бути порожнім")]
        public virtual BicycleModel Model { get; set; }
        [Display(Name = "Розмір")]
        [Required(ErrorMessage = "Поле не може бути порожнім")]
        public virtual Size Size { get; set; }
        public virtual ICollection<Bicycle> Bicycles { get; set; }
    }
}

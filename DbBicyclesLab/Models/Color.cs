using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

#nullable disable

namespace DbBicyclesLab.Models
{
    public partial class Color
    {
        public Color()
        {
            SizeColorModels = new HashSet<SizeColorModel>();
        }

        public int Id { get; set; }
        [Display(Name = "Назва кольору")]
        [Required(ErrorMessage = "Поле не може бути порожнім")]
        public string ColorName { get; set; }

        public virtual ICollection<SizeColorModel> SizeColorModels { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

#nullable disable

namespace DbBicyclesLab.Models
{
    public partial class Size
    {
        public Size()
        {
            SizeColorModels = new HashSet<SizeColorModel>();
        }

        public int Id { get; set; }
        [Display(Name = "Назва розміру")]
        [Required(ErrorMessage = ErrorMessages.Required)]
        [StringLength(25, ErrorMessage = ErrorMessages.StringLength)]
        public string SizeName { get; set; }

        public virtual ICollection<SizeColorModel> SizeColorModels { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

#nullable disable

namespace DbBicyclesLab.Models
{
    public partial class Category
    {
        public Category()
        {
            BicycleModels = new HashSet<BicycleModel>();
        }

        public int Id { get; set; }
        [Display(Name = "Назва категорії")]
        [Required(ErrorMessage = ErrorMessages.Required)]
        [StringLength(25, ErrorMessage = ErrorMessages.StringLength)]
        public string CategoryName { get; set; }
        [Display(Name = "Опис")]
        [StringLength(1500, ErrorMessage = ErrorMessages.StringLength)]
        public string Description { get; set; }

        public virtual ICollection<BicycleModel> BicycleModels { get; set; }
    }
}

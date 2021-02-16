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
        [Required(ErrorMessage = "Поле не може бути порожнім")]
        public string CategoryName { get; set; }
        [Display(Name = "Опис")]
        public string Description { get; set; }

        public virtual ICollection<BicycleModel> BicycleModels { get; set; }
    }
}

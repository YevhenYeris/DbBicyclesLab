using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

#nullable disable

namespace DbBicyclesLab.Models
{
    public partial class Gender
    {
        public Gender()
        {
            BicycleModels = new HashSet<BicycleModel>();
        }

        public int Id { get; set; }
        [Display(Name = "Для кого")]
        [Required(ErrorMessage = "Поле не може бути порожнім")]
        public string GenderName { get; set; }

        public virtual ICollection<BicycleModel> BicycleModels { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

#nullable disable

namespace DbBicyclesLab.Models
{
    public class ImageModel
    {
        public ImageModel()
        {
            //BicycleModels = new HashSet<BicycleModel>();
            //Brands = new HashSet<Brand>();
        }

        public int Id { get; set; }
        [Display(Name = "Для кого")]
        [Required(ErrorMessage = ErrorMessages.Required)]
        [StringLength(25, ErrorMessage = ErrorMessages.StringLength)]
        public string ImageName { get; set; }

        public string Path { get; set; }

       // public virtual ICollection<BicycleModel> BicycleModels { get; set; }
       // public virtual ICollection<Brand> Brands { get; set; }
    }
}

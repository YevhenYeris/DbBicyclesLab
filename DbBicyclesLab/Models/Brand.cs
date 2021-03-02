using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

#nullable disable

namespace DbBicyclesLab.Models
{
    public partial class Brand
    {
        public Brand()
        {
            BicycleModels = new HashSet<BicycleModel>();
        }

        public int Id { get; set; }
        [Display(Name = "Назва бренду")]
        [Required(ErrorMessage = ErrorMessages.Required)]
        [StringLength(25, ErrorMessage = ErrorMessages.StringLength)]
        public string BrandName { get; set; }
        [Display(Name = "Країна")]
        [Required(ErrorMessage = ErrorMessages.Required)]
        public int CountryId { get; set; }
        [Display(Name = "Представник в Україні")]
        [Required(ErrorMessage = ErrorMessages.Required)]
        public int DealerId { get; set; }
        [Display(Name = "Опис")]
        [StringLength(1500, ErrorMessage = ErrorMessages.StringLength)]
        public string Description { get; set; }
        [Display(Name = "Зображення")]
        public virtual byte[] Image { get; set; }
        [Display(Name = "Країна")]
        public virtual Country Country { get; set; }
        [Display(Name = "Представник в Україні")]
        public virtual AuthorizedDealer Dealer { get; set; }
        public virtual ICollection<BicycleModel> BicycleModels { get; set; }
    }
}

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
        [Required(ErrorMessage = "Поле не може бути порожнім")]
        public string BrandName { get; set; }
        public int CountryId { get; set; }
        public int DealerId { get; set; }
        [Display(Name = "Опис")]
        public string Description { get; set; }

        [Display(Name = "Країна")]
        [Required(ErrorMessage = "Поле не може бути порожнім")]
        public virtual Country Country { get; set; }
        [Display(Name = "Представник в Україні")]
        [Required(ErrorMessage = "Поле не може бути порожнім")]
        public virtual AuthorizedDealer Dealer { get; set; }
        public virtual ICollection<BicycleModel> BicycleModels { get; set; }
    }
}

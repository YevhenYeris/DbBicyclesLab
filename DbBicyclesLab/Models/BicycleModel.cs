using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

#nullable disable

namespace DbBicyclesLab.Models
{
    public partial class BicycleModel
    {
        public BicycleModel()
        {
            SizeColorModels = new HashSet<SizeColorModel>();
        }

        public int Id { get; set; }
        [Display(Name = "Вартість")]
        public int? Price { get; set; }
        [Display(Name = "Назва моделі")]
        [Required(ErrorMessage = ErrorMessages.Required)]
        [StringLength(50, ErrorMessage = ErrorMessages.StringLength)]
        public string ModelName { get; set; }
        [Display(Name = "Рік випуску")]
        [Required(ErrorMessage = ErrorMessages.Required)]
        [Range(0, 9999, ErrorMessage = ErrorMessages.Range)]
        public int ModelYear { get; set; }
        [Display(Name = "Бренд")]
        [Required(ErrorMessage = ErrorMessages.Required)]
        public int BrandId { get; set; }
        [Display(Name = "Для кого")]
        [Required(ErrorMessage = ErrorMessages.Required)]
        public int GenderId { get; set; }
        [Display(Name = "Категорія")]
        [Required(ErrorMessage = ErrorMessages.Required)]
        public int CategoryId { get; set; }
        [Display(Name = "Опис")]
        [StringLength(2500, ErrorMessage = ErrorMessages.StringLength)]
        public string Description { get; set; }
        [Display(Name ="Зображення")]
        public virtual byte[] Image { get; set; }
        [Display(Name = "Бренд")]
        public virtual Brand Brand { get; set; }
        [Display(Name = "Категорія")]
        public virtual Category Category { get; set; }
        [Display(Name = "Для кого")]
        public virtual Gender Gender { get; set; }
        public virtual ICollection<SizeColorModel> SizeColorModels { get; set; }
    }
}

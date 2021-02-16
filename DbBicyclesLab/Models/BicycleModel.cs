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
        [Required(ErrorMessage = "Поле не може бути порожнім")]
        public string ModelName { get; set; }
        [Display(Name = "Рік випуску")]
        [Required(ErrorMessage = "Поле не може бути порожнім")]
        public int ModelYear { get; set; }
        public int BrandId { get; set; }
        public int GenderId { get; set; }
        public int CategoryId { get; set; }
        [Display(Name = "Опис")]
        public string Description { get; set; }

        [Display(Name = "Бренд")]
        [Required(ErrorMessage = "Поле не може бути порожнім")]
        public virtual Brand Brand { get; set; }
        [Display(Name = "Категорія")]
        [Required(ErrorMessage = "Поле не може бути порожнім")]
        public virtual Category Category { get; set; }
        [Display(Name = "Для кого")]
        [Required(ErrorMessage = "Поле не може бути порожнім")]
        public virtual Gender Gender { get; set; }
        public virtual ICollection<SizeColorModel> SizeColorModels { get; set; }
    }
}

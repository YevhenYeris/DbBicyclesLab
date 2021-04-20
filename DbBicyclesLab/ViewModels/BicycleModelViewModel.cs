using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace DbBicyclesLab.Models
{
    public class BicycleModelViewModel
    {
        public int Id { get; set; }
        [Display(Name = "Ціна")]
        public int? Price { get; set; }
        [Display(Name = "Назва")]
        public string ModelName { get; set; }
        [Display(Name = "Рік випуску")]
        public int ModelYear { get; set; }
        public int BrandId { get; set; }
        public int GenderId { get; set; }
        public int CategoryId { get; set; }
        [Display(Name = "Опис")]
        public string Description { get; set; }
        [Display(Name = "Бренд")]
        public virtual Brand Brand { get; set; }
        [Display(Name = "Категорія")]
        public virtual Category Category { get; set; }
        [Display(Name = "Для кого")]
        public virtual Gender Gender { get; set; }
        public virtual ICollection<SizeColorModel> SizeColorModels { get; set; }
        public IFormFile ImageFile { get; set; }
        public virtual byte[] Image { get; set; }
        [Display(Name = "Кількість")]
        public int? Quantity { get; set; }
        [Display(Name = "Доступні кольори")]
        public virtual ICollection<Color> Colors { get; set; }
        [Display(Name = "Доступні розміри")]
        public virtual ICollection<Size> Sizes { get; set; }

    }
}

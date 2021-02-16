using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

#nullable disable

namespace DbBicyclesLab.Models
{
    public partial class AuthorizedDealer
    {
        public AuthorizedDealer()
        {
            Brands = new HashSet<Brand>();
        }

        public int Id { get; set; }
        [Display(Name = "Назва")]
        [Required(ErrorMessage = "Поле не може бути порожнім")]
        public string DealerName { get; set; }
        [Display(Name = "Адреса веб-сайту")]
        [Required(ErrorMessage = "Поле не може бути порожнім")]
        public string WebsiteAddress { get; set; }
        [Display(Name = "Опис")]
        public string Description { get; set; }

        public virtual ICollection<Brand> Brands { get; set; }
    }
}

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

        public string Title { get; } = "Офіційні представники в Україні";
        public int Id { get; set; }
        [Display(Name = "Назва")]
        [Required(ErrorMessage = ErrorMessages.Required)]
        [StringLength(45, ErrorMessage = ErrorMessages.StringLength)]
        public string DealerName { get; set; }
        [Display(Name = "Адреса веб-сайту")]
        [Required(ErrorMessage = ErrorMessages.Required)]
        [StringLength(2048, ErrorMessage = ErrorMessages.StringLength)]
        public string WebsiteAddress { get; set; }
        [Display(Name = "Опис")]
        [StringLength(1500, ErrorMessage = ErrorMessages.StringLength)]
        public string Description { get; set; }

        public virtual ICollection<Brand> Brands { get; set; }
    }
}

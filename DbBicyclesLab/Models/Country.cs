using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

#nullable disable

namespace DbBicyclesLab.Models
{
    public partial class Country
    {
        public Country()
        {
            Brands = new HashSet<Brand>();
        }

        public int Id { get; set; }
        [Display(Name = "Назва країни")]
        [Required(ErrorMessage = ErrorMessages.Required)]
        [StringLength(20, ErrorMessage = ErrorMessages.StringLength)]
        public string CountryName { get; set; }

        public virtual ICollection<Brand> Brands { get; set; }
    }
}

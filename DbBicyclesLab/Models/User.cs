using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;

#nullable disable

namespace DbBicyclesLab.Models
{
    public class User : IdentityUser
    {
        [Display(Name = "Рік народження")]
        public int Year { get; set; }
        [Display(Name = "Ім'я")]
        public string Name { get; set; }
    }
}

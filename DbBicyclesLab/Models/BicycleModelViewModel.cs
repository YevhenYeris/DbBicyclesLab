using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace DbBicyclesLab.Models
{
    public class BicycleModelViewModel
    {
        public int Id { get; set; }
        public int? Price { get; set; }
        public string ModelName { get; set; }
        public int ModelYear { get; set; }
        public int BrandId { get; set; }
        public int GenderId { get; set; }
        public int CategoryId { get; set; }
        public string Description { get; set; }
        public virtual Brand Brand { get; set; }
        public virtual Category Category { get; set; }
        public virtual Gender Gender { get; set; }
        public virtual ICollection<SizeColorModel> SizeColorModels { get; set; }
        public IFormFile Image { get; set; }
    }
}

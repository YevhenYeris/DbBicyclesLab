using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace DbBicyclesLab.Models
{
    public class BrandViewModel
    {
        public int Id { get; set; }
        public string BrandName { get; set; }
        public int CountryId { get; set; }
        public int DealerId { get; set; }
        public string Description { get; set; }
        public virtual Country Country { get; set; }
        public virtual AuthorizedDealer Dealer { get; set; }
        public virtual ICollection<BicycleModel> BicycleModels { get; set; }
        public IFormFile Image { get; set; }
    }
}

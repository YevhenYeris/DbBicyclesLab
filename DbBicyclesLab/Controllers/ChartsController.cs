using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DbBicyclesLab.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace DbBicyclesLab.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ChartsController : ControllerBase
    {
        private readonly DBBicyclesContext _context;

        public ChartsController(DBBicyclesContext context)
        {
            _context = context;
        }

        [HttpGet("JsonData")]
        public JsonResult JsonData()
        {
            var countries = _context.Countries.Include(c => c.Brands).ToList();

            List<object> countryBand = new List<object>();

            countryBand.Add(new[] { "Країна", "Кількість брендів" });

            foreach(var c in countries)
            {
                countryBand.Add(new object[] { c.CountryName, c.Brands.Count() });
            }
            return new JsonResult(countryBand);
        }

        [HttpGet("JsonBikeData")]
        public JsonResult JsonBikeData()
        {
            var countries = _context.Categories.Include(c => c.BicycleModels).ToList();

            List<object> countryBand = new List<object>();

            countryBand.Add(new[] { "Країна", "Кількість брендів" });

            foreach (var c in countries)
            {
                countryBand.Add(new object[] { c.CategoryName, c.BicycleModels.Count() });
            }
            return new JsonResult(countryBand);
        }
    }
}

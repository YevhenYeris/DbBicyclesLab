using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;
using Microsoft.EntityFrameworkCore.Query;
using Microsoft.EntityFrameworkCore;

namespace DbBicyclesLab.Models
{
    public class BicycleModelsListModelView
    {
        public BicycleModelsListModelView(DBBicyclesContext context)
        {
            BicycleModels = context.BicycleModels;
            Categories = context.Categories;
            Brands = context.Brands;
            Genders = context.Genders;
            SelectedCategory = null;
            SelectedBrand = null;
            SelectedGender = null;
        }

        public IEnumerable<BicycleModel> BicycleModels;
        public DbSet<Category> Categories;
        public int? SelectedCategory { get; set; }
        public DbSet<Brand> Brands;
        public int? SelectedBrand { get; set; }

        public DbSet<Gender> Genders;
        public int? SelectedGender { get; set; }

    }
}

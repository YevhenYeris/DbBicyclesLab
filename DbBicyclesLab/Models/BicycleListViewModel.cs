using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DbBicyclesLab.Models
{
    public class BicycleListViewModel
    {
        public BicycleListViewModel(DBBicyclesContext context)
        {
            Bicycles = context.Bicycles;
            Sizes = context.Sizes;
            Colors = context.Colors;
            BicycleModels = context.BicycleModels;
        }

        public IEnumerable<Bicycle> Bicycles;
        public IEnumerable<Size> Sizes;
        public int? SelectedSize {get; set;}
        public IEnumerable<Color> Colors;
        public int? SelectedColor { get; set; }
        public IEnumerable<BicycleModel> BicycleModels;
        public int? SelectedModel { get; set; }
    }
}

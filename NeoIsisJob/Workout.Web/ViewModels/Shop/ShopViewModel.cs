using Workout.Core.Models;
using Workout.Core.Utils.Filters;

namespace Workout.Web.ViewModels.Shop
{
    public class ShopViewModel
    {
        public IEnumerable<ProductModel> Products { get; set; } = [];
        public IEnumerable<CategoryModel> Categories { get; set; } = [];
        public ProductFilter Filter { get; set; } = new ProductFilter(null, null, null, null, null, null);
    }
}

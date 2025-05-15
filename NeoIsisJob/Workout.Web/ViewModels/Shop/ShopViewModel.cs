using Workout.Core.Models;

namespace Workout.Web.ViewModels.Shop
{
    public class ShopViewModel
    {
        public IEnumerable<ProductModel> Products { get; set; }
        public IEnumerable<CategoryModel> Categories { get; set; }
    }
}

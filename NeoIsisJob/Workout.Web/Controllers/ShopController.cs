using Microsoft.AspNetCore.Mvc;
using Workout.Core.IServices;
using Workout.Core.Models;
using Workout.Core.Utils.Filters;
using Workout.Web.ViewModels.Shop;

namespace Workout.Web.Controllers
{
    public class ShopController : Controller
    {
        private readonly IService<ProductModel> productService;
        private readonly IService<CategoryModel> categoryService;

        public ShopController(IService<ProductModel> productService, IService<CategoryModel> categoryService)
        {
            this.productService = productService;
            this.categoryService = categoryService;
        }

        [HttpGet]
        public async Task<IActionResult> Index([FromQuery] ProductFilter filter)
        {
            Console.WriteLine($"Filter: color {filter.Color}, size {filter.Size}, category {filter.CategoryId} ");

            var products = await productService.GetFilteredAsync(filter);
            var categories = await categoryService.GetAllAsync();

            Console.WriteLine($"Products: {string.Join(", ", products.Select(p => p.Name))}");

            if (!products.Any())
            {
                ViewBag.Message = "No products found for the selected filters.";
            }

            var viewModel = new ShopViewModel
            {
                Products = products,
                Categories = categories,
                Filter = filter
            };
            return View(viewModel);
        }
    }
}

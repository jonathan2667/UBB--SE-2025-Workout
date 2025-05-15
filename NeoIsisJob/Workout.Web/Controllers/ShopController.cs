using Microsoft.AspNetCore.Mvc;
using Workout.Core.IServices;
using Workout.Core.Models;
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
        public async Task<IActionResult> Index()
        {
            var products = await productService.GetAllAsync();
            var categories = await categoryService.GetAllAsync(); // Assume you injected this too

            var viewModel = new ShopViewModel
            {
                Products = products,
                Categories = categories
            };

            return View(viewModel);
        }
    }
}

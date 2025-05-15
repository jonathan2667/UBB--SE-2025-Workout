using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
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

        [HttpGet]
        public async Task<IActionResult> Product(int id)
        {
            var product = await productService.GetByIdAsync(id);
            if (product == null)
            {
                return NotFound();
            }

            var category = await categoryService.GetByIdAsync(product.CategoryID);
            var viewModel = new ProductViewModel
            {
                ID = product.ID,
                Name = product.Name,
                Description = product.Description,
                Price = product.Price,
                Color = product.Color,
                Size = product.Size,
                PhotoURL = product.PhotoURL,
                CategoryName = category?.Name ?? "Unknown",
                Stock = product.Stock
            };

            return View(viewModel);
        }

        [HttpGet]
        public async Task<IActionResult> Create()
        {
            var categories = await categoryService.GetAllAsync();
            var viewModel = new CreateProductViewModel
            {
                Categories = categories
            };
            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CreateProductViewModel model)
        {
            if (!ModelState.IsValid)
            {
                model.Categories = await categoryService.GetAllAsync();
                return View(model);
            }

            var product = new ProductModel
            {
                Name = model.Name,
                Description = model.Description,
                Price = model.Price,
                Color = model.Color,
                Size = model.Size,
                PhotoURL = model.PhotoURL,
                CategoryID = model.CategoryID,
                Stock = model.Stock
            };

            await productService.CreateAsync(product);
            return RedirectToAction(nameof(Index));
        }
    }
}

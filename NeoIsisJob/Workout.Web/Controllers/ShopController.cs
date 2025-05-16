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
        private readonly IService<WishlistItemModel> wishlistService;
        private readonly IService<CartItemModel> cartService;
        private const int DefaultUserId = 1; // This should be replaced with actual user ID from authentication

        public ShopController(
            IService<ProductModel> productService,
            IService<CategoryModel> categoryService,
            IService<WishlistItemModel> wishlistService,
            IService<CartItemModel> cartService)
        {
            this.productService = productService;
            this.categoryService = categoryService;
            this.wishlistService = wishlistService;
            this.cartService = cartService;
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
            var wishlistItems = await wishlistService.GetAllAsync();
            var isInWishlist = wishlistItems.Any(w => w.ProductID == id && w.UserID == DefaultUserId);

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
                Stock = product.Stock,
                IsInWishlist = isInWishlist
            };

            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ToggleWishlist(int productId)
        {
            var wishlistItems = await wishlistService.GetAllAsync();
            var existingItem = wishlistItems.FirstOrDefault(w => w.ProductID == productId && w.UserID == DefaultUserId);

            if (existingItem != null)
            {
                await wishlistService.DeleteAsync(existingItem.ID);
            }
            else
            {
                var newWishlistItem = new WishlistItemModel(productId, DefaultUserId);
                await wishlistService.CreateAsync(newWishlistItem);
            }

            return RedirectToAction(nameof(Product), new { id = productId });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddToCart(int productId)
        {
            var product = await productService.GetByIdAsync(productId);
            if (product == null)
            {
                return NotFound();
            }

            if (product.Stock <= 0)
            {
                TempData["ErrorMessage"] = "Sorry, this product is out of stock.";
                return RedirectToAction(nameof(Product), new { id = productId });
            }

            var cartItem = new CartItemModel(DefaultUserId, productId);
            await cartService.CreateAsync(cartItem);

            TempData["SuccessMessage"] = "Product added to cart successfully!";
            return RedirectToAction(nameof(Product), new { id = productId });
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
                Color = string.IsNullOrWhiteSpace(model.Color) ? "N/A" : model.Color,
                Size = string.IsNullOrWhiteSpace(model.Size) ? "N/A" : model.Size,
                PhotoURL = model.PhotoURL,
                CategoryID = model.CategoryID,
                Stock = model.Stock
            };

            await productService.CreateAsync(product);
            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public async Task<IActionResult> Update(int id)
        {
            var product = await productService.GetByIdAsync(id);
            if (product == null)
            {
                return NotFound();
            }

            var categories = await categoryService.GetAllAsync();
            var viewModel = new UpdateProductViewModel
            {
                ID = product.ID,
                Name = product.Name,
                Description = product.Description,
                Price = product.Price,
                Color = product.Color,
                Size = product.Size,
                PhotoURL = product.PhotoURL,
                CategoryID = product.CategoryID,
                Stock = product.Stock,
                Categories = categories
            };

            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Update(UpdateProductViewModel model)
        {
            if (!ModelState.IsValid)
            {
                model.Categories = await categoryService.GetAllAsync();
                return View(model);
            }

            var product = await productService.GetByIdAsync(model.ID);
            if (product == null)
            {
                return NotFound();
            }

            product.Name = model.Name;
            product.Description = model.Description;
            product.Price = model.Price;
            product.Color = string.IsNullOrWhiteSpace(model.Color) ? "N/A" : model.Color;
            product.Size = string.IsNullOrWhiteSpace(model.Size) ? "N/A" : model.Size;
            product.PhotoURL = model.PhotoURL;
            product.CategoryID = model.CategoryID;
            product.Stock = model.Stock;

            await productService.UpdateAsync(product);
            return RedirectToAction(nameof(Product), new { id = product.ID });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            var product = await productService.GetByIdAsync(id);
            if (product == null)
            {
                return NotFound();
            }

            await productService.DeleteAsync(id);
            TempData["SuccessMessage"] = "Product deleted successfully!";
            return RedirectToAction(nameof(Index));
        }
    }
}

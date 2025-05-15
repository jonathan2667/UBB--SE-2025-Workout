using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Workout.Core.Data;
using Workout.Core.IServices;
using Workout.Core.Models;
using Workout.Web.Models;

namespace Workout.Web.Controllers
{
    public class WishlistController : Controller
    {
        private readonly ILogger<WishlistController> _logger;
        private readonly IService<WishlistItemModel> _wishlistService;

        public WishlistController(ILogger<WishlistController> logger, IService<WishlistItemModel> wishlistService)
        {
            _logger = logger;
            _wishlistService = wishlistService;
        }

        public async Task<IActionResult> Index()
        {
            try
            {
                var wishlistItems = await _wishlistService.GetAllAsync();
                
                foreach (var item in wishlistItems)
                {
                    if (item.Product != null)
                    {
                        _logger.LogInformation($"Product {item.Product.ID} - {item.Product.Name} has PhotoURL: {item.Product.PhotoURL ?? "null"}");
                    }
                }
                
                return View(wishlistItems);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving wishlist items");
                return View("Error", new ErrorViewModel { RequestId = "Failed to retrieve wishlist items" });
            }
        }

        [HttpPost]
        public async Task<IActionResult> Remove(int id)
        {
            try
            {
                var result = await _wishlistService.DeleteAsync(id);
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error removing wishlist item");
                return View("Error", new ErrorViewModel { RequestId = "Failed to remove wishlist item" });
            }
        }

        [HttpPost]
        public async Task<IActionResult> Add(int productId)
        {
            try
            {
                var wishlistItem = new WishlistItemModel
                {
                    ProductID = productId,
                    UserID = 1 // TODO: Get the current user ID
                };
                
                var result = await _wishlistService.CreateAsync(wishlistItem);
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error adding wishlist item");
                return View("Error", new ErrorViewModel { RequestId = "Failed to add wishlist item" });
            }
        }
    }
} 
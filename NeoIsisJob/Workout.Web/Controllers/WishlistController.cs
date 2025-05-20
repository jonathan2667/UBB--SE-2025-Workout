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
using Workout.Web.Filters;
using Microsoft.AspNetCore.Http;
using System.Linq;

namespace Workout.Web.Controllers
{
    [AuthorizeUser]
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
                var userId = GetCurrentUserId();
                var allWishlistItems = await _wishlistService.GetAllAsync();
                // Filter the wishlist items for the current user
                var wishlistItems = allWishlistItems.Where(item => item.UserID == userId).ToList();
                
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
                // First check if this item belongs to the current user
                var userId = GetCurrentUserId();
                var wishlistItem = await _wishlistService.GetByIdAsync(id);
                
                if (wishlistItem == null || wishlistItem.UserID != userId)
                {
                    _logger.LogWarning($"Attempt to remove wishlist item {id} that doesn't belong to user {userId}");
                    return RedirectToAction(nameof(Index));
                }
                
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
                var userId = GetCurrentUserId();
                var wishlistItem = new WishlistItemModel
                {
                    ProductID = productId,
                    UserID = userId
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

        private int GetCurrentUserId()
        {
            var userIdString = HttpContext.Session.GetString("UserId");
            if (string.IsNullOrEmpty(userIdString) || !int.TryParse(userIdString, out int userId))
            {
                return 1; // Default to user ID 1 if not logged in
            }
            return userId;
        }
    }
} 
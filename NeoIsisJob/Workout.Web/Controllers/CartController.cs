using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Workout.Core.IServices;
using Workout.Core.Models;
using Workout.Core.Services;
using Workout.Web.Models;

namespace Workout.Web.Controllers
{
    public class CartController : Controller
    {
        private readonly ILogger<CartController> _logger;
        private readonly IService<CartItemModel> _cartService;

        public CartController(ILogger<CartController> logger, IService<CartItemModel> cartService)
        {
            _logger = logger;
            _cartService = cartService;
        }

        public async Task<IActionResult> Index()
        {
            try
            {
                var cartItems = await _cartService.GetAllAsync();
                return View(cartItems);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving cart items");
                return View("Error", new ErrorViewModel { RequestId = "Failed to retrieve cart items" });
            }
        }

        [HttpPost]
        public async Task<IActionResult> Remove(int id)
        {
            try
            {
                var result = await _cartService.DeleteAsync(id);
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error removing cart item");
                return View("Error", new ErrorViewModel { RequestId = "Failed to remove cart item" });
            }
        }

        [HttpPost]
        public async Task<IActionResult> Add(int productId)
        {
            try
            {
                var cartItem = new CartItemModel
                {
                    ProductID = productId,
                    UserID = 1 // TODO: Get the current user ID
                };
                
                var result = await _cartService.CreateAsync(cartItem);
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error adding cart item");
                return View("Error", new ErrorViewModel { RequestId = "Failed to add cart item" });
            }
        }

        [HttpPost]
        public async Task<IActionResult> Clear()
        {
            try
            {
                await ((CartService)_cartService).ResetCart();
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error clearing cart");
                return View("Error", new ErrorViewModel { RequestId = "Failed to clear cart" });
            }
        }

        public async Task<IActionResult> Payment()
        {
            try
            {
                var cartItems = await _cartService.GetAllAsync();
                decimal totalAmount = 0;
                
                foreach (var item in cartItems)
                {
                    if (item.Product != null)
                    {
                        totalAmount += item.Product.Price;
                    }
                }
                
                ViewData["TotalAmount"] = totalAmount;
                return View();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving cart items for payment");
                return View("Error", new ErrorViewModel { RequestId = "Failed to process payment" });
            }
        }

        [HttpPost]
        public async Task<IActionResult> ProcessPayment(
            string cardNumber, string cardName, string expiryDate, string cvv,
            string customerName, string email, string address, string city, string zipCode)
        {
            try
            {
                _logger.LogInformation($"Processing payment for {customerName} ({email})");
                _logger.LogInformation($"Shipping to: {address}, {city}, {zipCode}");
                
                await ((CartService)_cartService).ResetCart();
                
                TempData["CustomerName"] = customerName;
                TempData["Email"] = email;
                TempData["Address"] = address;
                TempData["OrderNumber"] = new Random().Next(100000, 999999).ToString();
                
                return RedirectToAction(nameof(Confirmation));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error processing payment");
                return View("Error", new ErrorViewModel { RequestId = "Failed to process payment" });
            }
        }

        public IActionResult Confirmation()
        {
            return View();
        }
    }
} 
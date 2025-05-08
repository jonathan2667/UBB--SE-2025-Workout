// <copyright file="CartController.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Workout.Core.IServices;
using Workout.Core.Models;
using Workout.Core.Services;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.ModelBinding.Binders;

/// <summary>
/// DTO for creating a new cart item
/// </summary>
public class CreateCartItemDto
{
    /// <summary>
    /// Gets or sets the product ID
    /// </summary>
    [Required]
    public int ProductID { get; set; }

    /// <summary>
    /// Gets or sets the user ID
    /// </summary>
    [Required]
    public int UserID { get; set; }
}

/// <summary>
/// Model binder provider for CartItemModel
/// </summary>
public class CartItemModelBinderProvider : IModelBinderProvider
{
    public IModelBinder GetBinder(ModelBinderProviderContext context)
    {
        if (context == null)
        {
            throw new ArgumentNullException(nameof(context));
        }

        if (context.Metadata.ModelType == typeof(CartItemModel))
        {
            return new CartItemModelBinder();
        }

        return null;
    }
}

/// <summary>
/// Custom model binder for CartItemModel
/// </summary>
public class CartItemModelBinder : IModelBinder
{
    public Task BindModelAsync(ModelBindingContext bindingContext)
    {
        if (bindingContext == null)
        {
            throw new ArgumentNullException(nameof(bindingContext));
        }

        var productIdValue = bindingContext.ValueProvider.GetValue("productID");
        var userIdValue = bindingContext.ValueProvider.GetValue("userID");

        if (productIdValue == ValueProviderResult.None || userIdValue == ValueProviderResult.None)
        {
            bindingContext.ModelState.AddModelError("productID", "ProductID is required");
            bindingContext.ModelState.AddModelError("userID", "UserID is required");
            bindingContext.Result = ModelBindingResult.Failed();
            return Task.CompletedTask;
        }

        if (!int.TryParse(productIdValue.FirstValue, out int productId))
        {
            bindingContext.ModelState.AddModelError("productID", "ProductID must be a valid integer");
            bindingContext.Result = ModelBindingResult.Failed();
            return Task.CompletedTask;
        }

        if (!int.TryParse(userIdValue.FirstValue, out int userId))
        {
            bindingContext.ModelState.AddModelError("userID", "UserID must be a valid integer");
            bindingContext.Result = ModelBindingResult.Failed();
            return Task.CompletedTask;
        }

        var model = new CartItemModel
        {
            ProductID = productId,
            UserID = userId
        };

        bindingContext.Result = ModelBindingResult.Success(model);
        return Task.CompletedTask;
    }
}

/// <summary>
/// API controller for managing shopping cart operations.
/// Provides endpoints for retrieving, adding, deleting, and resetting cart items.
/// </summary>
[Route("api/[controller]")]
[ApiController]
public class CartController : ControllerBase
{
    private readonly IService<CartItemModel> cartService;

    /// <summary>
    /// Initializes a new instance of the <see cref="CartController"/> class.
    /// </summary>
    /// <param name="cartService">The service for managing cart items.</param>
    public CartController(IService<CartItemModel> cartService)
    {
        this.cartService = cartService;
    }

    /// <summary>
    /// Retrieves all items in the shopping cart.
    /// </summary>
    /// <returns>An <see cref="IActionResult"/> containing the list of cart items.</returns>
    [HttpGet]
    public async Task<IActionResult> GetAllCartItems()
    {
        try
        {
            var cartItems = await this.cartService.GetAllAsync();
            return this.Ok(cartItems);
        }
        catch (Exception ex)
        {
            return this.BadRequest($"Error fetching cart items: {ex.Message}");
        }
    }

    /// <summary>
    /// Retrieves a specific cart item by its ID.
    /// </summary>
    /// <param name="cartItemId">The ID of the cart item to retrieve.</param>
    /// <returns>An <see cref="IActionResult"/> containing the cart item if found.</returns>
    [HttpGet("{cartItemId}")]
    public async Task<IActionResult> GetCartItemById(int cartItemId)
    {
        try
        {
            var cartItem = await this.cartService.GetByIdAsync(cartItemId);
            if (cartItem == null)
            {
                return this.NotFound($"Cart item with ID {cartItemId} not found.");
            }
            return this.Ok(cartItem);
        }
        catch (Exception ex)
        {
            return this.BadRequest("An error occurred while fetching the cart item.");
        }
    }

    /// <summary>
    /// Adds a new item to the shopping cart.
    /// </summary>
    /// <param name="productID">The ID of the product to add.</param>
    /// <param name="userID">The ID of the user adding the item.</param>
    /// <returns>An <see cref="IActionResult"/> containing the added cart item.</returns>
    [HttpPost]
    public async Task<IActionResult> AddCartItem([FromBody] CartItemRequest request)
    {
        try
        {
            if (request == null)
            {
                return this.BadRequest("Invalid request data.");
            }

            var cartItem = new CartItemModel
            {
                ProductID = request.ProductID,
                UserID = request.UserID
            };

            var addedItem = await this.cartService.CreateAsync(cartItem);
            return this.CreatedAtAction(nameof(this.GetCartItemById), new { cartItemId = addedItem.ID }, addedItem);
        }
        catch (Exception ex)
        {
            return this.BadRequest("An error occurred while adding the item to cart.");
        }
    }

    /// <summary>
    /// Deletes a specific cart item by its ID.
    /// </summary>
    /// <param name="cartItemId">The ID of the cart item to delete.</param>
    /// <returns>An <see cref="IActionResult"/> indicating the result of the operation.</returns>
    [HttpDelete("{cartItemId}")]
    public async Task<IActionResult> DeleteCartItem(int cartItemId)
    {
        try
        {
            var result = await this.cartService.DeleteAsync(cartItemId);
            return result ? this.Ok() : this.NotFound($"Cart item with ID {cartItemId} not found.");
        }
        catch (Exception ex)
        {
            return this.BadRequest($"Error deleting cart item: {ex.Message}");
        }
    }

    /// <summary>
    /// Resets the shopping cart by removing all items.
    /// </summary>
    /// <returns>An <see cref="IActionResult"/> indicating the result of the operation.</returns>
    [HttpDelete("reset")]
    public async Task<IActionResult> ResetCart()
    {
        try
        {
            await ((CartService)this.cartService).ResetCart();
            return this.Ok("Cart has been reset.");
        }
        catch (Exception ex)
        {
            return this.BadRequest($"Error resetting cart: {ex.Message}");
        }
    }
}

/// <summary>
/// Request model for adding items to cart
/// </summary>
public class CartItemRequest
{
    /// <summary>
    /// Gets or sets the product ID
    /// </summary>
    [Required]
    public int ProductID { get; set; }

    /// <summary>
    /// Gets or sets the user ID
    /// </summary>
    [Required]
    public int UserID { get; set; }
}
// <copyright file="CartController.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

using Microsoft.AspNetCore.Mvc;
using Workout.Core.IServices;
using Workout.Core.Models;

using Workout.Core.Services;

/// <summary>
/// API controller for managing shopping cart operations.
/// Provides endpoints for retrieving, adding, deleting, and resetting cart items.
/// </summary>
[Route("api/[controller]")]
[ApiController]
public class CartController : ControllerBase
{
    private readonly IService<CartItemModel> cartService;
    private readonly ILogger<CartController> logger;

    /// <summary>
    /// Initializes a new instance of the <see cref="CartController"/> class.
    /// </summary>
    /// <param name="cartService">The service for managing cart items.</param>
    /// <param name="logger">The logger for logging errors.</param>
    public CartController(IService<CartItemModel> cartService, ILogger<CartController> logger)
    {
        this.cartService = cartService;
        this.logger = logger;
    }

    /// <summary>
    /// Retrieves all items in the shopping cart.
    /// </summary>
    /// <returns>An <see cref="IActionResult"/> containing the list of cart items.</returns>
    [HttpGet]
    public async Task<ActionResult<IEnumerable<CartItemModel>>> GetAllCartItems()
    {
        try
        {
            var cartItems = await this.cartService.GetAllAsync();
            return this.Ok(cartItems);
        }
        catch (Exception ex)
        {
            this.logger.LogError(ex, "Error retrieving cart items");
            return this.StatusCode(500, "An error occurred while retrieving cart items");
        }
    }

    /// <summary>
    /// Retrieves a specific cart item by its ID.
    /// </summary>
    /// <param name="id">The ID of the cart item to retrieve.</param>
    /// <returns>An <see cref="IActionResult"/> containing the cart item if found.</returns>
    [HttpGet("{id}")]
    public async Task<ActionResult<CartItemModel>> GetCartItem(int id)
    {
        try
        {
            var cartItem = await this.cartService.GetByIdAsync(id);
            if (cartItem == null)
            {
                return this.NotFound($"Cart item with ID {id} not found");
            }

            return this.Ok(cartItem);
        }
        catch (KeyNotFoundException ex)
        {
            return this.NotFound(ex.Message);
        }
        catch (Exception ex)
        {
            this.logger.LogError(ex, "Error retrieving cart item {Id}", id);
            return this.StatusCode(500, "An error occurred while retrieving the cart item");
        }
    }

    /// <summary>
    /// Adds a new item to the shopping cart.
    /// </summary>
    /// <param name="cartItem">The cart item to add.</param>
    /// <returns>An <see cref="IActionResult"/> containing the added cart item.</returns>
    [HttpPost]
    public async Task<ActionResult<CartItemModel>> AddCartItem([FromBody] CartItemModel cartItem)
    {
        if (cartItem == null)
        {
            return this.BadRequest("Invalid request data");
        }

        if (!this.ModelState.IsValid)
        {
            return this.BadRequest(this.ModelState);
        }

        try
        {
            var result = await this.cartService.CreateAsync(cartItem);
            return this.CreatedAtAction(nameof(this.GetCartItem), new { id = result.ID }, result);
        }
        catch (Exception ex)
        {
            this.logger.LogError(ex, "Error adding cart item");
            return this.StatusCode(500, "An error occurred while adding the cart item");
        }
    }

    /// <summary>
    /// Updates a specific cart item by its ID.
    /// </summary>
    /// <param name="id">The ID of the cart item to update.</param>
    /// <param name="cartItem">The updated cart item.</param>
    /// <returns>An <see cref="IActionResult"/> containing the updated cart item.</returns>
    [HttpPut("{id}")]
    public async Task<ActionResult<CartItemModel>> UpdateCartItem(int id, [FromBody] CartItemModel cartItem)
    {
        if (cartItem == null)
        {
            return this.BadRequest("Invalid request data");
        }

        if (!this.ModelState.IsValid)
        {
            return this.BadRequest(this.ModelState);
        }

        try
        {
            cartItem.ID = id;
            var result = await this.cartService.UpdateAsync(cartItem);
            return this.Ok(result);
        }
        catch (Exception ex)
        {
            this.logger.LogError(ex, "Error updating cart item {Id}", id);
            return this.StatusCode(500, "An error occurred while updating the cart item");
        }
    }

    /// <summary>
    /// Deletes a specific cart item by its ID.
    /// </summary>
    /// <param name="id">The ID of the cart item to delete.</param>
    /// <returns>An <see cref="IActionResult"/> indicating the result of the operation.</returns>
    [HttpDelete("{id}")]
    public async Task<IActionResult> RemoveCartItem(int id)
    {
        try
        {
            var result = await this.cartService.DeleteAsync(id);
            if (!result)
            {
                return this.NotFound($"Cart item with ID {id} not found");
            }

            return this.NoContent();
        }
        catch (Exception ex)
        {
            this.logger.LogError(ex, "Error removing cart item {Id}", id);
            return this.StatusCode(500, "An error occurred while removing the cart item");
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
            return this.Ok("Cart has been reset");
        }
        catch (Exception ex)
        {
            this.logger.LogError(ex, "Error resetting cart");
            return this.StatusCode(500, "An error occurred while resetting the cart");
        }
    }
}
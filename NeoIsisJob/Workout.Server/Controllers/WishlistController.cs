// <copyright file="WishlistController.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace Workout.Server.Controllers
{
    using Microsoft.AspNetCore.Mvc;
    using Workout.Core.IServices;
    using Workout.Core.Models;

    /// <summary>
    /// API controller for managing wishlist items.
    /// Provides endpoints for retrieving, adding, updating, and deleting wishlist items.
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class WishlistController : ControllerBase
    {
        private readonly IService<WishlistItemModel> wishlistService;
        private readonly ILogger<WishlistController> logger;

        /// <summary>
        /// Initializes a new instance of the <see cref="WishlistController"/> class.
        /// </summary>
        /// <param name="wishlistService">The service for managing wishlist items.</param>
        /// <param name="logger">The logger for logging errors.</param>
        public WishlistController(IService<WishlistItemModel> wishlistService, ILogger<WishlistController> logger)
        {
            this.wishlistService = wishlistService;
            this.logger = logger;
        }

        /// <summary>
        /// Retrieves all wishlist items.
        /// </summary>
        /// <returns>An <see cref="ActionResult"/> containing the list of wishlist items.</returns>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<WishlistItemModel>>> GetAllWishlistItems()
        {
            try
            {
                var wishlistItems = await this.wishlistService.GetAllAsync();
                return this.Ok(wishlistItems);
            }
            catch (Exception ex)
            {
                this.logger.LogError(ex, "Error retrieving wishlist items");
                return this.StatusCode(500, "An error occurred while retrieving wishlist items");
            }
        }

        /// <summary>
        /// Retrieves a specific wishlist item by its ID.
        /// </summary>
        /// <param name="id">The ID of the wishlist item to retrieve.</param>
        /// <returns>An <see cref="ActionResult"/> containing the wishlist item if found.</returns>
        [HttpGet("{id}")]
        public async Task<ActionResult<WishlistItemModel>> GetWishlistItem(int id)
        {
            try
            {
                var wishlistItem = await this.wishlistService.GetByIdAsync(id);
                if (wishlistItem == null)
                {
                    return this.NotFound($"Wishlist item with ID {id} not found");
                }

                return this.Ok(wishlistItem);
            }
            catch (KeyNotFoundException ex)
            {
                return this.NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                this.logger.LogError(ex, "Error retrieving wishlist item {Id}", id);
                return this.StatusCode(500, "An error occurred while retrieving the wishlist item");
            }
        }

        /// <summary>
        /// Adds a new wishlist item.
        /// </summary>
        /// <param name="wishlistItem">The wishlist item to add.</param>
        /// <returns>An <see cref="ActionResult"/> containing the added wishlist item.</returns>
        [HttpPost]
        public async Task<ActionResult<WishlistItemModel>> AddWishlistItem([FromBody] WishlistItemModel wishlistItem)
        {
            if (wishlistItem == null)
            {
                return this.BadRequest("Invalid request data");
            }

            if (!this.ModelState.IsValid)
            {
                return this.BadRequest(this.ModelState);
            }

            try
            {
                var result = await this.wishlistService.CreateAsync(wishlistItem);
                return this.CreatedAtAction(nameof(this.GetWishlistItem), new { id = result.ID }, result);
            }
            catch (Exception ex)
            {
                this.logger.LogError(ex, "Error adding wishlist item");
                return this.StatusCode(500, "An error occurred while adding the wishlist item");
            }
        }

        /// <summary>
        /// Updates an existing wishlist item.
        /// </summary>
        /// <param name="id">The ID of the wishlist item to update.</param>
        /// <param name="wishlistItem">The updated wishlist item.</param>
        /// <returns>An <see cref="ActionResult"/> containing the updated wishlist item.</returns>
        [HttpPut("{id}")]
        public async Task<ActionResult<WishlistItemModel>> UpdateWishlistItem(int id, [FromBody] WishlistItemModel wishlistItem)
        {
            if (wishlistItem == null)
            {
                return this.BadRequest("Invalid request data");
            }

            if (!this.ModelState.IsValid)
            {
                return this.BadRequest(this.ModelState);
            }

            try
            {
                wishlistItem.ID = id;
                var result = await this.wishlistService.UpdateAsync(wishlistItem);
                return this.Ok(result);
            }
            catch (Exception ex)
            {
                this.logger.LogError(ex, "Error updating wishlist item {Id}", id);
                return this.StatusCode(500, "An error occurred while updating the wishlist item");
            }
        }

        /// <summary>
        /// Deletes a specific wishlist item by its ID.
        /// </summary>
        /// <param name="id">The ID of the wishlist item to delete.</param>
        /// <returns>An <see cref="IActionResult"/> indicating the result of the operation.</returns>
        [HttpDelete("{id}")]
        public async Task<IActionResult> RemoveWishlistItem(int id)
        {
            try
            {
                var result = await this.wishlistService.DeleteAsync(id);
                if (!result)
                {
                    return this.NotFound($"Wishlist item with ID {id} not found");
                }

                return this.NoContent();
            }
            catch (Exception ex)
            {
                this.logger.LogError(ex, "Error removing wishlist item {Id}", id);
                return this.StatusCode(500, "An error occurred while removing the wishlist item");
            }
        }
    }
}
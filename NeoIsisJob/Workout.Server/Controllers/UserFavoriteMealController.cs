// <copyright file="UserFavoriteMealController.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace Workout.Server.Controllers
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.Logging;
    using Workout.Core.Models;
    using Workout.Core.Services;

    /// <summary>
    /// API controller for managing user favorite meals.
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    public class UserFavoriteMealController : ControllerBase
    {
        private readonly UserFavoriteMealService favoriteMealService;
        private readonly ILogger<UserFavoriteMealController> logger;

        /// <summary>
        /// Initializes a new instance of the <see cref="UserFavoriteMealController"/> class.
        /// </summary>
        /// <param name="favoriteMealService">The service for managing user favorite meals.</param>
        /// <param name="logger">The logger for logging errors.</param>
        public UserFavoriteMealController(UserFavoriteMealService favoriteMealService, ILogger<UserFavoriteMealController> logger)
        {
            this.favoriteMealService = favoriteMealService;
            this.logger = logger;
        }

        /// <summary>
        /// Gets all favorite meals for a specific user.
        /// </summary>
        /// <param name="userId">The user identifier.</param>
        /// <returns>A list of user's favorite meals.</returns>
        [HttpGet("{userId}")]
        public async Task<ActionResult<IEnumerable<UserFavoriteMealModel>>> GetUserFavorites(int userId)
        {
            try
            {
                var favorites = await this.favoriteMealService.GetUserFavoritesAsync(userId);
                return this.Ok(favorites);
            }
            catch (Exception ex)
            {
                this.logger.LogError(ex, "Error retrieving user favorites for user {UserId}", userId);
                return this.StatusCode(500, "An error occurred while retrieving favorite meals");
            }
        }

        /// <summary>
        /// Adds a meal to user's favorites.
        /// </summary>
        /// <param name="userId">The user identifier.</param>
        /// <param name="mealId">The meal identifier.</param>
        /// <returns>The created favorite meal entry.</returns>
        [HttpPost("{userId}/{mealId}")]
        public async Task<ActionResult<UserFavoriteMealModel>> AddToFavorites(int userId, int mealId)
        {
            try
            {
                var favorite = await this.favoriteMealService.AddToFavoritesAsync(userId, mealId);
                return this.CreatedAtAction(nameof(GetUserFavorites), new { userId }, favorite);
            }
            catch (System.InvalidOperationException ex)
            {
                return this.BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                this.logger.LogError(ex, "Error adding meal {MealId} to favorites for user {UserId}", mealId, userId);
                return this.StatusCode(500, "An error occurred while adding meal to favorites");
            }
        }

        /// <summary>
        /// Removes a meal from user's favorites.
        /// </summary>
        /// <param name="userId">The user identifier.</param>
        /// <param name="mealId">The meal identifier.</param>
        /// <returns>No content if successful.</returns>
        [HttpDelete("{userId}/{mealId}")]
        public async Task<IActionResult> RemoveFromFavorites(int userId, int mealId)
        {
            try
            {
                var removed = await this.favoriteMealService.RemoveFromFavoritesAsync(userId, mealId);
                if (!removed)
                {
                    return this.NotFound("Favorite meal not found");
                }
                return this.NoContent();
            }
            catch (Exception ex)
            {
                this.logger.LogError(ex, "Error removing meal {MealId} from favorites for user {UserId}", mealId, userId);
                return this.StatusCode(500, "An error occurred while removing meal from favorites");
            }
        }

        /// <summary>
        /// Checks if a meal is in user's favorites.
        /// </summary>
        /// <param name="userId">The user identifier.</param>
        /// <param name="mealId">The meal identifier.</param>
        /// <returns>True if meal is favorite, false otherwise.</returns>
        [HttpGet("{userId}/{mealId}/isfavorite")]
        public async Task<ActionResult<bool>> IsMealFavorite(int userId, int mealId)
        {
            try
            {
                var isFavorite = await this.favoriteMealService.IsMealFavoriteAsync(userId, mealId);
                return this.Ok(isFavorite);
            }
            catch (Exception ex)
            {
                this.logger.LogError(ex, "Error checking if meal {MealId} is favorite for user {UserId}", mealId, userId);
                return this.StatusCode(500, "An error occurred while checking favorite status");
            }
        }
    }
} 
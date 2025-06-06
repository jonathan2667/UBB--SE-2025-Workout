// <copyright file="MealController.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace Workout.Server.Controllers
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.Logging;
    using Workout.Core.IServices;
    using Workout.Core.Models;
    using Workout.Core.Utils.Filters;

    /// <summary>
    /// API controller for managing meals.
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    public class MealController : ControllerBase
    {
        private readonly IService<MealModel> mealService;
        private readonly ILogger<MealController> logger;

        /// <summary>
        /// Initializes a new instance of the <see cref="MealController"/> class.
        /// </summary>
        /// <param name="mealService">The service for managing meals.</param>
        /// <param name="logger">The logger for logging errors.</param>
        public MealController(IService<MealModel> mealService, ILogger<MealController> logger)
        {
            this.mealService = mealService;
            this.logger = logger;
        }

        /// <summary>
        /// Gets all meals.
        /// </summary>
        /// <returns>A list of all meals.</returns>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<MealModel>>> GetAllMeals()
        {
            try
            {
                var meals = await this.mealService.GetAllAsync();
                return this.Ok(meals);
            }
            catch (Exception ex)
            {
                this.logger.LogError(ex, "Error retrieving meals");
                return this.StatusCode(500, "An error occurred while retrieving meals");
            }
        }

        /// <summary>
        /// Gets a specific meal by ID.
        /// </summary>
        /// <param name="id">The ID of the meal.</param>
        /// <returns>The meal with the specified ID.</returns>
        [HttpGet("{id}")]
        public async Task<ActionResult<MealModel>> GetMeal(int id)
        {
            try
            {
                var meal = await this.mealService.GetByIdAsync(id);
                if (meal == null)
                {
                    return this.NotFound($"Meal with ID {id} not found");
                }

                return this.Ok(meal);
            }
            catch (Exception ex)
            {
                this.logger.LogError(ex, "Error retrieving meal {Id}", id);
                return this.StatusCode(500, "An error occurred while retrieving the meal");
            }
        }

        /// <summary>
        /// Creates a new meal.
        /// </summary>
        /// <param name="meal">The meal model.</param>
        /// <returns>The created meal.</returns>
        [HttpPost]
        public async Task<ActionResult<MealModel>> AddMeal([FromBody] MealModel meal)
        {
            if (meal == null)
            {
                return this.BadRequest("Invalid request data");
            }

            if (!this.ModelState.IsValid)
            {
                return this.BadRequest(this.ModelState);
            }

            try
            {
                var result = await this.mealService.CreateAsync(meal);
                return this.CreatedAtAction(nameof(this.GetMeal), new { id = result.Id }, result);
            }
            catch (Exception ex)
            {
                this.logger.LogError(ex, "Error adding meal");
                return this.StatusCode(500, "An error occurred while adding the meal");
            }
        }

        /// <summary>
        /// Updates an existing meal.
        /// </summary>
        /// <param name="id">The ID of the meal.</param>
        /// <param name="meal">The updated meal model.</param>
        /// <returns>The updated meal.</returns>
        [HttpPut("{id}")]
        public async Task<ActionResult<MealModel>> UpdateMeal(int id, [FromBody] MealModel meal)
        {
            if (meal == null)
            {
                return this.BadRequest("Invalid request data");
            }

            if (!this.ModelState.IsValid)
            {
                return this.BadRequest(this.ModelState);
            }

            try
            {
                meal.Id = id;
                var result = await this.mealService.UpdateAsync(meal);
                return this.Ok(result);
            }
            catch (Exception ex)
            {
                this.logger.LogError(ex, "Error updating meal {Id}", id);
                return this.StatusCode(500, "An error occurred while updating the meal");
            }
        }

        /// <summary>
        /// Deletes a meal by ID.
        /// </summary>
        /// <param name="id">The ID of the meal.</param>
        /// <returns>No content if successful, or NotFound if not.</returns>
        [HttpDelete("{id}")]
        public async Task<IActionResult> RemoveMeal(int id)
        {
            try
            {
                var result = await this.mealService.DeleteAsync(id);
                if (!result)
                {
                    return this.NotFound($"Meal with ID {id} not found");
                }

                return this.NoContent();
            }
            catch (Exception ex)
            {
                this.logger.LogError(ex, "Error removing meal {Id}", id);
                return this.StatusCode(500, "An error occurred while removing the meal");
            }
        }

        /// <summary>
        /// Gets meals based on a filter.
        /// </summary>
        /// <param name="filter">The filter to apply.</param>
        /// <returns>A filtered list of meals.</returns>
        [HttpPost("filter")]
        public async Task<ActionResult<IEnumerable<MealModel>>> GetFilteredMeals([FromBody] MealFilter filter)
        {
            try
            {
                IEnumerable<MealModel> meals = await this.mealService.GetFilteredAsync(filter);
                return this.Ok(meals);
            }
            catch (Exception ex)
            {
                this.logger.LogError(ex, "Error filtering meals");
                return this.StatusCode(500, "An error occurred while filtering meals");
            }
        }
    }
}

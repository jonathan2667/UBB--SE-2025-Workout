// <copyright file="CategoryController.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace Workout.Server.Controllers
{
    using Microsoft.AspNetCore.Mvc;
    using Workout.Core.IServices;
    using Workout.Core.Models;

    /// <summary>
    /// API controller for managing category operations.
    /// Provides endpoints for retrieving, adding, updating, and deleting categories.
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class CategoryController : ControllerBase
    {
        private readonly IService<CategoryModel> categoryService;
        private readonly ILogger<CategoryController> logger;

        /// <summary>
        /// Initializes a new instance of the <see cref="CategoryController"/> class.
        /// </summary>
        /// <param name="categoryService">The service for managing categories.</param>
        /// <param name="logger">The logger for logging errors.</param>
        public CategoryController(IService<CategoryModel> categoryService, ILogger<CategoryController> logger)
        {
            this.categoryService = categoryService;
            this.logger = logger;
        }

        /// <summary>
        /// Retrieves all categories.
        /// </summary>
        /// <returns>An <see cref="ActionResult"/> containing the list of categories.</returns>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<CategoryModel>>> GetAllCategories()
        {
            try
            {
                var categories = await this.categoryService.GetAllAsync();
                return this.Ok(categories);
            }
            catch (Exception ex)
            {
                this.logger.LogError(ex, "Error retrieving categories");
                return this.StatusCode(500, "An error occurred while retrieving categories");
            }
        }

        /// <summary>
        /// Retrieves a specific category by its ID.
        /// </summary>
        /// <param name="id">The ID of the category to retrieve.</param>
        /// <returns>An <see cref="ActionResult"/> containing the category if found.</returns>
        [HttpGet("{id}")]
        public async Task<ActionResult<CategoryModel>> GetCategory(int id)
        {
            try
            {
                var category = await this.categoryService.GetByIdAsync(id);
                if (category == null)
                {
                    return this.NotFound($"Category with ID {id} not found");
                }

                return this.Ok(category);
            }
            catch (KeyNotFoundException ex)
            {
                return this.NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                this.logger.LogError(ex, "Error retrieving category {Id}", id);
                return this.StatusCode(500, "An error occurred while retrieving the category");
            }
        }

        /// <summary>
        /// Adds a new category.
        /// </summary>
        /// <param name="category">The category to add.</param>
        /// <returns>An <see cref="ActionResult"/> containing the added category.</returns>
        [HttpPost]
        public async Task<ActionResult<CategoryModel>> AddCategory([FromBody] CategoryModel category)
        {
            if (category == null)
            {
                return this.BadRequest("Invalid request data");
            }

            if (!this.ModelState.IsValid)
            {
                return this.BadRequest(this.ModelState);
            }

            try
            {
                var result = await this.categoryService.CreateAsync(category);
                return this.CreatedAtAction(nameof(this.GetCategory), new { id = result.ID }, result);
            }
            catch (Exception ex)
            {
                this.logger.LogError(ex, "Error adding category");
                return this.StatusCode(500, "An error occurred while adding the category");
            }
        }

        /// <summary>
        /// Updates a specific category by its ID.
        /// </summary>
        /// <param name="id">The ID of the category to update.</param>
        /// <param name="category">The updated category.</param>
        /// <returns>An <see cref="ActionResult"/> containing the updated category.</returns>
        [HttpPut("{id}")]
        public async Task<ActionResult<CategoryModel>> UpdateCategory(int id, [FromBody] CategoryModel category)
        {
            if (category == null)
            {
                return this.BadRequest("Invalid request data");
            }

            if (!this.ModelState.IsValid)
            {
                return this.BadRequest(this.ModelState);
            }

            try
            {
                category.ID = id;
                var result = await this.categoryService.UpdateAsync(category);
                return this.Ok(result);
            }
            catch (Exception ex)
            {
                this.logger.LogError(ex, "Error updating category {Id}", id);
                return this.StatusCode(500, "An error occurred while updating the category");
            }
        }

        /// <summary>
        /// Deletes a specific category by its ID.
        /// </summary>
        /// <param name="id">The ID of the category to delete.</param>
        /// <returns>An <see cref="IActionResult"/> indicating the result of the operation.</returns>
        [HttpDelete("{id}")]
        public async Task<IActionResult> RemoveCategory(int id)
        {
            try
            {
                var result = await this.categoryService.DeleteAsync(id);
                if (!result)
                {
                    return this.NotFound($"Category with ID {id} not found");
                }

                return this.NoContent();
            }
            catch (Exception ex)
            {
                this.logger.LogError(ex, "Error removing category {Id}", id);
                return this.StatusCode(500, "An error occurred while removing the category");
            }
        }
    }
}
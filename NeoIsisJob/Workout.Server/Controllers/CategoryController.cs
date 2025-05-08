using Microsoft.AspNetCore.Mvc;
using Workout.Core.IServices;
using Workout.Core.Models;

namespace Workout.Server.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CategoryController : ControllerBase
    {
        private readonly IService<CategoryModel> categoryService;
        private readonly ILogger<CategoryController> logger;

        public CategoryController(IService<CategoryModel> categoryService, ILogger<CategoryController> logger)
        {
            this.categoryService = categoryService;
            this.logger = logger;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<CategoryModel>>> GetAllCategories()
        {
            try
            {
                var categories = await categoryService.GetAllAsync();
                return Ok(categories);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error retrieving categories");
                return StatusCode(500, "An error occurred while retrieving categories");
            }
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<CategoryModel>> GetCategory(int id)
        {
            try
            {
                var category = await categoryService.GetByIdAsync(id);
                if (category == null)
                {
                    return NotFound($"Category with ID {id} not found");
                }
                return Ok(category);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error retrieving category {Id}", id);
                return StatusCode(500, "An error occurred while retrieving the category");
            }
        }

        [HttpPost]
        public async Task<ActionResult<CategoryModel>> CreateCategory([FromBody] CategoryRequest request)
        {
            if (request == null)
            {
                return BadRequest("Invalid request data");
            }

            try
            {
                var category = new CategoryModel
                {
                    Name = request.Name
                };

                var result = await categoryService.CreateAsync(category);
                return CreatedAtAction(nameof(GetCategory), new { id = result.ID }, result);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error creating category");
                return StatusCode(500, "An error occurred while creating the category");
            }
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<CategoryModel>> UpdateCategory(int id, [FromBody] CategoryRequest request)
        {
            if (request == null)
            {
                return BadRequest("Invalid request data");
            }

            try
            {
                var existingCategory = await categoryService.GetByIdAsync(id);
                if (existingCategory == null)
                {
                    return NotFound($"Category with ID {id} not found");
                }

                existingCategory.Name = request.Name;

                var result = await categoryService.UpdateAsync(existingCategory);
                return Ok(result);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error updating category {Id}", id);
                return StatusCode(500, "An error occurred while updating the category");
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCategory(int id)
        {
            try
            {
                var result = await categoryService.DeleteAsync(id);
                if (!result)
                {
                    return NotFound($"Category with ID {id} not found");
                }
                return NoContent();
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error deleting category {Id}", id);
                return StatusCode(500, "An error occurred while deleting the category");
            }
        }
    }

    public class CategoryRequest
    {
        public string Name { get; set; }
    }
} 
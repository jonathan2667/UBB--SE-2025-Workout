using Microsoft.AspNetCore.Mvc;
using Workout.Core.IServices;
using Workout.Core.Models;

namespace Workout.Server.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class WishlistController : ControllerBase
    {
        private readonly IService<WishlistItemModel> wishlistService;
        private readonly ILogger<WishlistController> logger;

        public WishlistController(IService<WishlistItemModel> wishlistService, ILogger<WishlistController> logger)
        {
            this.wishlistService = wishlistService;
            this.logger = logger;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<WishlistItemModel>>> GetAllWishlistItems()
        {
            try
            {
                var items = await wishlistService.GetAllAsync();
                return Ok(items);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error retrieving wishlist items");
                return StatusCode(500, "An error occurred while retrieving wishlist items");
            }
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<WishlistItemModel>> GetWishlistItem(int id)
        {
            try
            {
                var item = await wishlistService.GetByIdAsync(id);
                if (item == null)
                {
                    return NotFound($"Wishlist item with ID {id} not found");
                }
                return Ok(item);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error retrieving wishlist item {Id}", id);
                return StatusCode(500, "An error occurred while retrieving the wishlist item");
            }
        }

        [HttpPost]
        public async Task<ActionResult<WishlistItemModel>> AddWishlistItem([FromBody] WishlistItemRequest request)
        {
            if (request == null)
            {
                return BadRequest("Invalid request data");
            }

            try
            {
                var wishlistItem = new WishlistItemModel
                {
                    ProductID = request.ProductID,
                    UserID = request.UserID
                };

                var result = await wishlistService.CreateAsync(wishlistItem);
                return CreatedAtAction(nameof(GetWishlistItem), new { id = result.ID }, result);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error adding wishlist item");
                return StatusCode(500, "An error occurred while adding the wishlist item");
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> RemoveWishlistItem(int id)
        {
            try
            {
                var result = await wishlistService.DeleteAsync(id);
                if (!result)
                {
                    return NotFound($"Wishlist item with ID {id} not found");
                }
                return NoContent();
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error removing wishlist item {Id}", id);
                return StatusCode(500, "An error occurred while removing the wishlist item");
            }
        }
    }

    public class WishlistItemRequest
    {
        public int ProductID { get; set; }
        public int UserID { get; set; }
    }
} 
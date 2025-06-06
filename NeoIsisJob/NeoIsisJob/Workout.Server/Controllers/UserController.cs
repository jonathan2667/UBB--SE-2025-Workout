// Workout.Server/Controllers/UserController.cs
using Microsoft.AspNetCore.Mvc;
using Workout.Core.IServices;
using Workout.Core.Models;

namespace Workout.Server.Controllers
{
    [ApiController]
    [Route("api/user")]
    public class UserController : ControllerBase
    {
        private readonly IUserService userService;

        public UserController(IUserService userService)
            => this.userService = userService;

        // GET /api/user/{userId}
        [HttpGet("{userId}")]
        public async Task<IActionResult> GetUserById(int userId)
        {
            try
            {
                UserModel user = await userService.GetUserAsync(userId);
                if (user == null)
                {
                    return NotFound();
                }
                return Ok(user);
            }
            catch (Exception ex)
            {
                return BadRequest($"Error fetching user: {ex.Message}");
            }
        }

        // GET /api/user
        [HttpGet]
        public async Task<IActionResult> GetAllUsers()
        {
            try
            {
                IEnumerable<UserModel> users = await userService.GetAllUsersAsync();
                return Ok(users);
            }
            catch (Exception ex)
            {
                return BadRequest($"Error fetching users: {ex.Message}");
            }
        }

        // POST /api/user
        [HttpPost]
        public async Task<IActionResult> AddUser()
        {
            try
            {
                // RegisterNewUserAsync returns the new user's ID
                int newUserId = await userService.RegisterNewUserAsync();
                // return 200 OK with the new ID in the body
                return Ok(newUserId);
            }
            catch (Exception ex)
            {
                return BadRequest($"Error adding user: {ex.Message}");
            }
        }

        // DELETE /api/user/{userId}
        [HttpDelete("{userId}")]
        public async Task<IActionResult> DeleteUser(int userId)
        {
            try
            {
                await userService.RemoveUserAsync(userId);
                return NoContent();
            }
            catch (Exception ex)
            {
                return BadRequest($"Error deleting user: {ex.Message}");
            }
        }

        [HttpGet("user")]
        public ActionResult<UserModel> GetUserByUsername([FromQuery] string username)
        {
            try
            {
                return this.userService.GetUserByUsername(username);
            }
            catch (Exception e)
            {
                return this.BadRequest(e.Message);
            }
        }
    }
}

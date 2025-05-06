//using Microsoft.AspNetCore.Mvc;
//using Workout.Core.IServices;

//namespace Workout.Server.Controllers
//{
//    [ApiController]
//    [Route("api/[controller]")]
//    public class UserController : ControllerBase
//    {
//        private readonly IUserService userService;
//        public UserController(IUserService userService)
//        {
//            this.userService = userService;
//        }
//        [HttpGet("api/user/{userId}")]
//        public async Task<IActionResult> GetUserById(int userId)
//        {
//            try
//            {
//                var user = await userService.GetUserAsync(userId);
//                return Ok(user);
//            }
//            catch (Exception ex)
//            {
//                return BadRequest($"Error fetching user: {ex.Message}");
//            }
//        }

//        [HttpGet("api/user")]
//        public async Task<IActionResult> GetAllUsers()
//        {
//            try
//            {
//                var users = await userService.GetAllUsersAsync();
//                return Ok(users);
//            }
//            catch (Exception ex)
//            {
//                return BadRequest($"Error fetching users: {ex.Message}");
//            }
//        }

//        [HttpPost("api/user")]
//        public async Task<IActionResult> AddUser()
//        {
//            try
//            {
//                await userService.RegisterNewUserAsync();
//                return Ok();
//            }
//            catch (Exception ex)
//            {
//                return BadRequest($"Error adding user: {ex.Message}");
//            }
//        }

//        [HttpDelete("api/user/{userId}")]
//        public async Task<IActionResult> DeleteUser(int userId)
//        {
//            try
//            {
//                await userService.RemoveUserAsync(userId);
//                return Ok();
//            }
//            catch (Exception ex)
//            {
//                return BadRequest($"Error deleting user: {ex.Message}");
//            }
//        }
//    }
//}

// Workout.Server/Controllers/UserController.cs
using Microsoft.AspNetCore.Mvc;
using Workout.Core.IServices;
using Workout.Core.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Workout.Server.Controllers
{
    [ApiController]
    [Route("api/user")]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;
        public UserController(IUserService userService)
            => _userService = userService;

        // GET /api/user/{userId}
        [HttpGet("{userId}")]
        public async Task<IActionResult> GetUserById(int userId)
        {
            try
            {
                UserModel user = await _userService.GetUserAsync(userId);
                if (user == null) return NotFound();
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
                IEnumerable<UserModel> users = await _userService.GetAllUsersAsync();
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
                int newUserId = await _userService.RegisterNewUserAsync();
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
                await _userService.RemoveUserAsync(userId);
                return NoContent();
            }
            catch (Exception ex)
            {
                return BadRequest($"Error deleting user: {ex.Message}");
            }
        }
    }
}



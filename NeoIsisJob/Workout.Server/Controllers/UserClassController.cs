using Microsoft.AspNetCore.Mvc;
using Workout.Core.IServices;
using Workout.Core.Models;

namespace Workout.Server.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserClassController : ControllerBase
    {
        private readonly IUserClassService userClassService;
        public UserClassController(IUserClassService userClassService)
        {
            this.userClassService = userClassService;
        }
        [HttpGet("api/userclass")]
        public async Task<IActionResult> GetAllUserClasses(int userId)
        {
            try
            {
                var userClasses = await userClassService.GetAllUserClassesAsync();
                return Ok(userClasses);
            }
            catch (Exception ex)
            {
                return BadRequest($"Error fetching user classes: {ex.Message}");
            }
        }
        [HttpGet("api/userclass/{userId}/{classId}/{date}")]
        public async Task<IActionResult> GetUserClassById(int userId, int classId, DateTime date)
        {
            try
            {
                var userClass = await userClassService.GetUserClassByIdAsync(userId, classId, date);
                return Ok(userClass);
            }
            catch (Exception ex)
            {
                return BadRequest($"Error fetching user class: {ex.Message}");
            }
        }

        [HttpPost("api/userclass")]
        public async Task<IActionResult> AddUserClass([FromBody] UserClassModel userClass)
        {
            try
            {
                await userClassService.AddUserClassAsync(userClass);
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest($"Error adding user class: {ex.Message}");
            }
        }

        [HttpDelete("api/userclass/{userId}/{classId}/{date}")]
        public async Task<IActionResult> DeleteUserClass(int userId, int classId, DateTime date)
        {
            try
            {
                await userClassService.DeleteUserClassAsync(userId, classId, date);
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest($"Error deleting user class: {ex.Message}");
            }
        }
    }
}

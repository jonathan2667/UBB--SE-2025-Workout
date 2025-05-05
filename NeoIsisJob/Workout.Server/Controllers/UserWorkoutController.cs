using Microsoft.AspNetCore.Mvc;
using Workout.Core.IServices;
using Workout.Core.Models;

namespace Workout.Server.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserWorkoutController : ControllerBase
    {
        private readonly IUserWorkoutService userWorkoutService;

        public UserWorkoutController(IUserWorkoutService userWorkoutService)
        {
            this.userWorkoutService = userWorkoutService;
        }
        [HttpGet("api/userworkout/{userId}/{date}")]
        public async Task<IActionResult> GetUserWorkout(int userId, DateTime date)
        {
            try
            {
                var userWorkout = await userWorkoutService.GetUserWorkoutForDateAsync(userId, date);
                return Ok(userWorkout);
            }
            catch (Exception ex)
            {
                return BadRequest($"Error fetching user workout: {ex.Message}");
            }
        }
        [HttpPost("api/userworkout")]
        public async Task<IActionResult> AddUserWorkout([FromBody] UserWorkoutModel userWorkout)
        {
            try
            {
                await userWorkoutService.AddUserWorkoutAsync(userWorkout);
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest($"Error adding user workout: {ex.Message}");
            }
        }

        [HttpPut("api/userworkout/{userId}/{workoutId}/{date}")]
        public async Task<IActionResult> CompleteUserWorkout(int userId, int workoutId, DateTime date)
        {
            try
            {
                await userWorkoutService.CompleteUserWorkoutAsync(userId, workoutId, date);
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest($"Error updating user workout: {ex.Message}");
            }
        }

        [HttpDelete("api/userworkout/{userId}/{workoutId}/{date}")]
        public async Task<IActionResult> DeleteUserWorkout(int userId, int workoutId, DateTime date)
        {
            try
            {
                await userWorkoutService.DeleteUserWorkoutAsync(userId, workoutId, date);
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest($"Error deleting user workout: {ex.Message}");
            }
        }

    }
}

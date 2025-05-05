using Microsoft.AspNetCore.Mvc;
using Workout.Core.IServices;
using Workout.Core.Models;

namespace Workout.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class WorkoutController : ControllerBase
    {
        private readonly IWorkoutService _workoutService;
        public WorkoutController(IWorkoutService workoutService)
        {
            _workoutService = workoutService;
        }
        [HttpGet("api/workout")]
        public async Task<IActionResult> GetAllWorkouts()
        {
            try
            {
                var workouts = await _workoutService.GetAllWorkoutsAsync();
                return Ok(workouts);
            }
            catch (Exception ex)
            {
                return BadRequest($"Error fetching workouts: {ex.Message}");
            }
        }

        [HttpGet("api/workout/{workoutName}")]
        public async Task<IActionResult> GetWorkoutByName(string workoutName)
        {
            try
            {
                var workout = await _workoutService.GetWorkoutByNameAsync(workoutName);
                return Ok(workout);
            }
            catch (Exception ex)
            {
                return BadRequest($"Error fetching workout: {ex.Message}");
            }
        }

        [HttpPost("api/workout/{workoutName}/{workoutTypeId}")]
        public async Task<IActionResult> AddWorkout(string workoutName, int workoutTypeId)
        {
            try
            {
                await _workoutService.InsertWorkoutAsync(workoutName, workoutTypeId);
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest($"Error adding workout: {ex.Message}");
            }
        }

        [HttpDelete("api/workout/{workoutId}")]
        public async Task<IActionResult> DeleteWorkout(int workoutId)
        {
            try
            {
                await _workoutService.DeleteWorkoutAsync(workoutId);
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest($"Error deleting workout: {ex.Message}");
            }
        }

        [HttpPut("api/workout")]
        public async Task<IActionResult> UpdateWorkout([FromBody] WorkoutModel workout)
        {
            try
            {
                await _workoutService.UpdateWorkoutAsync(workout);
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest($"Error updating workout: {ex.Message}");
            }
        }
    }
}

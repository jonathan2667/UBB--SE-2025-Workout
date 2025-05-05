using Microsoft.AspNetCore.Mvc;
using Workout.Core.IServices;

namespace Workout.Server.Controllers
{
    public class CompleteWorkoutController : ControllerBase
    {

        private readonly ICompleteWorkoutService _completeWorkoutService;

        public CompleteWorkoutController(ICompleteWorkoutService completeWorkoutService)
        {
            _completeWorkoutService = completeWorkoutService;
        }

        [HttpGet("api/completeworkout")]
        public async Task<IActionResult> GetAllCompleteWorkouts()
        {
            try
            {
                var completeWorkouts = await _completeWorkoutService.GetAllCompleteWorkoutsAsync();
                return Ok(completeWorkouts);
            }
            catch (Exception ex)
            {
                return BadRequest($"Error fetching complete workouts: {ex.Message}");
            }
        }

        [HttpGet("api/completeworkout/{workoutId}")]
        public async Task<IActionResult> GetCompleteWorkoutsByWorkoutId(int workoutId)
        {
            try
            {
                var completeWorkouts = await _completeWorkoutService.GetCompleteWorkoutsByWorkoutIdAsync(workoutId);
                return Ok(completeWorkouts);
            }
            catch (Exception ex)
            {
                return BadRequest($"Error fetching complete workouts: {ex.Message}");
            }
        }

        [HttpDelete("api/completeworkout/{workoutId}")]
        public async Task<IActionResult> DeleteCompleteWorkoutsByWorkoutId(int workoutId)
        {
            try
            {
                await _completeWorkoutService.DeleteCompleteWorkoutsByWorkoutIdAsync(workoutId);
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest($"Error deleting complete workouts: {ex.Message}");
            }
        }

        [HttpPost("api/completeworkout/{workoutId}/{exerciseId}/{sets}/{repetitionsPerSet}")]
        public async Task<IActionResult> InsertCompleteWorkout(int workoutId, int exerciseId, int sets, int repetitionsPerSet)
        {
            try
            {
                await _completeWorkoutService.InsertCompleteWorkoutAsync(workoutId, exerciseId, sets, repetitionsPerSet);
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest($"Error inserting complete workout: {ex.Message}");
            }
        }

    }
}

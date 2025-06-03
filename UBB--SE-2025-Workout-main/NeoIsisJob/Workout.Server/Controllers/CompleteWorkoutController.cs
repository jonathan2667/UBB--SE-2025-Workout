//using Microsoft.AspNetCore.Mvc;
//using Workout.Core.IServices;

//namespace Workout.Server.Controllers
//{
//    public class CompleteWorkoutController : ControllerBase
//    {
//        private readonly ICompleteWorkoutService completeWorkoutService;

//        public CompleteWorkoutController(ICompleteWorkoutService completeWorkoutService)
//        {
//            this.completeWorkoutService = completeWorkoutService;
//        }

//        [HttpGet("api/completeworkout")]
//        public async Task<IActionResult> GetAllCompleteWorkouts()
//        {
//            try
//            {
//                var completeWorkouts = await completeWorkoutService.GetAllCompleteWorkoutsAsync();
//                return Ok(completeWorkouts);
//            }
//            catch (Exception ex)
//            {
//                return BadRequest($"Error fetching complete workouts: {ex.Message}");
//            }
//        }

//        [HttpGet("api/completeworkout/{workoutId}")]
//        public async Task<IActionResult> GetCompleteWorkoutsByWorkoutId(int workoutId)
//        {
//            try
//            {
//                var completeWorkouts = await completeWorkoutService.GetCompleteWorkoutsByWorkoutIdAsync(workoutId);
//                return Ok(completeWorkouts);
//            }
//            catch (Exception ex)
//            {
//                return BadRequest($"Error fetching complete workouts: {ex.Message}");
//            }
//        }

//        [HttpDelete("api/completeworkout/{workoutId}")]
//        public async Task<IActionResult> DeleteCompleteWorkoutsByWorkoutId(int workoutId)
//        {
//            try
//            {
//                await completeWorkoutService.DeleteCompleteWorkoutsByWorkoutIdAsync(workoutId);
//                return Ok();
//            }
//            catch (Exception ex)
//            {
//                return BadRequest($"Error deleting complete workouts: {ex.Message}");
//            }
//        }

//        [HttpPost("api/completeworkout/{workoutId}/{exerciseId}/{sets}/{repetitionsPerSet}")]
//        public async Task<IActionResult> InsertCompleteWorkout(int workoutId, int exerciseId, int sets, int repetitionsPerSet)
//        {
//            try
//            {
//                await completeWorkoutService.InsertCompleteWorkoutAsync(workoutId, exerciseId, sets, repetitionsPerSet);
//                return Ok();
//            }
//            catch (Exception ex)
//            {
//                return BadRequest($"Error inserting complete workout: {ex.Message}");
//            }
//        }
//    }
//}
using Microsoft.AspNetCore.Mvc;
using Workout.Core.IServices;

namespace Workout.Server.Controllers
{
    [ApiController]
    [Route("api/completeworkout")]
    public class CompleteWorkoutController : ControllerBase
    {
        private readonly ICompleteWorkoutService _svc;
        public CompleteWorkoutController(ICompleteWorkoutService svc)
            => _svc = svc;

        // GET  /api/completeworkout
        [HttpGet]
        public async Task<IActionResult> GetAll()
            => Ok(await _svc.GetAllCompleteWorkoutsAsync());

        // GET  /api/completeworkout/{workoutId}
        [HttpGet("{workoutId}")]
        public async Task<IActionResult> GetByWorkout(int workoutId)
            => Ok(await _svc.GetCompleteWorkoutsByWorkoutIdAsync(workoutId));

        // DELETE  /api/completeworkout/{workoutId}
        [HttpDelete("{workoutId}")]
        public async Task<IActionResult> DeleteByWorkout(int workoutId)
        {
            await _svc.DeleteCompleteWorkoutsByWorkoutIdAsync(workoutId);
            return NoContent();
        }

        // POST  /api/completeworkout/{workoutId}/{exerciseId}/{sets}/{repetitions}
        [HttpPost("{workoutId}/{exerciseId}/{sets}/{repetitions}")]
        public async Task<IActionResult> Add(
            int workoutId,
            int exerciseId,
            int sets,
            int repetitions)
        {
            await _svc.InsertCompleteWorkoutAsync(workoutId, exerciseId, sets, repetitions);
            return CreatedAtAction(nameof(GetByWorkout), new { workoutId }, null);
        }
    }
}

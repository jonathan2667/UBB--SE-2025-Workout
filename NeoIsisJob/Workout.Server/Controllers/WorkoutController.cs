//using Microsoft.AspNetCore.Mvc;
//using Workout.Core.IServices;
//using Workout.Core.Models;

//namespace Workout.Server.Controllers
//{
//    [Route("api/[controller]")]
//    [ApiController]
//    public class WorkoutController : ControllerBase
//    {
//        private readonly IWorkoutService _workoutService;
//        public WorkoutController(IWorkoutService workoutService)
//        {
//            _workoutService = workoutService;
//        }
//        [HttpGet("api/workout")]
//        public async Task<IActionResult> GetAllWorkouts()
//        {
//            try
//            {
//                var workouts = await _workoutService.GetAllWorkoutsAsync();
//                return Ok(workouts);
//            }
//            catch (Exception ex)
//            {
//                return BadRequest($"Error fetching workouts: {ex.Message}");
//            }
//        }

//        [HttpGet("api/workout/{workoutName}")]
//        public async Task<IActionResult> GetWorkoutByName(string workoutName)
//        {
//            try
//            {
//                var workout = await _workoutService.GetWorkoutByNameAsync(workoutName);
//                return Ok(workout);
//            }
//            catch (Exception ex)
//            {
//                return BadRequest($"Error fetching workout: {ex.Message}");
//            }
//        }

//        [HttpPost("api/workout/{workoutName}/{workoutTypeId}")]
//        public async Task<IActionResult> AddWorkout(string workoutName, int workoutTypeId)
//        {
//            try
//            {
//                await _workoutService.InsertWorkoutAsync(workoutName, workoutTypeId);
//                return Ok();
//            }
//            catch (Exception ex)
//            {
//                return BadRequest($"Error adding workout: {ex.Message}");
//            }
//        }

//        [HttpDelete("api/workout/{workoutId}")]
//        public async Task<IActionResult> DeleteWorkout(int workoutId)
//        {
//            try
//            {
//                await _workoutService.DeleteWorkoutAsync(workoutId);
//                return Ok();
//            }
//            catch (Exception ex)
//            {
//                return BadRequest($"Error deleting workout: {ex.Message}");
//            }
//        }

//        [HttpPut("api/workout")]
//        public async Task<IActionResult> UpdateWorkout([FromBody] WorkoutModel workout)
//        {
//            try
//            {
//                await _workoutService.UpdateWorkoutAsync(workout);
//                return Ok();
//            }
//            catch (Exception ex)
//            {
//                return BadRequest($"Error updating workout: {ex.Message}");
//            }
//        }
//    }
//}



// Workout.Server/Controllers/WorkoutController.cs
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using Workout.Core.IServices;
using Workout.Core.Models;

namespace Workout.Server.Controllers
{
    [ApiController]
    [Route("api/workout")]
    public class WorkoutController : ControllerBase
    {
        private readonly IWorkoutService _workoutService;

        public WorkoutController(IWorkoutService workoutService)
            => _workoutService = workoutService;

        // GET /api/workout
        [HttpGet]
        public async Task<ActionResult<IEnumerable<WorkoutModel>>> GetAll()
        {
            var workouts = await _workoutService.GetAllWorkoutsAsync();
            return Ok(workouts);
        }

        // GET /api/workout/{workoutName}
        [HttpGet("{workoutName}")]
        public async Task<ActionResult<WorkoutModel>> GetByName(string workoutName)
        {
            var workout = await _workoutService.GetWorkoutByNameAsync(workoutName);
            if (workout == null) return NotFound();
            return Ok(workout);
        }

        // POST /api/workout/{workoutName}/{workoutTypeId}
        [HttpPost("{workoutName}/{workoutTypeId}")]
        public async Task<IActionResult> Create(string workoutName, int workoutTypeId)
        {
            await _workoutService.InsertWorkoutAsync(workoutName, workoutTypeId);
            return CreatedAtAction(nameof(GetByName), new { workoutName }, null);
        }

        // DELETE /api/workout/{workoutId}
        [HttpDelete("{workoutId}")]
        public async Task<IActionResult> Delete(int workoutId)
        {
            await _workoutService.DeleteWorkoutAsync(workoutId);
            return NoContent();
        }

        // PUT /api/workout
        [HttpPut]
        public async Task<IActionResult> Update([FromBody] WorkoutModel workout)
        {
            await _workoutService.UpdateWorkoutAsync(workout);
            return NoContent();
        }
    }
}

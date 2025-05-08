using Microsoft.AspNetCore.Mvc;
using Workout.Core.IServices;

namespace Workout.Server.Controllers
{
    [ApiController]
    [Route("api/completeworkout")]
    public class CompleteWorkoutController : ControllerBase
    {
        private readonly ICompleteWorkoutService svc;
        public CompleteWorkoutController(ICompleteWorkoutService svc)
            => this.svc = svc;

        // GET  /api/completeworkout
        [HttpGet]
        public async Task<IActionResult> GetAll()
            => Ok(await svc.GetAllCompleteWorkoutsAsync());

        // GET  /api/completeworkout/{workoutId}
        [HttpGet("{workoutId}")]
        public async Task<IActionResult> GetByWorkout(int workoutId)
            => Ok(await svc.GetCompleteWorkoutsByWorkoutIdAsync(workoutId));

        // DELETE  /api/completeworkout/{workoutId}
        [HttpDelete("{workoutId}")]
        public async Task<IActionResult> DeleteByWorkout(int workoutId)
        {
            await svc.DeleteCompleteWorkoutsByWorkoutIdAsync(workoutId);
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
            await svc.InsertCompleteWorkoutAsync(workoutId, exerciseId, sets, repetitions);
            return CreatedAtAction(nameof(GetByWorkout), new { workoutId }, null);
        }
    }
}

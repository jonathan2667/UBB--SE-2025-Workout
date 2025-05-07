// Workout.Server/Controllers/WorkoutController.cs
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Workout.Core.IServices;
using Workout.Core.Models;

namespace Workout.Server.Controllers
{
    [ApiController]
    [Route("api/workout")]
    public class WorkoutController : ControllerBase
    {
        private readonly IWorkoutService workoutService;

        public WorkoutController(IWorkoutService workoutService)
            => this.workoutService = workoutService;

        // GET /api/workout
        [HttpGet]
        public async Task<ActionResult<IEnumerable<WorkoutModel>>> GetAll()
        {
            var workouts = await workoutService.GetAllWorkoutsAsync();
            return Ok(workouts);
        }

        // GET /api/workout/{workoutName}
        [HttpGet("{workoutName}")]
        [HttpGet("name/{workoutName}")]
        public async Task<ActionResult<WorkoutModel>> GetByName(string workoutName)
        {
            var workout = await workoutService.GetWorkoutByNameAsync(workoutName);
            if (workout == null)
            {
                return NotFound();
            }
            return Ok(workout);
        }

        // POST /api/workout/{workoutName}/{workoutTypeId}
        [HttpPost("{workoutName}/{workoutTypeId}")]
        public async Task<IActionResult> Create(string workoutName, int workoutTypeId)
        {
            await workoutService.InsertWorkoutAsync(workoutName, workoutTypeId);
            return CreatedAtAction(nameof(GetByName), new { workoutName }, null);
        }
        // Workout.Server/Controllers/WorkoutController.cs
        [HttpPost]
        public async Task<IActionResult> CreateFromBody([FromBody] WorkoutModel model)
        {
            if (model == null)
            {
                return BadRequest();
            }
            await workoutService.InsertWorkoutAsync(model.Name, model.WTID, model.Description);
            return CreatedAtAction(nameof(GetByName),
                new { workoutName = model.Name },
                model);
        }

        // DELETE /api/workout/{workoutId}
        [HttpDelete("{workoutId}")]
        public async Task<IActionResult> Delete(int workoutId)
        {
            await workoutService.DeleteWorkoutAsync(workoutId);
            return NoContent();
        }

        // PUT /api/workout
        [HttpPut]
        public async Task<IActionResult> Update([FromBody] WorkoutModel workout)
        {
            await workoutService.UpdateWorkoutAsync(workout);
            return NoContent();
        }
    }
}

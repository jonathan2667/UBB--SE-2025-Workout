using Microsoft.AspNetCore.Mvc;
using Workout.Core.IServices;
namespace Workout.Server.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ExerciseController : ControllerBase
    {
        private readonly IExerciseService exerciseService;

        public ExerciseController(IExerciseService exerciseService)
        {
            this.exerciseService = exerciseService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllExercises()
        {
            try
            {
                var exercises = await exerciseService.GetAllExercisesAsync();
                return Ok(exercises);
            }
            catch (Exception ex)
            {
                return BadRequest($"Error fetching exercises: {ex.Message}");
            }
        }
        
        [HttpGet("{exerciseId}")]
        public async Task<IActionResult> GetExerciseById(int exerciseId)
        {
            try
            {
                var exercise = await exerciseService.GetExerciseByIdAsync(exerciseId);
                return Ok(exercise);
            }
            catch (Exception ex)
            {
                return BadRequest($"Error fetching exercise: {ex.Message}");
            }
        }
    }
}

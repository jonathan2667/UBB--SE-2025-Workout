using Microsoft.AspNetCore.Mvc;
using Workout.Core.IServices;

namespace Workout.Server.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class WorkoutTypeController : ControllerBase
    {
        private readonly IWorkoutTypeService workoutTypeService;
        public WorkoutTypeController(IWorkoutTypeService workoutTypeService)
        {
            this.workoutTypeService = workoutTypeService;
        }

        [HttpGet("api/workouttype")]
        public async Task<IActionResult> GetAllWorkoutTypes()
        {
            try
            {
                var workoutTypes = await workoutTypeService.GetAllWorkoutTypesAsync();
                return Ok(workoutTypes);
            }
            catch (Exception ex)
            {
                return BadRequest($"Error fetching workout types: {ex.Message}");
            }
        }

        [HttpGet("api/workouttype/{workoutTypeId}")]
        public async Task<IActionResult> GetWorkoutTypeById(int workoutTypeId)
        {
            try
            {
                var workoutType = await workoutTypeService.GetWorkoutTypeByIdAsync(workoutTypeId);
                return Ok(workoutType);
            }
            catch (Exception ex)
            {
                return BadRequest($"Error fetching workout type: {ex.Message}");
            }
        }

        [HttpPost("api/workouttype/{workoutTypeName")]
        public async Task<IActionResult> AddWorkoutType([FromBody] string workoutTypeName)
        {
            try
            {
                await workoutTypeService.InsertWorkoutTypeAsync(workoutTypeName);
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest($"Error adding workout type: {ex.Message}");
            }
        }

        [HttpDelete("api/workouttype/{workoutTypeId}")]
        public async Task<IActionResult> DeleteWorkoutType(int workoutTypeId)
        {
            try
            {
                await workoutTypeService.DeleteWorkoutTypeAsync(workoutTypeId);
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest($"Error deleting workout type: {ex.Message}");
            }
        }
    }
}

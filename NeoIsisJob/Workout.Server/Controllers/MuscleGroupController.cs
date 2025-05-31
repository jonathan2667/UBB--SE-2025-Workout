using Microsoft.AspNetCore.Mvc;
using Workout.Core.IServices;

namespace Workout.Server.Controllers
{
    [ApiController]
    [Route("api/musclegroup")]
    public class MuscleGroupController : ControllerBase
    {
        private readonly IMuscleGroupService muscleGroupService;
        public MuscleGroupController(IMuscleGroupService muscleGroupService)
        {
            this.muscleGroupService = muscleGroupService;
        }

        [HttpGet("{muscleGroupId}")]
        public async Task<IActionResult> GetAllMuscleGroups(int muscleGroupId)
        {
            try
            {
                var muscleGroup = await muscleGroupService.GetMuscleGroupByIdAsync(muscleGroupId);
                return Ok(muscleGroup);
            }
            catch (Exception ex)
            {
                return BadRequest($"Error fetching muscle group: {ex.Message}");
            }
        }
    }
}

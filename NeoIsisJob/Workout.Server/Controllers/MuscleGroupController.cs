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
                var muscleGroups = await muscleGroupService.GetMuscleGroupByIdAsync(muscleGroupId);
                return Ok(muscleGroups);
            }
            catch (Exception ex)
            {
                return BadRequest($"Error fetching muscle groups: {ex.Message}");
            }
        }
    }
}

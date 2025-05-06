//using Microsoft.AspNetCore.Mvc;
//using Workout.Core.IServices;

//namespace Workout.Server.Controllers
//{
//    [ApiController]
//    [Route("api/[controller]")]
//    public class WorkoutTypeController : ControllerBase
//    {
//        private readonly IWorkoutTypeService workoutTypeService;
//        public WorkoutTypeController(IWorkoutTypeService workoutTypeService)
//        {
//            this.workoutTypeService = workoutTypeService;
//        }

//        [HttpGet("api/workouttype")]
//        public async Task<IActionResult> GetAllWorkoutTypes()
//        {
//            try
//            {
//                var workoutTypes = await workoutTypeService.GetAllWorkoutTypesAsync();
//                return Ok(workoutTypes);
//            }
//            catch (Exception ex)
//            {
//                return BadRequest($"Error fetching workout types: {ex.Message}");
//            }
//        }

//        [HttpGet("api/workouttype/{workoutTypeId}")]
//        public async Task<IActionResult> GetWorkoutTypeById(int workoutTypeId)
//        {
//            try
//            {
//                var workoutType = await workoutTypeService.GetWorkoutTypeByIdAsync(workoutTypeId);
//                return Ok(workoutType);
//            }
//            catch (Exception ex)
//            {
//                return BadRequest($"Error fetching workout type: {ex.Message}");
//            }
//        }

//        [HttpPost("api/workouttype/{workoutTypeName}")]
//        public async Task<IActionResult> AddWorkoutType([FromBody] string workoutTypeName)
//        {
//            try
//            {
//                await workoutTypeService.InsertWorkoutTypeAsync(workoutTypeName);
//                return Ok();
//            }
//            catch (Exception ex)
//            {
//                return BadRequest($"Error adding workout type: {ex.Message}");
//            }
//        }

//        [HttpDelete("api/workouttype/{workoutTypeId}")]
//        public async Task<IActionResult> DeleteWorkoutType(int workoutTypeId)
//        {
//            try
//            {
//                await workoutTypeService.DeleteWorkoutTypeAsync(workoutTypeId);
//                return Ok();
//            }
//            catch (Exception ex)
//            {
//                return BadRequest($"Error deleting workout type: {ex.Message}");
//            }
//        }
//    }
//}


// Workout.Server/Controllers/WorkoutTypeController.cs
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using Workout.Core.IServices;
using Workout.Core.Models;

namespace Workout.Server.Controllers
{
    [ApiController]
    [Route("api/workouttype")]
    public class WorkoutTypeController : ControllerBase
    {
        private readonly IWorkoutTypeService _service;

        public WorkoutTypeController(IWorkoutTypeService service)
            => _service = service;

        // GET /api/workouttype
        [HttpGet]
        public async Task<ActionResult<IEnumerable<WorkoutTypeModel>>> GetAll()
        {
            var types = await _service.GetAllWorkoutTypesAsync();
            return Ok(types);
        }

        // GET /api/workouttype/{id}
        [HttpGet("{id}")]
        public async Task<ActionResult<WorkoutTypeModel>> GetById(int id)
        {
            var type = await _service.GetWorkoutTypeByIdAsync(id);
            if (type == null) return NotFound();
            return Ok(type);
        }

        // POST /api/workouttype/{name}
        [HttpPost("{name}")]
        public async Task<IActionResult> Create(string name)
        {
            await _service.InsertWorkoutTypeAsync(name);
            return CreatedAtAction(nameof(GetById), new { id = /* assume service returns new Id */ 0 }, null);
        }

        // DELETE /api/workouttype/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            await _service.DeleteWorkoutTypeAsync(id);
            return NoContent();
        }
    }
}

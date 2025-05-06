// Workout.Server/Controllers/WorkoutTypeController.cs
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Workout.Core.IServices;
using Workout.Core.Models;

namespace Workout.Server.Controllers
{
    [ApiController]
    [Route("api/workouttype")]
    public class WorkoutTypeController : ControllerBase
    {
        private readonly IWorkoutTypeService service;

        public WorkoutTypeController(IWorkoutTypeService service)
            => this.service = service;

        // GET /api/workouttype
        [HttpGet]
        public async Task<ActionResult<IEnumerable<WorkoutTypeModel>>> GetAll()
        {
            var types = await service.GetAllWorkoutTypesAsync();
            return Ok(types);
        }

        // GET /api/workouttype/{id}
        [HttpGet("{id}")]
        public async Task<ActionResult<WorkoutTypeModel>> GetById(int id)
        {
            var type = await service.GetWorkoutTypeByIdAsync(id);
            if (type == null)
            {
                return NotFound();
            }
            return Ok(type);
        }

        // POST /api/workouttype/{name}
        [HttpPost("{name}")]
        public async Task<IActionResult> Create(string name)
        {
            await service.InsertWorkoutTypeAsync(name);
            return CreatedAtAction(nameof(GetById), new { id = /* assume service returns new Id */ 0 }, null);
        }

        // DELETE /api/workouttype/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            await service.DeleteWorkoutTypeAsync(id);
            return NoContent();
        }
    }
}

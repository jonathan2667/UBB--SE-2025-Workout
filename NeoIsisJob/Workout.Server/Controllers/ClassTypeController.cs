using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Workout.Core.IServices;
using Workout.Core.Models;

namespace Workout.Server.Controllers
{
    [ApiController]
    [Route("api/classtype")]
    public class ClassTypeController : ControllerBase
    {
        private readonly IClassTypeService classTypeService;

        public ClassTypeController(IClassTypeService classTypeService)
        {
            this.classTypeService = classTypeService;
        }

        // GET /api/classtype
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ClassTypeModel>>> GetAll()
        {
            var items = await classTypeService.GetAllClassTypesAsync();
            return Ok(items);
        }

        // GET /api/classtype/{id}
        [HttpGet("{id}")]
        public async Task<ActionResult<ClassTypeModel>> GetById(int id)
        {
            var item = await classTypeService.GetClassTypeByIdAsync(id);
            if (item == null)
            {
                return NotFound();
            }
            return Ok(item);
        }

        // POST /api/classtype
        [HttpPost]
        public async Task<ActionResult<ClassTypeModel>> Create([FromBody] ClassTypeModel model)
        {
            await classTypeService.AddClassTypeAsync(model);
            // assume model.CTID is set by EF when saved
            return CreatedAtAction(nameof(GetById), new { id = model.CTID }, model);
        }

        // DELETE /api/classtype/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            await classTypeService.DeleteClassTypeAsync(id);
            return NoContent();
        }
    }
}

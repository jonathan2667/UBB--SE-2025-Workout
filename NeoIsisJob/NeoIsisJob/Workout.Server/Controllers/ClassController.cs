using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Workout.Core.IServices;
using Workout.Core.Models;

namespace Workout.Server.Controllers
{
    [ApiController]
    [Route("api/class")]
    public class ClassController : ControllerBase
    {
        private readonly IClassService classService;

        public ClassController(IClassService classService)
        {
            this.classService = classService;
        }

        // GET /api/class
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ClassModel>>> GetAll()
        {
            var items = await classService.GetAllClassesAsync();
            return Ok(items);
        }

        // GET /api/class/{id}
        [HttpGet("{id}")]
        public async Task<ActionResult<ClassModel>> GetById(int id)
        {
            var item = await classService.GetClassByIdAsync(id);
            if (item == null)
            {
                return NotFound();
            }
            return Ok(item);
        }

        // POST /api/class
        [HttpPost]
        public async Task<ActionResult<ClassModel>> Create([FromBody] ClassModel model)
        {
            await classService.AddClassAsync(model);
            // Assuming the repo/EF fills model.CID
            return CreatedAtAction(nameof(GetById), new { id = model.CID }, model);
        }

        // DELETE /api/class/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            await classService.DeleteClassAsync(id);
            return NoContent();
        }

        // POST /api/class/confirm
        [HttpPost("confirm")]
        public async Task<ActionResult<string>> ConfirmRegistration([FromBody] ConfirmRegistrationRequest req)
        {
            var result = await classService.ConfirmRegistrationAsync(req.UserId, req.ClassId, req.Date);
            return Ok(result);
        }
    }

    // You can define this in a shared DTOs folder instead of inside controller
    public class ConfirmRegistrationRequest
    {
        public int UserId { get; set; }
        public int ClassId { get; set; }
        public DateTime Date { get; set; }
    }
}



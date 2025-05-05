//using Microsoft.AspNetCore.Mvc;
//using Workout.Core.IServices;
//using Workout.Core.Models;


//namespace Workout.Server.Controllers
//{
//    [ApiController]
//    [Route("api/[controller]")]
//    public class ClassController : ControllerBase
//    {
//        private readonly IClassService _classService;

//        public ClassController(IClassService classService)
//        {
//            _classService = classService;
//        }

//        [HttpGet("api/class")]
//        public async Task<IActionResult> GetAllClasses()
//        {
//            try
//            {
//                var classes = await _classService.GetAllClassesAsync();
//                return Ok(classes);
//            }
//            catch (Exception ex)
//            {
//                return BadRequest($"Error fetching classes: {ex.Message}");
//            }
//        }
//        [HttpGet("api/class/{classId}")]
//        public async Task<IActionResult> GetClassById(int classId)
//        {
//            try
//            {
//                var classModel = await _classService.GetClassByIdAsync(classId);
//                return Ok(classModel);
//            }
//            catch (Exception ex)
//            {
//                return BadRequest($"Error fetching class: {ex.Message}");
//            }
//        }
//        [HttpPost("api/class")]
//        public async Task<IActionResult> AddClass([FromBody] ClassModel classModel)
//        {
//            try
//            {
//                await _classService.AddClassAsync(classModel);
//                return Ok();
//            }
//            catch (Exception ex)
//            {
//                return BadRequest($"Error adding class: {ex.Message}");
//            }
//        }
//        [HttpDelete("api/class/{classId}")]
//        public async Task<IActionResult> DeleteClass(int classId)
//        {
//            try
//            {
//                await _classService.DeleteClassAsync(classId);
//                return Ok();
//            }
//            catch (Exception ex)
//            {
//                return BadRequest($"Error deleting class: {ex.Message}");
//            }
//        }
//        [HttpPut("api/class/confirm/{userId}/{classId}/{date}")]
//        public async Task<IActionResult> ConfirmRegistration(int userId, int classId, DateTime date)
//        {
//            try
//            {
//                await _classService.ConfirmRegistrationAsync(userId, classId, date);
//                return Ok();
//            }
//            catch (Exception ex)
//            {
//                return BadRequest($"Error updating class: {ex.Message}");
//            }
//        }
//    }
//}

using Microsoft.AspNetCore.Mvc;
using Workout.Core.IServices;
using Workout.Core.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Workout.Server.Controllers
{
    [ApiController]
    [Route("api/class")]
    public class ClassController : ControllerBase
    {
        private readonly IClassService _classService;

        public ClassController(IClassService classService)
        {
            _classService = classService;
        }

        // GET /api/class
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ClassModel>>> GetAll()
        {
            var items = await _classService.GetAllClassesAsync();
            return Ok(items);
        }

        // GET /api/class/{id}
        [HttpGet("{id}")]
        public async Task<ActionResult<ClassModel>> GetById(int id)
        {
            var item = await _classService.GetClassByIdAsync(id);
            if (item == null) return NotFound();
            return Ok(item);
        }

        // POST /api/class
        [HttpPost]
        public async Task<ActionResult<ClassModel>> Create([FromBody] ClassModel model)
        {
            await _classService.AddClassAsync(model);
            // Assuming the repo/EF fills model.CID
            return CreatedAtAction(nameof(GetById), new { id = model.CID }, model);
        }

        // DELETE /api/class/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            await _classService.DeleteClassAsync(id);
            return NoContent();
        }

        // POST /api/class/confirm
        [HttpPost("confirm")]
        public async Task<ActionResult<string>> ConfirmRegistration([FromBody] ConfirmRegistrationRequest req)
        {
            var result = await _classService.ConfirmRegistrationAsync(req.UserId, req.ClassId, req.Date);
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



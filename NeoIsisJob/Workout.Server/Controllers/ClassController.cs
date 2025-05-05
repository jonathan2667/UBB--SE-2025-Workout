using Microsoft.AspNetCore.Mvc;
using Workout.Core.IServices;
using Workout.Core.Models;


namespace Workout.Server.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ClassController : ControllerBase
    {
        private readonly IClassService _classService;

        public ClassController(IClassService classService)
        {
            _classService = classService;
        }

        [HttpGet("api/class")]
        public async Task<IActionResult> GetAllClasses()
        {
            try
            {
                var classes = await _classService.GetAllClassesAsync();
                return Ok(classes);
            }
            catch (Exception ex)
            {
                return BadRequest($"Error fetching classes: {ex.Message}");
            }
        }
        [HttpGet("api/class/{classId}")]
        public async Task<IActionResult> GetClassById(int classId)
        {
            try
            {
                var classModel = await _classService.GetClassByIdAsync(classId);
                return Ok(classModel);
            }
            catch (Exception ex)
            {
                return BadRequest($"Error fetching class: {ex.Message}");
            }
        }
        [HttpPost("api/class")]
        public async Task<IActionResult> AddClass([FromBody] ClassModel classModel)
        {
            try
            {
                await _classService.AddClassAsync(classModel);
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest($"Error adding class: {ex.Message}");
            }
        }
        [HttpDelete("api/class/{classId}")]
        public async Task<IActionResult> DeleteClass(int classId)
        {
            try
            {
                await _classService.DeleteClassAsync(classId);
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest($"Error deleting class: {ex.Message}");
            }
        }
        [HttpPut("api/class/confirm/{userId}/{classId}/{date}")]
        public async Task<IActionResult> ConfirmRegistration(int userId, int classId, DateTime date)
        {
            try
            {
                await _classService.ConfirmRegistrationAsync(userId, classId, date);
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest($"Error updating class: {ex.Message}");
            }
        }
    }
}

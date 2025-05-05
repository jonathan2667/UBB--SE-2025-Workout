using Microsoft.AspNetCore.Mvc;
using Workout.Core.IServices;
using Workout.Core.Models;

namespace Workout.Server.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ClassTypeController : ControllerBase
    {
        private readonly IClassTypeService _classTypeService;
        public ClassTypeController(IClassTypeService classTypeService)
        {
            _classTypeService = classTypeService;
        }

        [HttpGet("api/classtype")]
        public async Task<IActionResult> GetAllClassTypes()
        {
            try
            {
                var classTypes = await _classTypeService.GetAllClassTypesAsync();
                return Ok(classTypes);
            }
            catch (Exception ex)
            {
                return BadRequest($"Error fetching class types: {ex.Message}");
            }
        }
        [HttpGet("api/classtype/{classTypeId}")]
        public async Task<IActionResult> GetClassTypeById(int classTypeId)
        {
            try
            {
                var classType = await _classTypeService.GetClassTypeByIdAsync(classTypeId);
                return Ok(classType);
            }
            catch (Exception ex)
            {
                return BadRequest($"Error fetching class type: {ex.Message}");
            }
        }

        [HttpPost("api/classtype")]
        public async Task<IActionResult> AddClassType([FromBody] ClassTypeModel classTypeModel)
        {
            try
            {
                await _classTypeService.AddClassTypeAsync(classTypeModel);
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest($"Error adding class type: {ex.Message}");
            }
        }

        [HttpDelete("api/classtype/{classTypeId}")]
        public async Task<IActionResult> DeleteClassType(int classTypeId)
        {
            try
            {
                await _classTypeService.DeleteClassTypeAsync(classTypeId);
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest($"Error deleting class type: {ex.Message}");
            }
        }
    }
}

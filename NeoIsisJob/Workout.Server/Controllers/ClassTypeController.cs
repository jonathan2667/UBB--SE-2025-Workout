//using Microsoft.AspNetCore.Mvc;
//using Workout.Core.IServices;
//using Workout.Core.Models;

//namespace Workout.Server.Controllers
//{
//    [ApiController]
//    [Route("api/[controller]")]
//    public class ClassTypeController : ControllerBase
//    {
//        private readonly IClassTypeService _classTypeService;
//        public ClassTypeController(IClassTypeService classTypeService)
//        {
//            _classTypeService = classTypeService;
//        }

//        [HttpGet("api/classtype")]
//        public async Task<IActionResult> GetAllClassTypes()
//        {
//            try
//            {
//                var classTypes = await _classTypeService.GetAllClassTypesAsync();
//                return Ok(classTypes);
//            }
//            catch (Exception ex)
//            {
//                return BadRequest($"Error fetching class types: {ex.Message}");
//            }
//        }
//        [HttpGet("api/classtype/{classTypeId}")]
//        public async Task<IActionResult> GetClassTypeById(int classTypeId)
//        {
//            try
//            {
//                var classType = await _classTypeService.GetClassTypeByIdAsync(classTypeId);
//                return Ok(classType);
//            }
//            catch (Exception ex)
//            {
//                return BadRequest($"Error fetching class type: {ex.Message}");
//            }
//        }

//        [HttpPost("api/classtype")]
//        public async Task<IActionResult> AddClassType([FromBody] ClassTypeModel classTypeModel)
//        {
//            try
//            {
//                await _classTypeService.AddClassTypeAsync(classTypeModel);
//                return Ok();
//            }
//            catch (Exception ex)
//            {
//                return BadRequest($"Error adding class type: {ex.Message}");
//            }
//        }

//        [HttpDelete("api/classtype/{classTypeId}")]
//        public async Task<IActionResult> DeleteClassType(int classTypeId)
//        {
//            try
//            {
//                await _classTypeService.DeleteClassTypeAsync(classTypeId);
//                return Ok();
//            }
//            catch (Exception ex)
//            {
//                return BadRequest($"Error deleting class type: {ex.Message}");
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
    [Route("api/classtype")]
    public class ClassTypeController : ControllerBase
    {
        private readonly IClassTypeService _classTypeService;

        public ClassTypeController(IClassTypeService classTypeService)
        {
            _classTypeService = classTypeService;
        }

        // GET /api/classtype
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ClassTypeModel>>> GetAll()
        {
            var items = await _classTypeService.GetAllClassTypesAsync();
            return Ok(items);
        }

        // GET /api/classtype/{id}
        [HttpGet("{id}")]
        public async Task<ActionResult<ClassTypeModel>> GetById(int id)
        {
            var item = await _classTypeService.GetClassTypeByIdAsync(id);
            if (item == null) return NotFound();
            return Ok(item);
        }

        // POST /api/classtype
        [HttpPost]
        public async Task<ActionResult<ClassTypeModel>> Create([FromBody] ClassTypeModel model)
        {
            await _classTypeService.AddClassTypeAsync(model);
            // assume model.CTID is set by EF when saved
            return CreatedAtAction(nameof(GetById), new { id = model.CTID }, model);
        }

        // DELETE /api/classtype/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            await _classTypeService.DeleteClassTypeAsync(id);
            return NoContent();
        }
    }
}

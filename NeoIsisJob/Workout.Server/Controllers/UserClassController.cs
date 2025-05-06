//using Microsoft.AspNetCore.Mvc;
//using Workout.Core.IServices;
//using Workout.Core.Models;

//namespace Workout.Server.Controllers
//{
//    [ApiController]
//    [Route("api/[controller]")]
//    public class UserClassController : ControllerBase
//    {
//        private readonly IUserClassService userClassService;
//        public UserClassController(IUserClassService userClassService)
//        {
//            this.userClassService = userClassService;
//        }
//        [HttpGet("api/userclass")]
//        public async Task<IActionResult> GetAllUserClasses(int userId)
//        {
//            try
//            {
//                var userClasses = await userClassService.GetAllUserClassesAsync();
//                return Ok(userClasses);
//            }
//            catch (Exception ex)
//            {
//                return BadRequest($"Error fetching user classes: {ex.Message}");
//            }
//        }
//        [HttpGet("api/userclass/{userId}/{classId}/{date}")]
//        public async Task<IActionResult> GetUserClassById(int userId, int classId, DateTime date)
//        {
//            try
//            {
//                var userClass = await userClassService.GetUserClassByIdAsync(userId, classId, date);
//                return Ok(userClass);
//            }
//            catch (Exception ex)
//            {
//                return BadRequest($"Error fetching user class: {ex.Message}");
//            }
//        }

//        [HttpPost("api/userclass")]
//        public async Task<IActionResult> AddUserClass([FromBody] UserClassModel userClass)
//        {
//            try
//            {
//                await userClassService.AddUserClassAsync(userClass);
//                return Ok();
//            }
//            catch (Exception ex)
//            {
//                return BadRequest($"Error adding user class: {ex.Message}");
//            }
//        }

//        [HttpDelete("api/userclass/{userId}/{classId}/{date}")]
//        public async Task<IActionResult> DeleteUserClass(int userId, int classId, DateTime date)
//        {
//            try
//            {
//                await userClassService.DeleteUserClassAsync(userId, classId, date);
//                return Ok();
//            }
//            catch (Exception ex)
//            {
//                return BadRequest($"Error deleting user class: {ex.Message}");
//            }
//        }
//    }
//}


// Workout.Server/Controllers/UserClassController.cs
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Workout.Core.IServices;
using Workout.Core.Models;

namespace Workout.Server.Controllers
{
    [ApiController]
    [Route("api/[controller]")]   // ⇒ /api/userclass
    public class UserClassController : ControllerBase
    {
        private readonly IUserClassService _userClassService;
        public UserClassController(IUserClassService userClassService)
            => _userClassService = userClassService;

        // GET /api/userclass
        [HttpGet]
        public async Task<ActionResult<IEnumerable<UserClassModel>>> GetAll()
        {
            var list = await _userClassService.GetAllUserClassesAsync();
            return Ok(list);
        }

        // GET /api/userclass/{userId}/{classId}/{date}
        [HttpGet("{userId}/{classId}/{date}")]
        public async Task<ActionResult<UserClassModel>> Get(
            int userId, int classId, DateTime date)
        {
            var uc = await _userClassService.GetUserClassByIdAsync(userId, classId, date);
            if (uc == null) return NotFound();
            return Ok(uc);
        }

        // POST /api/userclass
        [HttpPost]
        public async Task<ActionResult<UserClassModel>> Create([FromBody] UserClassModel model)
        {
            await _userClassService.AddUserClassAsync(model);
            return CreatedAtAction(
                nameof(Get),
                new { userId = model.UID, classId = model.CID, date = model.Date },
                model);
        }

        // DELETE /api/userclass/{userId}/{classId}/{date}
        [HttpDelete("{userId}/{classId}/{date}")]
        public async Task<IActionResult> Delete(
            int userId, int classId, DateTime date)
        {
            await _userClassService.DeleteUserClassAsync(userId, classId, date);
            return NoContent();
        }
    }
}

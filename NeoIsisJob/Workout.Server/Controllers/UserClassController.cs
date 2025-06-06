// Workout.Server/Controllers/UserClassController.cs
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Workout.Core.IServices;
using Workout.Core.Models;

namespace Workout.Server.Controllers
{
    [ApiController]
    [Route("api/[controller]")]// ⇒ /api/userclass
    public class UserClassController : ControllerBase
    {
        private readonly IUserClassService userClassService;
        public UserClassController(IUserClassService userClassService)
            => this.userClassService = userClassService;

        // GET /api/userclass
        [HttpGet]
        public async Task<ActionResult<IEnumerable<UserClassModel>>> GetAll()
        {
            var list = await userClassService.GetAllUserClassesAsync();
            return Ok(list);
        }

        // GET /api/userclass/{userId}/{classId}/{date}
        [HttpGet("{userId}/{classId}/{date}")]
        public async Task<ActionResult<UserClassModel>> Get(
            int userId, int classId, DateTime date)
        {
            var uc = await userClassService.GetUserClassByIdAsync(userId, classId, date);
            if (uc == null)
            {
                return NotFound();
            }
            return Ok(uc);
        }

        // POST /api/userclass
        [HttpPost]
        public async Task<ActionResult<UserClassModel>> Create([FromBody] UserClassModel model)
        {
            await userClassService.AddUserClassAsync(model);
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
            await userClassService.DeleteUserClassAsync(userId, classId, date);
            return NoContent();
        }
    }
}

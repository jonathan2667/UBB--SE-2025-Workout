// Workout.Server/Controllers/UserWorkoutController.cs
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Workout.Core.IServices;
using Workout.Core.Models;

namespace Workout.Server.Controllers
{
    [ApiController]
    [Route("api/[controller]")]// ⇒ /api/userworkout
    public class UserWorkoutController : ControllerBase
    {
        private readonly IUserWorkoutService userWorkoutService;
        public UserWorkoutController(IUserWorkoutService userWorkoutService)
            => this.userWorkoutService = userWorkoutService;

        // GET /api/userworkout/{userId}/{date}
        [HttpGet("{userId}/{date}")]
        public async Task<ActionResult<UserWorkoutModel>> Get(int userId, DateTime date)
        {
            var uw = await userWorkoutService.GetUserWorkoutForDateAsync(userId, date);
            if (uw == null)
            {
                return NotFound();
            }
            return Ok(uw);
        }

        // POST /api/userworkout
        [HttpPost]
        public async Task<ActionResult<UserWorkoutModel>> Create([FromBody] UserWorkoutModel model)
        {
            await userWorkoutService.AddUserWorkoutAsync(model);
            return CreatedAtAction(
                nameof(Get),
                new { userId = model.UID, date = model.Date },
                model);
        }

        // PUT /api/userworkout/{userId}/{workoutId}/{date}
        [HttpPut("{userId}/{workoutId}/{date}")]
        public async Task<IActionResult> Complete(
            int userId, int workoutId, DateTime date)
        {
            await userWorkoutService.CompleteUserWorkoutAsync(userId, workoutId, date);
            return NoContent();
        }

        // DELETE /api/userworkout/{userId}/{workoutId}/{date}
        [HttpDelete("{userId}/{workoutId}/{date}")]
        public async Task<IActionResult> Delete(
            int userId, int workoutId, DateTime date)
        {
            await userWorkoutService.DeleteUserWorkoutAsync(userId, workoutId, date);
            return NoContent();
        }
    }
}

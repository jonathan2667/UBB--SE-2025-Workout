//using Microsoft.AspNetCore.Mvc;
//using Workout.Core.IServices;
//using Workout.Core.Models;

//namespace Workout.Server.Controllers
//{
//    [ApiController]
//    [Route("api/[controller]")]
//    public class UserWorkoutController : ControllerBase
//    {
//        private readonly IUserWorkoutService userWorkoutService;

//        public UserWorkoutController(IUserWorkoutService userWorkoutService)
//        {
//            this.userWorkoutService = userWorkoutService;
//        }
//        [HttpGet("api/userworkout/{userId}/{date}")]
//        public async Task<IActionResult> GetUserWorkout(int userId, DateTime date)
//        {
//            try
//            {
//                var userWorkout = await userWorkoutService.GetUserWorkoutForDateAsync(userId, date);
//                return Ok(userWorkout);
//            }
//            catch (Exception ex)
//            {
//                return BadRequest($"Error fetching user workout: {ex.Message}");
//            }
//        }
//        [HttpPost("api/userworkout")]
//        public async Task<IActionResult> AddUserWorkout([FromBody] UserWorkoutModel userWorkout)
//        {
//            try
//            {
//                await userWorkoutService.AddUserWorkoutAsync(userWorkout);
//                return Ok();
//            }
//            catch (Exception ex)
//            {
//                return BadRequest($"Error adding user workout: {ex.Message}");
//            }
//        }

//        [HttpPut("api/userworkout/{userId}/{workoutId}/{date}")]
//        public async Task<IActionResult> CompleteUserWorkout(int userId, int workoutId, DateTime date)
//        {
//            try
//            {
//                await userWorkoutService.CompleteUserWorkoutAsync(userId, workoutId, date);
//                return Ok();
//            }
//            catch (Exception ex)
//            {
//                return BadRequest($"Error updating user workout: {ex.Message}");
//            }
//        }

//        [HttpDelete("api/userworkout/{userId}/{workoutId}/{date}")]
//        public async Task<IActionResult> DeleteUserWorkout(int userId, int workoutId, DateTime date)
//        {
//            try
//            {
//                await userWorkoutService.DeleteUserWorkoutAsync(userId, workoutId, date);
//                return Ok();
//            }
//            catch (Exception ex)
//            {
//                return BadRequest($"Error deleting user workout: {ex.Message}");
//            }
//        }

//    }
//}


// Workout.Server/Controllers/UserWorkoutController.cs
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Workout.Core.IServices;
using Workout.Core.Models;

namespace Workout.Server.Controllers
{
    [ApiController]
    [Route("api/[controller]")]   // ⇒ /api/userworkout
    public class UserWorkoutController : ControllerBase
    {
        private readonly IUserWorkoutService _userWorkoutService;
        public UserWorkoutController(IUserWorkoutService userWorkoutService)
            => _userWorkoutService = userWorkoutService;

        // GET /api/userworkout/{userId}/{date}
        [HttpGet("{userId}/{date}")]
        public async Task<ActionResult<UserWorkoutModel>> Get(int userId, DateTime date)
        {
            var uw = await _userWorkoutService.GetUserWorkoutForDateAsync(userId, date);
            if (uw == null) return NotFound();
            return Ok(uw);
        }

        // POST /api/userworkout
        [HttpPost]
        public async Task<ActionResult<UserWorkoutModel>> Create([FromBody] UserWorkoutModel model)
        {
            await _userWorkoutService.AddUserWorkoutAsync(model);
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
            await _userWorkoutService.CompleteUserWorkoutAsync(userId, workoutId, date);
            return NoContent();
        }

        // DELETE /api/userworkout/{userId}/{workoutId}/{date}
        [HttpDelete("{userId}/{workoutId}/{date}")]
        public async Task<IActionResult> Delete(
            int userId, int workoutId, DateTime date)
        {
            await _userWorkoutService.DeleteUserWorkoutAsync(userId, workoutId, date);
            return NoContent();
        }
    }
}

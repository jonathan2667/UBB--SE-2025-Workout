//using Microsoft.AspNetCore.Mvc;
//using Workout.Core.Models;
//using Workout.Core.IServices;
//using System.Collections.ObjectModel;

//namespace Workout.Server.Controllers
//{
//    [ApiController]
//    [Route("api/[controller]")]
//    public class CalendarController : ControllerBase
//    {
//        private readonly ICalendarService calendarService;
//        public CalendarController(ICalendarService calendarService)
//        {
//            this.calendarService = calendarService;
//        }

//        [HttpGet("api/calendar/{userId}/{year}/{month}")]
//        public async Task<IActionResult> GetCalendarDays(int userId, int year, int month)
//        {
//            try
//            {
//                var date = new DateTime(year, month, 1);
//                var calendarDays = await calendarService.GetCalendarDaysForMonthAsync(userId, date);
//                return Ok(calendarDays);
//            }
//            catch (Exception ex)
//            {
//                return BadRequest($"Error fetching calendar days: {ex.Message}");
//            }
//        }

//        [HttpGet("api/calendar/{userId}/{year}/{month}/{day}")]
//        public async Task<IActionResult> GetUserWorkout(int userId, int year, int month, int day)
//        {
//            try
//            {
//                var date = new DateTime(year, month, day);
//                var userWorkout = await calendarService.GetUserWorkoutAsync(userId, date);
//                return Ok(userWorkout);
//            }
//            catch (Exception ex)
//            {
//                return BadRequest($"Error fetching user workout: {ex.Message}");
//            }
//        }

//        [HttpPost("api/calendar")]
//        public async Task<IActionResult> AddUserWorkout([FromBody] UserWorkoutModel userWorkout)
//        {
//            try
//            {
//                await calendarService.AddUserWorkoutAsync(userWorkout);
//                return Ok();
//            }
//            catch (Exception ex)
//            {
//                return BadRequest($"Error adding user workout: {ex.Message}");
//            }
//        }

//        [HttpPut("api/calendar")]
//        public async Task<IActionResult> UpdateUserWorkout([FromBody] UserWorkoutModel userWorkout)
//        {
//            try
//            {
//                await calendarService.UpdateUserWorkoutAsync(userWorkout);
//                return Ok();
//            }
//            catch (Exception ex)
//            {
//                return BadRequest($"Error updating user workout: {ex.Message}");
//            }
//        }

//        [HttpDelete("api/calendar/{userId}/{workoutId}/{year}/{month}/{day}")]
//        public async Task<IActionResult> DeleteUserWorkout(int userId, int workoutId, int year, int month, int day)
//        {
//            try
//            {
//                var date = new DateTime(year, month, day);
//                await calendarService.DeleteUserWorkoutAsync(userId, workoutId, date);
//                return Ok();
//            }
//            catch (Exception ex)
//            {
//                return BadRequest($"Error deleting user workout: {ex.Message}");
//            }
//        }

//        [HttpGet("api/calendar/{userId}/{year}/{month}/{day}/class")]
//        public async Task<IActionResult> GetUserClass(int userId, int year, int month, int day)
//        {
//            try
//            {
//                var date = new DateTime(year, month, day);
//                var userClass = await calendarService.GetUserClassAsync(userId, date);
//                return Ok(userClass);
//            }
//            catch (Exception ex)
//            {
//                return BadRequest($"Error fetching user class: {ex.Message}");
//            }
//        }
//        [HttpGet("api/calendar/workouts")]
//        public async Task<IActionResult> GetWorkouts()
//        {
//            try
//            {
//                var workouts = await calendarService.GetWorkoutsAsync();
//                return Ok(workouts);
//            }
//            catch (Exception ex)
//            {
//                return BadRequest($"Error fetching workouts: {ex.Message}");
//            }
//        }
//        [HttpGet("api/calendar/{userId}/{year}/{month}/{day}/remove")]
//        public async Task<IActionResult> RemoveWorkout(int userId, int year, int month, int day)
//        {
//            try
//            {
//                var date = new DateTime(year, month, day);
//                var calendarDay = new CalendarDayModel { Date = date };
//                await calendarService.RemoveWorkoutAsync(userId, calendarDay);
//                return Ok();
//            }
//            catch (Exception ex)
//            {
//                return BadRequest($"Error removing workout: {ex.Message}");
//            }
//        }
//        [HttpGet("api/calendar/{userId}/{year}/{month}/{day}/change")]
//        public async Task<IActionResult> ChangeWorkout(int userId, int year, int month, int day)
//        {
//            try
//            {
//                var date = new DateTime(year, month, day);
//                var calendarDay = new CalendarDayModel { Date = date };
//                await calendarService.ChangeWorkoutAsync(userId, calendarDay);
//                return Ok();
//            }
//            catch (Exception ex)
//            {
//                return BadRequest($"Error changing workout: {ex.Message}");
//            }
//        }
//        [HttpGet("api/calendar/workoutdayscount")]
//        public IActionResult GetWorkoutDaysCountText([FromBody] ObservableCollection<CalendarDayModel> calendarDays)
//        {
//            try
//            {
//                var text = calendarService.GetWorkoutDaysCountText(calendarDays);
//                return Ok(text);
//            }
//            catch (Exception ex)
//            {
//                return BadRequest($"Error getting workout days count text: {ex.Message}");
//            }
//        }

//        [HttpGet("api/calendar/dayscount")]
//        public IActionResult GetDaysCountText([FromBody] ObservableCollection<CalendarDayModel> calendarDays)
//        {
//            try
//            {
//                var text = calendarService.GetDaysCountText(calendarDays);
//                return Ok(text);
//            }
//            catch (Exception ex)
//            {
//                return BadRequest($"Error getting days count text: {ex.Message}");
//            }
//        }


//    }
//}
// Workout.Server/Controllers/CalendarController.cs


//using Microsoft.AspNetCore.Mvc;
//using System;
//using System.Collections.Generic;
//using System.Collections.ObjectModel;
//using System.Threading.Tasks;
//using Workout.Core.IServices;
//using Workout.Core.Models;

//namespace Workout.Server.Controllers
//{
//    [ApiController]
//    [Route("api/[controller]")]    // ⇒ /api/calendar
//    public class CalendarController : ControllerBase
//    {
//        private readonly ICalendarService _calendarService;
//        public CalendarController(ICalendarService calendarService)
//            => _calendarService = calendarService;

//        // GET /api/calendar/{userId}/{year}/{month}
//        [HttpGet("{userId}/{year}/{month}")]
//        public async Task<IActionResult> GetCalendarDays(int userId, int year, int month)
//        {
//            var date = new DateTime(year, month, 1);
//            var days = await _calendarService.GetCalendarDaysForMonthAsync(userId, date);
//            return Ok(days);
//        }

//        // GET /api/calendar/{userId}/{year}/{month}/{day}
//        [HttpGet("{userId}/{year}/{month}/{day}")]
//        public async Task<IActionResult> GetUserWorkout(int userId, int year, int month, int day)
//        {
//            var date = new DateTime(year, month, day);
//            var workout = await _calendarService.GetUserWorkoutAsync(userId, date);
//            return Ok(workout);
//        }

//        // GET /api/calendar/workouts
//        [HttpGet("workouts")]
//        public async Task<IActionResult> GetWorkouts()
//            => Ok(await _calendarService.GetWorkoutsAsync());

//        // POST /api/calendar/userworkout
//        [HttpPost("userworkout")]
//        public async Task<IActionResult> AddUserWorkout([FromBody] UserWorkoutModel uw)
//        {
//            await _calendarService.AddUserWorkoutAsync(uw);
//            return Ok();
//        }

//        // PUT /api/calendar/userworkout
//        [HttpPut("userworkout")]
//        public async Task<IActionResult> UpdateUserWorkout([FromBody] UserWorkoutModel uw)
//        {
//            await _calendarService.UpdateUserWorkoutAsync(uw);
//            return Ok();
//        }

//        // DELETE /api/calendar/userworkout/{userId}/{workoutId}/{year}/{month}/{day}
//        [HttpDelete("userworkout/{userId}/{workoutId}/{year}/{month}/{day}")]
//        public async Task<IActionResult> DeleteUserWorkout(int userId, int workoutId, int year, int month, int day)
//        {
//            var date = new DateTime(year, month, day);
//            await _calendarService.DeleteUserWorkoutAsync(userId, workoutId, date);
//            return Ok();
//        }

//        // PUT /api/calendar/workout/{userId}/{year}/{month}/{day}
//        [HttpPut("workout/{userId}/{year}/{month}/{day}")]
//        public async Task<IActionResult> ChangeWorkout(
//            int userId, int year, int month, int day,
//            [FromBody] CalendarDayModel payload)
//        {
//            // your service only needs (userId, CalendarDayModel)
//            await _calendarService.ChangeWorkoutAsync(userId, payload);
//            return Ok();
//        }

//        // POST /api/calendar/workoutdayscount
//        [HttpPost("workoutdayscount")]
//        public IActionResult WorkoutDaysCount([FromBody] List<CalendarDayModel> days)
//        {
//            // wrap into ObservableCollection to match service signature
//            var text = _calendarService.GetWorkoutDaysCountText(
//                new ObservableCollection<CalendarDayModel>(days));
//            return Ok(text);
//        }

//        // POST /api/calendar/dayscount
//        [HttpPost("dayscount")]
//        public IActionResult DaysCount([FromBody] List<CalendarDayModel> days)
//        {
//            var text = _calendarService.GetDaysCountText(
//                new ObservableCollection<CalendarDayModel>(days));
//            return Ok(text);
//        }
//    }
//}



// Workout.Server/Controllers/CalendarController.cs
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using Workout.Core.IServices;
using Workout.Core.Models;

namespace Workout.Server.Controllers
{
    [ApiController]
    [Route("api/calendar")]
    public class CalendarController : ControllerBase
    {
        private readonly ICalendarService _calendarService;

        public CalendarController(ICalendarService calendarService)
            => _calendarService = calendarService;

        // GET /api/calendar/{userId}/{year}/{month}
        [HttpGet("{userId}/{year}/{month}")]
        public async Task<ActionResult<IEnumerable<CalendarDayModel>>> GetCalendarDays(int userId, int year, int month)
        {
            var date = new DateTime(year, month, 1);
            var days = await _calendarService.GetCalendarDaysForMonthAsync(userId, date);
            return Ok(days);
        }

        // GET /api/calendar/{userId}/{year}/{month}/{day}
        [HttpGet("{userId}/{year}/{month}/{day}")]
        public async Task<ActionResult<UserWorkoutModel>> GetUserWorkout(int userId, int year, int month, int day)
        {
            var date = new DateTime(year, month, day);
            var workout = await _calendarService.GetUserWorkoutAsync(userId, date);
            return Ok(workout);
        }

        // GET /api/calendar/workouts
        [HttpGet("workouts")]
        public async Task<ActionResult<IEnumerable<WorkoutModel>>> GetWorkouts()
            => Ok(await _calendarService.GetWorkoutsAsync());

        // POST /api/calendar/userworkout
        [HttpPost("userworkout")]
        public async Task<IActionResult> AddUserWorkout([FromBody] UserWorkoutModel uw)
        {
            await _calendarService.AddUserWorkoutAsync(uw);
            return Ok();
        }

        // PUT /api/calendar/userworkout
        [HttpPut("userworkout")]
        public async Task<IActionResult> UpdateUserWorkout([FromBody] UserWorkoutModel uw)
        {
            await _calendarService.UpdateUserWorkoutAsync(uw);
            return Ok();
        }

        // DELETE /api/calendar/userworkout/{userId}/{workoutId}/{year}/{month}/{day}
        [HttpDelete("userworkout/{userId}/{workoutId}/{year}/{month}/{day}")]
        public async Task<IActionResult> DeleteUserWorkout(int userId, int workoutId, int year, int month, int day)
        {
            var date = new DateTime(year, month, day);
            await _calendarService.DeleteUserWorkoutAsync(userId, workoutId, date);
            return Ok();
        }

        // PUT /api/calendar/workout/{userId}/{year}/{month}/{day}
        [HttpPut("workout/{userId}/{year}/{month}/{day}")]
        public async Task<IActionResult> ChangeWorkout(
            int userId, int year, int month, int day,
            [FromBody] CalendarDayModel payload)
        {
            await _calendarService.ChangeWorkoutAsync(userId, payload);
            return Ok();
        }

        // POST /api/calendar/workoutdayscount
        [HttpPost("workoutdayscount")]
        public ActionResult<string> WorkoutDaysCount([FromBody] List<CalendarDayModel> days)
        {
            var text = _calendarService.GetWorkoutDaysCountText(
                new ObservableCollection<CalendarDayModel>(days));
            return Ok(text);
        }

        // POST /api/calendar/dayscount
        [HttpPost("dayscount")]
        public ActionResult<string> DaysCount([FromBody] List<CalendarDayModel> days)
        {
            var text = _calendarService.GetDaysCountText(
                new ObservableCollection<CalendarDayModel>(days));
            return Ok(text);
        }
    }
}

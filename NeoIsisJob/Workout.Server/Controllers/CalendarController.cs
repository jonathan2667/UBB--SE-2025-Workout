// Workout.Server/Controllers/CalendarController.cs
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Workout.Core.IServices;
using Workout.Core.Models;

namespace Workout.Server.Controllers
{
    [ApiController]
    [Route("api/calendar")]
    public class CalendarController : ControllerBase
    {
        private readonly ICalendarService calendarService;

        public CalendarController(ICalendarService calendarService)
            => this.calendarService = calendarService;

        // GET /api/calendar/{userId}/{year}/{month}
        [HttpGet("{userId}/{year}/{month}")]
        public async Task<ActionResult<IEnumerable<CalendarDayModel>>> GetCalendarDays(int userId, int year, int month)
        {
            var date = new DateTime(year, month, 1);
            var days = await calendarService.GetCalendarDaysForMonthAsync(userId, date);
            return Ok(days);
        }

        // GET /api/calendar/{userId}/{year}/{month}/{day}
        [HttpGet("{userId}/{year}/{month}/{day}")]
        public async Task<ActionResult<UserWorkoutModel>> GetUserWorkout(int userId, int year, int month, int day)
        {
            var date = new DateTime(year, month, day);
            var workout = await calendarService.GetUserWorkoutAsync(userId, date);
            return Ok(workout);
        }

        // GET /api/calendar/workouts
        [HttpGet("workouts")]
        public async Task<ActionResult<IEnumerable<WorkoutModel>>> GetWorkouts()
            => Ok(await calendarService.GetWorkoutsAsync());

        // POST /api/calendar/userworkout
        [HttpPost("userworkout")]
        public async Task<IActionResult> AddUserWorkout([FromBody] UserWorkoutModel uw)
        {
            await calendarService.AddUserWorkoutAsync(uw);
            return Ok();
        }

        // PUT /api/calendar/userworkout
        [HttpPut("userworkout")]
        public async Task<IActionResult> UpdateUserWorkout([FromBody] UserWorkoutModel uw)
        {
            await calendarService.UpdateUserWorkoutAsync(uw);
            return Ok();
        }

        // DELETE /api/calendar/userworkout/{userId}/{workoutId}/{year}/{month}/{day}
        [HttpDelete("userworkout/{userId}/{workoutId}/{year}/{month}/{day}")]
        public async Task<IActionResult> DeleteUserWorkout(int userId, int workoutId, int year, int month, int day)
        {
            var date = new DateTime(year, month, day);
            await calendarService.DeleteUserWorkoutAsync(userId, workoutId, date);
            return Ok();
        }

        // PUT /api/calendar/workout/{userId}/{year}/{month}/{day}
        [HttpPut("workout/{userId}/{year}/{month}/{day}")]
        public async Task<IActionResult> ChangeWorkout(
            int userId, int year, int month, int day,
            [FromBody] CalendarDayModel payload)
        {
            await calendarService.ChangeWorkoutAsync(userId, payload);
            return Ok();
        }

        // POST /api/calendar/workoutdayscount
        [HttpPost("workoutdayscount")]
        public ActionResult<string> WorkoutDaysCount([FromBody] List<CalendarDayModel> days)
        {
            var text = calendarService.GetWorkoutDaysCountText(
                new ObservableCollection<CalendarDayModel>(days));
            return Ok(text);
        }

        // POST /api/calendar/dayscount
        [HttpPost("dayscount")]
        public ActionResult<string> DaysCount([FromBody] List<CalendarDayModel> days)
        {
            var text = calendarService.GetDaysCountText(
                new ObservableCollection<CalendarDayModel>(days));
            return Ok(text);
        }
    }
}

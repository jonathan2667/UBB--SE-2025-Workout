using Microsoft.AspNetCore.Mvc;
using Workout.Core.Models;
using Workout.Core.IServices;
using System.Collections.ObjectModel;

namespace Workout.Server.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CalendarController : ControllerBase
    {
        private readonly ICalendarService calendarService;
        public CalendarController(ICalendarService calendarService)
        {
            this.calendarService = calendarService;
        }

        [HttpGet("api/calendar/{userId}/{year}/{month}")]
        public async Task<IActionResult> GetCalendarDays(int userId, int year, int month)
        {
            try
            {
                var date = new DateTime(year, month, 1);
                var calendarDays = await calendarService.GetCalendarDaysForMonthAsync(userId, date);
                return Ok(calendarDays);
            }
            catch (Exception ex)
            {
                return BadRequest($"Error fetching calendar days: {ex.Message}");
            }
        }

        [HttpGet("api/calendar/{userId}/{year}/{month}/{day}")]
        public async Task<IActionResult> GetUserWorkout(int userId, int year, int month, int day)
        {
            try
            {
                var date = new DateTime(year, month, day);
                var userWorkout = await calendarService.GetUserWorkoutAsync(userId, date);
                return Ok(userWorkout);
            }
            catch (Exception ex)
            {
                return BadRequest($"Error fetching user workout: {ex.Message}");
            }
        }

        [HttpPost("api/calendar")]
        public async Task<IActionResult> AddUserWorkout([FromBody] UserWorkoutModel userWorkout)
        {
            try
            {
                await calendarService.AddUserWorkoutAsync(userWorkout);
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest($"Error adding user workout: {ex.Message}");
            }
        }

        [HttpPut("api/calendar")]
        public async Task<IActionResult> UpdateUserWorkout([FromBody] UserWorkoutModel userWorkout)
        {
            try
            {
                await calendarService.UpdateUserWorkoutAsync(userWorkout);
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest($"Error updating user workout: {ex.Message}");
            }
        }

        [HttpDelete("api/calendar/{userId}/{workoutId}/{year}/{month}/{day}")]
        public async Task<IActionResult> DeleteUserWorkout(int userId, int workoutId, int year, int month, int day)
        {
            try
            {
                var date = new DateTime(year, month, day);
                await calendarService.DeleteUserWorkoutAsync(userId, workoutId, date);
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest($"Error deleting user workout: {ex.Message}");
            }
        }

        [HttpGet("api/calendar/{userId}/{year}/{month}/{day}/class")]
        public async Task<IActionResult> GetUserClass(int userId, int year, int month, int day)
        {
            try
            {
                var date = new DateTime(year, month, day);
                var userClass = await calendarService.GetUserClassAsync(userId, date);
                return Ok(userClass);
            }
            catch (Exception ex)
            {
                return BadRequest($"Error fetching user class: {ex.Message}");
            }
        }
        [HttpGet("api/calendar/workouts")]
        public async Task<IActionResult> GetWorkouts()
        {
            try
            {
                var workouts = await calendarService.GetWorkoutsAsync();
                return Ok(workouts);
            }
            catch (Exception ex)
            {
                return BadRequest($"Error fetching workouts: {ex.Message}");
            }
        }
        [HttpGet("api/calendar/{userId}/{year}/{month}/{day}/remove")]
        public async Task<IActionResult> RemoveWorkout(int userId, int year, int month, int day)
        {
            try
            {
                var date = new DateTime(year, month, day);
                var calendarDay = new CalendarDayModel { Date = date };
                await calendarService.RemoveWorkoutAsync(userId, calendarDay);
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest($"Error removing workout: {ex.Message}");
            }
        }
        [HttpGet("api/calendar/{userId}/{year}/{month}/{day}/change")]
        public async Task<IActionResult> ChangeWorkout(int userId, int year, int month, int day)
        {
            try
            {
                var date = new DateTime(year, month, day);
                var calendarDay = new CalendarDayModel { Date = date };
                await calendarService.ChangeWorkoutAsync(userId, calendarDay);
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest($"Error changing workout: {ex.Message}");
            }
        }
        [HttpGet("api/calendar/workoutdayscount")]
        public IActionResult GetWorkoutDaysCountText([FromBody] ObservableCollection<CalendarDayModel> calendarDays)
        {
            try
            {
                var text = calendarService.GetWorkoutDaysCountText(calendarDays);
                return Ok(text);
            }
            catch (Exception ex)
            {
                return BadRequest($"Error getting workout days count text: {ex.Message}");
            }
        }

        [HttpGet("api/calendar/dayscount")]
        public IActionResult GetDaysCountText([FromBody] ObservableCollection<CalendarDayModel> calendarDays)
        {
            try
            {
                var text = calendarService.GetDaysCountText(calendarDays);
                return Ok(text);
            }
            catch (Exception ex)
            {
                return BadRequest($"Error getting days count text: {ex.Message}");
            }
        }


    }
}

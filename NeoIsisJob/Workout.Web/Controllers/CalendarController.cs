using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Workout.Core.IServices;
using Workout.Core.Models;
using System.Security.Claims;
using Microsoft.Extensions.Logging;
using System.Linq;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Http;
using Workout.Web.Filters;

namespace Workout.Web.Controllers
{
    public class CalendarController : Controller
    {
        private readonly ICalendarService _calendarService;
        private readonly ILogger<CalendarController> _logger;

        public CalendarController(ICalendarService calendarService, ILogger<CalendarController> logger)
        {
            _calendarService = calendarService;
            _logger = logger;
        }

        private int GetCurrentUserId()
        {
            var userIdString = HttpContext.Session.GetString("UserId");
            if (string.IsNullOrEmpty(userIdString) || !int.TryParse(userIdString, out int userId))
            {
                return 1; // Default to user ID 1 if not logged in (for backward compatibility)
            }
            return userId;
        }

        // GET: Calendar
        [AuthorizeUser]
        public async Task<IActionResult> Index(int? year, int? month)
        {
            try
            {
                DateTime currentDate;
                if (year.HasValue && month.HasValue)
                {
                    currentDate = new DateTime(year.Value, month.Value, 1);
                }
                else
                {
                    currentDate = DateTime.Now;
                }

                var userId = GetCurrentUserId();
                var days = await _calendarService.GetCalendarDaysForMonthAsync(userId, currentDate);
                
                // Assign grid positions for days
                var firstOfMonth = new DateTime(currentDate.Year, currentDate.Month, 1);
                int firstColumn = (int)firstOfMonth.DayOfWeek;   // Sunday=0, Monday=1, … Saturday=6

                for (int i = 0; i < days.Count; i++)
                {
                    var day = days[i];
                    int slot = firstColumn + i;            // zero-based slot in the calendar
                    day.GridRow = slot / 7;                // integer division → which row (0..5)
                    day.GridColumn = slot % 7;             // mod 7 → which column (0..6)
                }

                return View(days);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error fetching calendar days: {ex.Message}");
                ViewBag.ErrorMessage = "An error occurred while loading the calendar.";
                return View(new List<CalendarDayModel>());
            }
        }

        // GET: Calendar/Details/5/2024/3/15
        [AuthorizeUser]
        public async Task<IActionResult> Details(int year, int month, int day)
        {
            try
            {
                var userId = GetCurrentUserId();
                var date = new DateTime(year, month, day);
                var workout = await _calendarService.GetUserWorkoutAsync(userId, date);
                
                if (workout == null)
                {
                    _logger.LogWarning($"Workout not found for user {userId} on {date:yyyy-MM-dd}");
                    return NotFound();
                }

                return View(workout);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error fetching workout details: {ex.Message}");
                return NotFound();
            }
        }

        // GET: Calendar/Create
        [AuthorizeUser]
        public async Task<IActionResult> Create(string date, int workoutId = 0)
        {
            try
            {
                var workouts = await _calendarService.GetWorkoutsAsync();
                ViewBag.Workouts = new Microsoft.AspNetCore.Mvc.Rendering.SelectList(
                    workouts, "WID", "Name", workoutId);
                
                var userId = GetCurrentUserId();
                var userWorkout = new UserWorkoutModel
                {
                    UID = userId,
                    WID = workoutId,
                    Completed = false
                };

                if (!string.IsNullOrEmpty(date) && DateTime.TryParse(date, out DateTime parsedDate))
                {
                    userWorkout.Date = parsedDate;
                }
                else
                {
                    userWorkout.Date = DateTime.Today;
                }

                return View(userWorkout);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error loading workouts for create: {ex.Message}");
                ViewBag.ErrorMessage = "An error occurred while loading workouts.";
                return View(new UserWorkoutModel { Date = DateTime.Today, UID = GetCurrentUserId() });
            }
        }

        // POST: Calendar/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        [AuthorizeUser]
        public async Task<IActionResult> Create([Bind("WID,Date")] UserWorkoutModel userWorkout)
        {
            // Set the user ID from session
            userWorkout.UID = GetCurrentUserId();
            
            // Validate that the date is not in the past
            if (userWorkout.Date.Date < DateTime.Today)
            {
                ModelState.AddModelError("Date", "Cannot add workouts to past dates");
                _logger.LogWarning($"Attempt to add workout for past date: {userWorkout.Date:yyyy-MM-dd}");
            }
            
            if (ModelState.IsValid)
            {
                try
                {
                    await _calendarService.AddUserWorkoutAsync(userWorkout);
                    return RedirectToAction(nameof(Index));
                }
                catch (Exception ex)
                {
                    _logger.LogError($"Error creating workout: {ex.Message}");
                    ViewBag.ErrorMessage = "An error occurred while creating the workout.";
                }
            }
            else
            {
                foreach (var state in ModelState)
                {
                    if (state.Value.Errors.Count > 0)
                    {
                        _logger.LogError($"Validation error for {state.Key}: {string.Join(", ", state.Value.Errors.Select(e => e.ErrorMessage))}");
                    }
                }
            }
            
            try
            {
                var workouts = await _calendarService.GetWorkoutsAsync();
                ViewBag.Workouts = new SelectList(workouts, "WID", "Name", userWorkout.WID);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error loading workouts for redisplay: {ex.Message}");
            }
            return View(userWorkout);
        }

        // GET: Calendar/Edit/5/2024/3/15
        [AuthorizeUser]
        public async Task<IActionResult> Edit(int year, int month, int day)
        {
            try
            {
                var userId = GetCurrentUserId();
                var date = new DateTime(year, month, day);
                var workout = await _calendarService.GetUserWorkoutAsync(userId, date);
                
                if (workout == null)
                {
                    _logger.LogWarning($"Workout not found for user {userId} on {date:yyyy-MM-dd}");
                    return NotFound();
                }
                
                var workouts = await _calendarService.GetWorkoutsAsync();
                ViewBag.Workouts = new Microsoft.AspNetCore.Mvc.Rendering.SelectList(
                    workouts, "WID", "Name", workout.WID);
                    
                return View(workout);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error loading workout for edit: {ex.Message}");
                return NotFound();
            }
        }

        // POST: Calendar/Edit
        [HttpPost]
        [ValidateAntiForgeryToken]
        [AuthorizeUser]
        public async Task<IActionResult> Edit([Bind("WID,Date")] UserWorkoutModel userWorkout)
        {
            // Set the user ID from session
            userWorkout.UID = GetCurrentUserId();
            
            // Validate that the date is not in the past
            if (userWorkout.Date.Date < DateTime.Today)
            {
                ModelState.AddModelError("Date", "Cannot modify workouts for past dates");
                _logger.LogWarning($"Attempt to modify workout for past date: {userWorkout.Date:yyyy-MM-dd}");
            }
            
            if (ModelState.IsValid)
            {
                try
                {
                    await _calendarService.UpdateUserWorkoutAsync(userWorkout);
                    return RedirectToAction(nameof(Index));
                }
                catch (Exception ex)
                {
                    _logger.LogError($"Error updating workout: {ex.Message}");
                    ViewBag.ErrorMessage = "An error occurred while updating the workout.";
                }
            }
            else
            {
                foreach (var state in ModelState)
                {
                    if (state.Value.Errors.Count > 0)
                    {
                        _logger.LogError($"Validation error for {state.Key}: {string.Join(", ", state.Value.Errors.Select(e => e.ErrorMessage))}");
                    }
                }
            }
            
            try
            {
                var workouts = await _calendarService.GetWorkoutsAsync();
                ViewBag.Workouts = new SelectList(workouts, "WID", "Name", userWorkout.WID);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error loading workouts for redisplay: {ex.Message}");
            }
            return View(userWorkout);
        }

        // GET: Calendar/Delete/2024/3/15
        [AuthorizeUser]
        public async Task<IActionResult> Delete(int year, int month, int day)
        {
            try
            {
                var userId = GetCurrentUserId();
                var date = new DateTime(year, month, day);
                var workout = await _calendarService.GetUserWorkoutAsync(userId, date);
                
                if (workout == null)
                {
                    _logger.LogWarning($"Workout not found for user {userId} on {date:yyyy-MM-dd}");
                    return NotFound();
                }
                    
                return View(workout);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error loading workout for delete: {ex.Message}");
                return NotFound();
            }
        }

        // POST: Calendar/Delete/2024/3/15
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [AuthorizeUser]
        public async Task<IActionResult> DeleteConfirmed(int workoutId, int year, int month, int day)
        {
            try
            {
                var userId = GetCurrentUserId();
                var date = new DateTime(year, month, day);
                await _calendarService.DeleteUserWorkoutAsync(userId, workoutId, date);
                
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error deleting workout: {ex.Message}");
                return RedirectToAction(nameof(Index));
            }
        }

        // Direct AJAX delete endpoint that doesn't require antiforgery token
        [HttpPost]
        [Route("Calendar/DirectDelete/{userId}/{year}/{month}/{day}")]
        public async Task<IActionResult> DirectDelete(int userId, int year, int month, int day)
        {
            try
            {
                var date = new DateTime(year, month, day);
                
                // Validate that the date is not in the past
                if (date.Date < DateTime.Today)
                {
                    _logger.LogWarning($"Attempt to delete workout for past date: {date:yyyy-MM-dd}");
                    return Json(new { success = false, message = "Cannot delete workouts for past dates." });
                }
                
                // Get the workout details
                var userWorkout = await _calendarService.GetUserWorkoutAsync(userId, date);
                if (userWorkout == null)
                {
                    _logger.LogWarning($"No workout found to delete for user {userId} on {date:yyyy-MM-dd}");
                    return Json(new { success = false, message = "No workout found for this date." });
                }
                
                // Delete the workout
                await _calendarService.DeleteUserWorkoutAsync(userId, userWorkout.WID, date);
                
                _logger.LogInformation($"Workout for user {userId} on {date:yyyy-MM-dd} deleted successfully via DirectDelete");
                return Json(new { success = true, message = "Workout deleted successfully." });
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error in DirectDelete: {ex.Message}");
                return Json(new { success = false, message = "An error occurred while deleting the workout." });
            }
        }

        // Direct AJAX update endpoint that doesn't require antiforgery token
        [HttpPost]
        [Route("Calendar/DirectUpdate/{userId}/{year}/{month}/{day}/{workoutId}")]
        public async Task<IActionResult> DirectUpdate(int userId, int year, int month, int day, int workoutId)
        {
            try
            {
                var date = new DateTime(year, month, day);
                
                // Validate that the date is not in the past
                if (date.Date < DateTime.Today)
                {
                    _logger.LogWarning($"Attempt to update workout for past date: {date:yyyy-MM-dd}");
                    return Json(new { success = false, message = "Cannot modify workouts for past dates." });
                }
                
                // Validate that the workoutId is valid
                if (workoutId <= 0)
                {
                    _logger.LogWarning($"Invalid workoutId {workoutId} for date: {date:yyyy-MM-dd}");
                    return Json(new { success = false, message = "Invalid workout selected." });
                }
                
                // Check if workoutId exists
                var workouts = await _calendarService.GetWorkoutsAsync();
                var selectedWorkout = workouts.FirstOrDefault(w => w.WID == workoutId);
                if (selectedWorkout == null)
                {
                    _logger.LogWarning($"Workout with ID {workoutId} not found");
                    return Json(new { success = false, message = "Selected workout not found." });
                }
                
                // Get the existing workout if any
                var existingWorkout = await _calendarService.GetUserWorkoutAsync(userId, date);
                
                // Create or update the workout
                var userWorkout = new UserWorkoutModel
                {
                    UID = userId,
                    WID = workoutId,
                    Date = date,
                    // If there's an existing workout, preserve its completion status
                    Completed = existingWorkout?.Completed ?? false
                };
                
                if (existingWorkout != null)
                {
                    // Update existing workout
                    await _calendarService.UpdateUserWorkoutAsync(userWorkout);
                    _logger.LogInformation($"Workout updated for user {userId} on {date:yyyy-MM-dd} to workout {workoutId} via DirectUpdate");
                }
                else
                {
                    // Create new workout
                    await _calendarService.AddUserWorkoutAsync(userWorkout);
                    _logger.LogInformation($"New workout created for user {userId} on {date:yyyy-MM-dd} with workout {workoutId} via DirectUpdate");
                }
                
                return Json(new { 
                    success = true, 
                    message = "Workout updated successfully.",
                    workoutName = selectedWorkout.Name,
                    workoutId = selectedWorkout.WID
                });
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error in DirectUpdate: {ex.Message}");
                return Json(new { success = false, message = "An error occurred while updating the workout." });
            }
        }

        // API Endpoints for AJAX calls

        [HttpGet]
        [Route("api/calendar/{userId}/{year}/{month}")]
        public async Task<IActionResult> GetCalendarDaysForMonth(int userId, int year, int month)
        {
            try
            {
                var date = new DateTime(year, month, 1);
                var days = await _calendarService.GetCalendarDaysForMonthAsync(userId, date);
                
                // Assign grid positions
                var firstOfMonth = new DateTime(year, month, 1);
                int firstColumn = (int)firstOfMonth.DayOfWeek;

                for (int i = 0; i < days.Count; i++)
                {
                    var day = days[i];
                    int slot = firstColumn + i;
                    day.GridRow = slot / 7;
                    day.GridColumn = slot % 7;
                }

                return Ok(days);
            }
            catch (Exception ex)
            {
                _logger.LogError($"API Error fetching calendar days: {ex.Message}");
                return StatusCode(500, new { error = "An error occurred while loading the calendar." });
            }
        }

        [HttpGet]
        [Route("api/calendar/{userId}/{year}/{month}/{day}")]
        public async Task<IActionResult> GetUserWorkoutForDay(int userId, int year, int month, int day)
        {
            try
            {
                var date = new DateTime(year, month, day);
                var userWorkout = await _calendarService.GetUserWorkoutAsync(userId, date);
                
                if (userWorkout == null)
                {
                    return NotFound(new { error = "No workout found for this day" });
                }
                
                // Get the associated workout name
                var workouts = await _calendarService.GetWorkoutsAsync();
                var workoutDetails = workouts.FirstOrDefault(w => w.WID == userWorkout.WID);
                
                var result = new
                {
                    userWorkout.UID,
                    userWorkout.WID,
                    userWorkout.Date,
                    userWorkout.Completed,
                    WorkoutName = workoutDetails?.Name ?? "Unknown Workout"
                };
                
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError($"API Error fetching user workout: {ex.Message}");
                return StatusCode(500, new { error = "An error occurred while loading workout information." });
            }
        }

        [HttpGet]
        [Route("api/calendar/{userId}/{year}/{month}/{day}/class")]
        public async Task<IActionResult> GetUserClass(int userId, int year, int month, int day)
        {
            try
            {
                var date = new DateTime(year, month, day);
                
                _logger.LogInformation($"Fetching class for userId: {userId}, date: {date:yyyy-MM-dd}");
                
                // First check if this date has a class by looking at the calendar day
                var calendarDays = await _calendarService.GetCalendarDaysForMonthAsync(userId, date);
                var calendarDay = calendarDays.FirstOrDefault(d => d.Date.Day == date.Day);
                
                _logger.LogInformation($"Calendar day found: {calendarDay != null}, HasClass: {calendarDay?.HasClass}");
                
                if (calendarDay == null || !calendarDay.HasClass)
                {
                    return Ok(new { className = "No class found", hasClass = false });
                }
                
                // Try to get the class name from the service
                try
                {
                    var className = await _calendarService.GetUserClassAsync(userId, date);
                    
                    _logger.LogInformation($"Class name from service: {className ?? "null"}");
                    
                    if (!string.IsNullOrEmpty(className))
                    {
                        return Ok(new { className = className, hasClass = true });
                    }
                }
                catch (Exception ex)
                {
                    _logger.LogError($"Error calling GetUserClassAsync: {ex.Message}");
                }
                
                // If we know it has a class but couldn't get the name, return a default
                return Ok(new { className = "Class Scheduled", hasClass = true });
            }
            catch (Exception ex)
            {
                _logger.LogError($"API Error fetching user class: {ex.Message}");
                return StatusCode(500, new { error = "An error occurred while loading class information." });
            }
        }
        
        [HttpGet]
        [Route("api/calendar/workouts")]
        public async Task<IActionResult> GetWorkouts()
        {
            try
            {
                var workouts = await _calendarService.GetWorkoutsAsync();
                return Ok(workouts);
            }
            catch (Exception ex)
            {
                _logger.LogError($"API Error fetching workouts: {ex.Message}");
                return StatusCode(500, new { error = "An error occurred while loading workouts." });
            }
        }

        [HttpGet]
        [Route("api/calendar/workouts/{workoutId}")]
        public async Task<IActionResult> GetWorkout(int workoutId)
        {
            try
            {
                var workouts = await _calendarService.GetWorkoutsAsync();
                var workout = workouts.FirstOrDefault(w => w.WID == workoutId);
                
                if (workout == null)
                {
                    return NotFound(new { error = "Workout not found" });
                }
                
                return Ok(workout);
            }
            catch (Exception ex)
            {
                _logger.LogError($"API Error fetching workout: {ex.Message}");
                return StatusCode(500, new { error = "An error occurred while loading workout information." });
            }
        }

        [HttpDelete]
        [Route("api/calendar/{userId}/{year}/{month}/{day}")]
        public async Task<IActionResult> DeleteWorkoutApi(int userId, int year, int month, int day)
        {
            try
            {
                var date = new DateTime(year, month, day);
                
                // Validate that the date is not in the past
                if (date.Date < DateTime.Today)
                {
                    _logger.LogWarning($"API attempt to delete workout for past date: {date:yyyy-MM-dd}");
                    return BadRequest(new { error = "Cannot delete workouts for past dates." });
                }
                
                // Get the workout details
                var userWorkout = await _calendarService.GetUserWorkoutAsync(userId, date);
                if (userWorkout == null)
                {
                    return NotFound(new { error = "No workout found for this date." });
                }
                
                // Delete the workout
                await _calendarService.DeleteUserWorkoutAsync(userId, userWorkout.WID, date);
                return Ok(new { success = true, message = "Workout deleted successfully." });
            }
            catch (Exception ex)
            {
                _logger.LogError($"API Error deleting workout: {ex.Message}");
                return StatusCode(500, new { error = "An error occurred while deleting the workout." });
            }
        }
    }
} 
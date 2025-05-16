using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Workout.Core.IServices;
using Workout.Core.Models;

namespace Workout.Web.Controllers
{
    public class CalendarController : Controller
    {
        private readonly ICalendarService _calendarService;

        public CalendarController(ICalendarService calendarService)
        {
            _calendarService = calendarService;
        }

        // GET: Calendar
        public async Task<IActionResult> Index()
        {
            var currentDate = DateTime.Now;
            var userId = 1; // TODO: Get actual user ID from authentication
            var days = await _calendarService.GetCalendarDaysForMonthAsync(userId, currentDate);
            return View(days);
        }

        // GET: Calendar/Details/5/2024/3/15
        public async Task<IActionResult> Details(int userId, int year, int month, int day)
        {
            var date = new DateTime(year, month, day);
            var workout = await _calendarService.GetUserWorkoutAsync(userId, date);
            
            if (workout == null)
            {
                return NotFound();
            }

            return View(workout);
        }

        // GET: Calendar/Create
        public async Task<IActionResult> Create()
        {
            ViewBag.Workouts = await _calendarService.GetWorkoutsAsync();
            return View();
        }

        // POST: Calendar/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("UID,WID,Date")] UserWorkoutModel userWorkout)
        {
            if (ModelState.IsValid)
            {
                await _calendarService.AddUserWorkoutAsync(userWorkout);
                return RedirectToAction(nameof(Index));
            }
            
            ViewBag.Workouts = await _calendarService.GetWorkoutsAsync();
            return View(userWorkout);
        }

        // GET: Calendar/Edit/5/2024/3/15
        public async Task<IActionResult> Edit(int userId, int year, int month, int day)
        {
            var date = new DateTime(year, month, day);
            var workout = await _calendarService.GetUserWorkoutAsync(userId, date);
            
            if (workout == null)
            {
                return NotFound();
            }
            
            ViewBag.Workouts = await _calendarService.GetWorkoutsAsync();
            return View(workout);
        }

        // POST: Calendar/Edit
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit([Bind("UID,WID,Date")] UserWorkoutModel userWorkout)
        {
            if (ModelState.IsValid)
            {
                await _calendarService.UpdateUserWorkoutAsync(userWorkout);
                return RedirectToAction(nameof(Index));
            }
            
            ViewBag.Workouts = await _calendarService.GetWorkoutsAsync();
            return View(userWorkout);
        }

        // GET: Calendar/Delete/5/2024/3/15
        public async Task<IActionResult> Delete(int userId, int year, int month, int day)
        {
            var date = new DateTime(year, month, day);
            var workout = await _calendarService.GetUserWorkoutAsync(userId, date);
            
            if (workout == null)
            {
                return NotFound();
            }

            return View(workout);
        }

        // POST: Calendar/Delete/5/2024/3/15
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int userId, int workoutId, int year, int month, int day)
        {
            var date = new DateTime(year, month, day);
            await _calendarService.DeleteUserWorkoutAsync(userId, workoutId, date);
            return RedirectToAction(nameof(Index));
        }
    }
} 
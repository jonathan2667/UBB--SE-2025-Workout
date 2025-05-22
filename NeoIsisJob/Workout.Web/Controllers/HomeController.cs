using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Workout.Core.IServices;
using Workout.Core.Models;
using Workout.Web.Models;
using Workout.Web.Filters;

namespace Workout.Web.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;
    private readonly IWorkoutService _workoutService;
    private readonly ICompleteWorkoutService _completeWorkoutService;
    private readonly IExerciseService _exerciseService;
    private readonly IUserWorkoutService _userWorkoutService;

    public HomeController(
        ILogger<HomeController> logger,
        IWorkoutService workoutService,
        ICompleteWorkoutService completeWorkoutService,
        IExerciseService exerciseService,
        IUserWorkoutService userWorkoutService)
    {
        _logger = logger;
        _workoutService = workoutService;
        _completeWorkoutService = completeWorkoutService;
        _exerciseService = exerciseService;
        _userWorkoutService = userWorkoutService;
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

    public async Task<IActionResult> Index()
    {
        var viewModel = new HomeViewModel
        {
            CurrentDate = DateTime.Now.ToString("dddd, MMMM d, yyyy"),
            WorkoutExercises = new List<HomeViewModel.ExerciseWithDetails>()
        };

        // Check for success message in TempData
        if (TempData["SuccessMessage"] != null)
        {
            viewModel.ShowSuccessMessage = true;
            viewModel.SuccessMessage = TempData["SuccessMessage"].ToString();
        }

        // Check if user is logged in
        var userIdString = HttpContext.Session.GetString("UserId");
        if (!string.IsNullOrEmpty(userIdString))
        {
            try
            {
                // Get today's workout for the current user
                var today = DateTime.Now.Date;
                var currentUserId = GetCurrentUserId();
                var userWorkout = await _userWorkoutService.GetUserWorkoutForDateAsync(currentUserId, today);

                if (userWorkout != null)
                {
                    // Get the workout details
                    viewModel.CurrentWorkout = await _workoutService.GetWorkoutAsync(userWorkout.WID);

                    // Get the exercises for this workout
                    var completeWorkouts = await _completeWorkoutService.GetCompleteWorkoutsByWorkoutIdAsync(userWorkout.WID);

                    foreach (var completeWorkout in completeWorkouts)
                    {
                        var exercise = await _exerciseService.GetExerciseByIdAsync(completeWorkout.EID);
                        if (exercise != null)
                        {
                            viewModel.WorkoutExercises.Add(new HomeViewModel.ExerciseWithDetails
                            {
                                Name = exercise.Name,
                                Details = $"{completeWorkout.Sets} sets × {completeWorkout.RepsPerSet} reps"
                            });
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error loading workout data for home page");
            }
        }

        return View(viewModel);
    }

    [HttpPost]
    [AuthorizeUser]
    public async Task<IActionResult> AddWorkout(int workoutId)
    {
        try
        {
            var today = DateTime.Now.Date;
            var userWorkout = new Workout.Core.Models.UserWorkoutModel
            {
                UID = GetCurrentUserId(),
                WID = workoutId,
                Date = today,
                Completed = false
            };

            await _userWorkoutService.AddUserWorkoutAsync(userWorkout);
            
            // Get the workout name to display in the success message
            var workout = await _workoutService.GetWorkoutAsync(workoutId);
            TempData["SuccessMessage"] = $"Workout '{workout.Name}' successfully added for today!";
            
            return RedirectToAction(nameof(Index));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error adding workout");
            return RedirectToAction(nameof(Index));
        }
    }

    [HttpPost]
    [AuthorizeUser]
    public async Task<IActionResult> CompleteWorkout(int workoutId)
    {
        try
        {
            var today = DateTime.Now.Date;
            await _userWorkoutService.CompleteUserWorkoutAsync(GetCurrentUserId(), workoutId, today);
            return RedirectToAction(nameof(Index));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error completing workout");
            return RedirectToAction(nameof(Index));
        }
    }

    [HttpPost]
    [AuthorizeUser]
    public async Task<IActionResult> DeleteWorkout(int workoutId)
    {
        try
        {
            var today = DateTime.Now.Date;
            await _userWorkoutService.DeleteUserWorkoutAsync(GetCurrentUserId(), workoutId, today);
            return RedirectToAction(nameof(Index));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error deleting workout");
            return RedirectToAction(nameof(Index));
        }
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}

using Microsoft.AspNetCore.Mvc;
using Workout.Core.IServices;
using Workout.Core.Models;
using Workout.Web.Filters;
using Workout.Web.ViewModels.Statistics;

namespace Workout.Web.Controllers
{
    [AuthorizeUser]
    public class StatisticsController : Controller
    {
        private readonly IUserNutritionService _nutritionService;
        private readonly IWaterTrackingService _waterTrackingService;

        public StatisticsController(
            IUserNutritionService nutritionService,
            IWaterTrackingService waterTrackingService)
        {
            _nutritionService = nutritionService;
            _waterTrackingService = waterTrackingService;
        }

        /// <summary>
        /// Displays the main statistics dashboard.
        /// </summary>
        /// <returns>The dashboard view with nutrition and water tracking statistics.</returns>
        public async Task<IActionResult> Dashboard()
        {
            try
            {
                int userId = int.Parse(HttpContext.Session.GetString("UserId"));
                var today = DateTime.Today;

                var viewModel = new StatisticsDashboardViewModel
                {
                    UserId = userId,
                    Date = today
                };

                // Get today's nutrition data
                viewModel.TodayNutrition = await _nutritionService.GetDailyNutritionAsync(userId, today);

                // Get weekly average
                var weekStart = today.AddDays(-(int)today.DayOfWeek);
                viewModel.WeeklyAverage = await _nutritionService.GetWeeklyAverageAsync(userId, weekStart);

                // Get monthly average
                viewModel.MonthlyAverage = await _nutritionService.GetMonthlyAverageAsync(userId, today.Month, today.Year);

                // Get top meal types
                viewModel.TopMealTypes = await _nutritionService.GetTopMealTypesAsync(userId, 30);

                // Get water tracking data
                viewModel.TodayWaterIntake = await _waterTrackingService.GetDailyWaterIntakeAsync(userId, today);
                viewModel.WaterGoal = await _waterTrackingService.GetWaterGoalAsync(userId);
                viewModel.WaterProgress = await _waterTrackingService.GetWaterIntakeProgressAsync(userId, today);
                viewModel.WaterHistory = await _waterTrackingService.GetWaterIntakeHistoryAsync(userId, 7);

                // Get today's meal logs
                viewModel.TodayMealLogs = await _nutritionService.GetMealLogsAsync(userId, today);

                // Get nutrition trends (last 7 days)
                viewModel.NutritionTrends = await _nutritionService.GetNutritionDataAsync(userId, 7);

                return View(viewModel);
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = "Failed to load statistics: " + ex.Message;
                return View(new StatisticsDashboardViewModel());
            }
        }

        /// <summary>
        /// Gets nutrition data for a specified number of days.
        /// </summary>
        /// <param name="days">The number of days to retrieve data for.</param>
        /// <returns>JSON data with nutrition information.</returns>
        [HttpGet]
        public async Task<IActionResult> GetNutritionData(int days = 7)
        {
            try
            {
                int userId = int.Parse(HttpContext.Session.GetString("UserId"));
                var nutritionData = await _nutritionService.GetNutritionDataAsync(userId, days);
                
                return Json(new { success = true, data = nutritionData });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message });
            }
        }

        /// <summary>
        /// Logs a meal as consumed by the user.
        /// </summary>
        /// <param name="mealId">The meal identifier.</param>
        /// <param name="portionMultiplier">The portion size multiplier.</param>
        /// <param name="notes">Optional notes about the meal.</param>
        /// <returns>Redirect to dashboard with success/error message.</returns>
        [HttpPost]
        public async Task<IActionResult> LogMeal(int mealId, double portionMultiplier = 1.0, string notes = null)
        {
            try
            {
                int userId = int.Parse(HttpContext.Session.GetString("UserId"));
                await _nutritionService.LogMealAsync(userId, mealId, portionMultiplier, notes);
                
                TempData["SuccessMessage"] = "Meal logged successfully!";
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = "Failed to log meal: " + ex.Message;
            }
            
            return RedirectToAction(nameof(Dashboard));
        }

        /// <summary>
        /// Adds water intake for the user.
        /// </summary>
        /// <param name="amount">The amount of water in milliliters.</param>
        /// <param name="notes">Optional notes about the water intake.</param>
        /// <returns>JSON response indicating success or failure.</returns>
        [HttpPost]
        public async Task<IActionResult> AddWater(int amount, string notes = null)
        {
            try
            {
                int userId = int.Parse(HttpContext.Session.GetString("UserId"));
                await _waterTrackingService.AddWaterIntakeAsync(userId, amount, notes);
                
                var todayIntake = await _waterTrackingService.GetDailyWaterIntakeAsync(userId, DateTime.Today);
                var progress = await _waterTrackingService.GetWaterIntakeProgressAsync(userId, DateTime.Today);
                
                return Json(new { 
                    success = true, 
                    message = $"Added {amount}ml of water!",
                    totalIntake = todayIntake,
                    progress = Math.Round(progress, 1)
                });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message });
            }
        }

        /// <summary>
        /// Gets today's water intake data.
        /// </summary>
        /// <returns>JSON data with today's water intake information.</returns>
        [HttpGet]
        public async Task<IActionResult> GetWaterToday()
        {
            try
            {
                int userId = int.Parse(HttpContext.Session.GetString("UserId"));
                var today = DateTime.Today;
                
                var totalIntake = await _waterTrackingService.GetDailyWaterIntakeAsync(userId, today);
                var goal = await _waterTrackingService.GetWaterGoalAsync(userId);
                var progress = await _waterTrackingService.GetWaterIntakeProgressAsync(userId, today);
                var entries = await _waterTrackingService.GetWaterIntakeEntriesAsync(userId, today);
                
                return Json(new { 
                    success = true,
                    totalIntake = totalIntake,
                    goal = goal,
                    progress = Math.Round(progress, 1),
                    entries = entries.Select(e => new {
                        id = e.Id,
                        amount = e.AmountMl,
                        time = e.ConsumedAt.ToString("HH:mm"),
                        notes = e.Notes
                    })
                });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message });
            }
        }

        /// <summary>
        /// Deletes a water intake entry.
        /// </summary>
        /// <param name="entryId">The water intake entry identifier.</param>
        /// <returns>JSON response indicating success or failure.</returns>
        [HttpPost]
        public async Task<IActionResult> DeleteWaterEntry(int entryId)
        {
            try
            {
                int userId = int.Parse(HttpContext.Session.GetString("UserId"));
                var success = await _waterTrackingService.DeleteWaterIntakeAsync(userId, entryId);
                
                if (success)
                {
                    var todayIntake = await _waterTrackingService.GetDailyWaterIntakeAsync(userId, DateTime.Today);
                    var progress = await _waterTrackingService.GetWaterIntakeProgressAsync(userId, DateTime.Today);
                    
                    return Json(new { 
                        success = true, 
                        message = "Water entry deleted!",
                        totalIntake = todayIntake,
                        progress = Math.Round(progress, 1)
                    });
                }
                else
                {
                    return Json(new { success = false, message = "Entry not found or could not be deleted." });
                }
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message });
            }
        }

        /// <summary>
        /// Gets nutrition data for charts and analysis.
        /// </summary>
        /// <param name="days">The number of days to analyze.</param>
        /// <returns>JSON data formatted for charts.</returns>
        [HttpGet]
        public async Task<IActionResult> GetNutritionChartData(int days = 30)
        {
            try
            {
                int userId = int.Parse(HttpContext.Session.GetString("UserId"));
                var nutritionData = await _nutritionService.GetNutritionDataAsync(userId, days);
                
                var chartData = new
                {
                    labels = nutritionData.Select(n => n.Date.ToString("MM/dd")).ToArray(),
                    calories = nutritionData.Select(n => n.TotalCalories).ToArray(),
                    proteins = nutritionData.Select(n => n.TotalProteins).ToArray(),
                    carbs = nutritionData.Select(n => n.TotalCarbohydrates).ToArray(),
                    fats = nutritionData.Select(n => n.TotalFats).ToArray(),
                    water = nutritionData.Select(n => n.WaterIntakeMl).ToArray()
                };
                
                return Json(new { success = true, data = chartData });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message });
            }
        }
    }
} 
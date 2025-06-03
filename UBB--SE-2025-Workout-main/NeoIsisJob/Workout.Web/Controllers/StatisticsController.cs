using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Workout.Core.IServices;
using Workout.Core.Models;
using Workout.Web.Filters;
using Workout.Web.ViewModels.Statistics;
using Workout.Core.Data;
using Workout.Core.Services;

namespace Workout.Web.Controllers
{
    [AuthorizeUser]
    public class StatisticsController : Controller
    {
        private readonly IUserNutritionService _nutritionService;
        private readonly IWaterTrackingService _waterTrackingService;
        private readonly IService<MealModel> _mealService;

        public StatisticsController(
            IUserNutritionService nutritionService,
            IWaterTrackingService waterTrackingService,
            IService<MealModel> mealService)
        {
            _nutritionService = nutritionService;
            _waterTrackingService = waterTrackingService;
            _mealService = mealService;
        }

        /// <summary>
        /// Displays the statistics dashboard with nutrition and water tracking data.
        /// </summary>
        /// <returns>The dashboard view with statistics data.</returns>
        [HttpGet]
        public async Task<IActionResult> Dashboard()
        {
            try
            {
                var viewModel = new StatisticsDashboardViewModel();
                
                // Get user ID from session like the working MealController does
                int userId = int.Parse(HttpContext.Session.GetString("UserId"));
                
                viewModel.UserId = userId;
                var today = DateTime.Today;
                
                // Load nutrition data - simple direct call like MealController
                try
                {
                    viewModel.TodayNutrition = await _nutritionService.GetDailyNutritionAsync(userId, today);
                }
                catch (Exception ex)
                {
                    // Create a default nutrition record for today if none exists
                    viewModel.TodayNutrition = new UserDailyNutritionModel
                    {
                        UserId = userId,
                        Date = today,
                        TotalCalories = 0,
                        TotalProteins = 0,
                        TotalCarbohydrates = 0,
                        TotalFats = 0,
                        MealsConsumed = 0,
                        WaterIntakeMl = 0
                    };
                    System.Diagnostics.Debug.WriteLine($"Using default nutrition data: {ex.Message}");
                }

                // Load water tracking data - simple direct call like MealController
                try
                {
                    viewModel.TodayWaterIntake = await _waterTrackingService.GetDailyWaterIntakeAsync(userId, today);
                    viewModel.WaterGoal = await _waterTrackingService.GetWaterGoalAsync(userId);
                    viewModel.WaterProgress = await _waterTrackingService.GetWaterIntakeProgressAsync(userId, today);
                    viewModel.WaterHistory = await _waterTrackingService.GetWaterIntakeHistoryAsync(userId, 7);
                }
                catch (Exception ex)
                {
                    viewModel.TodayWaterIntake = 0;
                    viewModel.WaterGoal = 2000;
                    viewModel.WaterProgress = 0;
                    viewModel.WaterHistory = new Dictionary<DateTime, int>();
                    System.Diagnostics.Debug.WriteLine($"Using default water data: {ex.Message}");
                }

                // Load other data with simple fallbacks
                try
                {
                    var weekStart = today.AddDays(-(int)today.DayOfWeek);
                    viewModel.WeeklyAverage = await _nutritionService.GetWeeklyAverageAsync(userId, weekStart);
                }
                catch (Exception ex)
                {
                    viewModel.WeeklyAverage = new UserDailyNutritionModel { UserId = userId, Date = today };
                    System.Diagnostics.Debug.WriteLine($"Using default weekly data: {ex.Message}");
                }

                try
                {
                    viewModel.MonthlyAverage = await _nutritionService.GetMonthlyAverageAsync(userId, today.Month, today.Year);
                }
                catch (Exception ex)
                {
                    viewModel.MonthlyAverage = new UserDailyNutritionModel { UserId = userId, Date = today };
                    System.Diagnostics.Debug.WriteLine($"Using default monthly data: {ex.Message}");
                }

                try
                {
                    var topMealTypesData = await _nutritionService.GetTopMealTypesAsync(userId, 30);
                    viewModel.TopMealTypes = topMealTypesData.Take(5).ToDictionary(kvp => kvp.Key, kvp => kvp.Value);
                }
                catch (Exception ex)
                {
                    viewModel.TopMealTypes = new Dictionary<string, int>();
                    System.Diagnostics.Debug.WriteLine($"No meal types available: {ex.Message}");
                }

                try
                {
                    viewModel.TodayMealLogs = await _nutritionService.GetMealLogsAsync(userId, today);
                }
                catch (Exception ex)
                {
                    viewModel.TodayMealLogs = new List<UserMealLogModel>();
                    System.Diagnostics.Debug.WriteLine($"No meal logs available: {ex.Message}");
                }

                try
                {
                    viewModel.NutritionTrends = await _nutritionService.GetNutritionDataAsync(userId, 7);
                }
                catch (Exception ex)
                {
                    viewModel.NutritionTrends = new List<UserDailyNutritionModel>();
                    System.Diagnostics.Debug.WriteLine($"No nutrition trends available: {ex.Message}");
                }

                return View(viewModel);
            }
            catch (Exception ex)
            {
                // If everything fails, still show the view with empty data
                TempData["ErrorMessage"] = "Some statistics data could not be loaded: " + ex.Message;
                System.Diagnostics.Debug.WriteLine($"Statistics dashboard error: {ex}");
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
        /// Gets all available meals for meal logging.
        /// </summary>
        /// <returns>JSON response with all meals.</returns>
        [HttpGet]
        public async Task<IActionResult> GetAllMeals()
        {
            try
            {
                int userId = int.Parse(HttpContext.Session.GetString("UserId"));
                System.Diagnostics.Debug.WriteLine($"GetAllMeals: userId={userId}");
                
                var meals = await _mealService.GetAllAsync();
                System.Diagnostics.Debug.WriteLine($"GetAllMeals: Found {meals.Count()} meals");
                
                return Json(new { success = true, meals = meals });
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"GetAllMeals error: {ex.Message}");
                return Json(new { success = false, message = ex.Message });
            }
        }

        /// <summary>
        /// Logs a meal as consumed by the user.
        /// </summary>
        /// <param name="mealId">The meal identifier.</param>
        /// <param name="portionMultiplier">The portion size multiplier.</param>
        /// <param name="notes">Optional notes about the meal.</param>
        /// <returns>JSON response for AJAX or redirect for regular form submission.</returns>
        [HttpPost]
        public async Task<IActionResult> LogMeal(int mealId, double portionMultiplier = 1.0, string notes = null)
        {
            try
            {
                // Validate inputs
                if (mealId <= 0)
                {
                    var errorMsg = "Invalid meal ID.";
                    if (Request.Headers["X-Requested-With"] == "XMLHttpRequest" || 
                        Request.ContentType?.Contains("application/json") == true)
                    {
                        return Json(new { success = false, message = errorMsg });
                    }
                    TempData["ErrorMessage"] = errorMsg;
                    return RedirectToAction(nameof(Dashboard));
                }

                if (portionMultiplier <= 0 || portionMultiplier > 10)
                {
                    var errorMsg = "Portion size must be between 0.1 and 10.";
                    if (Request.Headers["X-Requested-With"] == "XMLHttpRequest" || 
                        Request.ContentType?.Contains("application/json") == true)
                    {
                        return Json(new { success = false, message = errorMsg });
                    }
                    TempData["ErrorMessage"] = errorMsg;
                    return RedirectToAction(nameof(Dashboard));
                }

                // Get user ID from session - same pattern as MealController
                int userId = int.Parse(HttpContext.Session.GetString("UserId"));
                System.Diagnostics.Debug.WriteLine($"LogMeal: userId={userId}, mealId={mealId}, portion={portionMultiplier}, notes={notes}");
                
                // Log meal using the service - same pattern as MealController
                var mealLog = await _nutritionService.LogMealAsync(userId, mealId, portionMultiplier, notes ?? string.Empty);
                System.Diagnostics.Debug.WriteLine($"LogMeal: Successfully logged meal with ID={mealLog.Id}");
                
                // Get updated nutrition data for response
                var todayNutrition = await _nutritionService.GetTodayNutritionAsync(userId);
                System.Diagnostics.Debug.WriteLine($"LogMeal: Updated nutrition - Calories={todayNutrition.TotalCalories}");

                var successMsg = $"Successfully logged {mealLog.Meal?.Name ?? "meal"}!";
                
                // Return JSON for AJAX requests
                if (Request.Headers["X-Requested-With"] == "XMLHttpRequest" || 
                    Request.ContentType?.Contains("multipart/form-data") == true)
                {
                    return Json(new { 
                        success = true, 
                        message = successMsg,
                        mealLog = new {
                            id = mealLog.Id,
                            mealName = mealLog.Meal?.Name,
                            portionMultiplier = mealLog.PortionMultiplier,
                            notes = mealLog.Notes,
                            consumedAt = mealLog.ConsumedAt.ToString("HH:mm"),
                            calories = (mealLog.Meal?.Calories * mealLog.PortionMultiplier ?? 0)
                        },
                        todayNutrition = new {
                            totalCalories = todayNutrition.TotalCalories,
                            totalProteins = todayNutrition.TotalProteins,
                            totalCarbohydrates = todayNutrition.TotalCarbohydrates,
                            totalFats = todayNutrition.TotalFats,
                            mealsConsumed = todayNutrition.MealsConsumed
                        }
                    });
                }
                
                // Regular form submission - redirect with message
                TempData["SuccessMessage"] = successMsg;
                return RedirectToAction(nameof(Dashboard));
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"LogMeal error: {ex.Message}");
                var errorMsg = "Failed to log meal: " + ex.Message;
                
                if (Request.Headers["X-Requested-With"] == "XMLHttpRequest" || 
                    Request.ContentType?.Contains("multipart/form-data") == true)
                {
                    return Json(new { success = false, message = errorMsg });
                }
                
                TempData["ErrorMessage"] = errorMsg;
                return RedirectToAction(nameof(Dashboard));
            }
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
                // Validate amount
                if (amount <= 0)
                {
                    return Json(new { success = false, message = "Water amount must be greater than 0." });
                }

                // Get user ID from session - same pattern as MealController
                int userId = int.Parse(HttpContext.Session.GetString("UserId"));
                System.Diagnostics.Debug.WriteLine($"AddWater: userId={userId}, amount={amount}");

                // Add water using the service - same pattern as MealController
                var addResult = await _waterTrackingService.AddWaterIntakeAsync(userId, amount, notes);
                System.Diagnostics.Debug.WriteLine($"AddWater: Service returned entry ID={addResult?.Id}, Amount={addResult?.AmountMl}");

                // Get updated data for the UI - if this fails, use reasonable defaults
                int totalIntake = amount; // At minimum, we added this amount
                double progress = 0;
                int goal = 2000;

                try
                {
                    System.Diagnostics.Debug.WriteLine($"AddWater: Getting daily water intake for userId={userId}, date={DateTime.Today}");
                    totalIntake = await _waterTrackingService.GetDailyWaterIntakeAsync(userId, DateTime.Today);
                    System.Diagnostics.Debug.WriteLine($"AddWater: Got totalIntake={totalIntake}");
                    
                    goal = await _waterTrackingService.GetWaterGoalAsync(userId);
                    System.Diagnostics.Debug.WriteLine($"AddWater: Got goal={goal}");
                    
                    progress = await _waterTrackingService.GetWaterIntakeProgressAsync(userId, DateTime.Today);
                    System.Diagnostics.Debug.WriteLine($"AddWater: Got progress={progress}");
                }
                catch (Exception ex)
                {
                    // If we can't get updated data, calculate basic progress
                    progress = goal > 0 ? (double)totalIntake / goal * 100 : 0;
                    System.Diagnostics.Debug.WriteLine($"AddWater: Exception getting updated data: {ex.Message}");
                    System.Diagnostics.Debug.WriteLine($"AddWater: Using fallback values - totalIntake={totalIntake}, goal={goal}, progress={progress}");
                }
                
                System.Diagnostics.Debug.WriteLine($"AddWater: Final response - totalIntake={totalIntake}, progress={progress}, goal={goal}");
                
                return Json(new { 
                    success = true, 
                    message = $"Added {amount}ml of water!",
                    totalIntake = totalIntake,
                    progress = Math.Round(progress, 1),
                    goal = goal
                });
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"AddWater: Main exception: {ex.Message}");
                return Json(new { success = false, message = "Failed to add water: " + ex.Message });
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
                
                return Json(new { 
                    success = true,
                    totalIntake = totalIntake,
                    goal = goal,
                    progress = Math.Round(progress, 1)
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
        /// Gets nutrition chart data for visualization.
        /// </summary>
        /// <param name="days">Number of days to include in the chart.</param>
        /// <returns>JSON data formatted for chart display.</returns>
        [HttpGet]
        public async Task<IActionResult> GetNutritionChartData(int days = 30)
        {
            try
            {
                int userId = int.Parse(HttpContext.Session.GetString("UserId"));
                var nutritionData = await _nutritionService.GetNutritionDataAsync(userId, days);
                
                // Format data for chart
                var chartData = nutritionData.Select(n => new {
                    date = n.Date.ToString("yyyy-MM-dd"),
                    calories = n.TotalCalories,
                    proteins = n.TotalProteins,
                    carbs = n.TotalCarbohydrates,
                    fats = n.TotalFats
                }).OrderBy(n => n.date);
                
                return Json(new { success = true, data = chartData });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message });
            }
        }
    }
} 
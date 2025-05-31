using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Workout.Core.IServices;
using Workout.Core.Models;
using Workout.Web.Filters;
using Workout.Web.ViewModels.Statistics;
using Workout.Core.Data;

namespace Workout.Web.Controllers
{
    [AuthorizeUser]
    public class StatisticsController : Controller
    {
        private readonly IUserNutritionService _nutritionService;
        private readonly IWaterTrackingService _waterTrackingService;
        private readonly IService<MealModel> _mealService;
        private readonly WorkoutDbContext _dbContext;

        public StatisticsController(
            IUserNutritionService nutritionService,
            IWaterTrackingService waterTrackingService,
            IService<MealModel> mealService,
            WorkoutDbContext dbContext)
        {
            _nutritionService = nutritionService;
            _waterTrackingService = waterTrackingService;
            _mealService = mealService;
            _dbContext = dbContext;
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

        /// <summary>
        /// Test endpoint to check database connectivity and add sample data.
        /// </summary>
        /// <returns>JSON response with database status and sample data creation results.</returns>
        [HttpGet]
        public async Task<IActionResult> TestDatabase()
        {
            try
            {
                int userId = 1; // Default test user
                if (HttpContext.Session.GetString("UserId") != null)
                {
                    if (!int.TryParse(HttpContext.Session.GetString("UserId"), out userId))
                    {
                        userId = 1;
                    }
                }

                var today = DateTime.Today;
                var results = new List<string>();
                
                // Test nutrition service
                try
                {
                    var nutrition = await _nutritionService.GetDailyNutritionAsync(userId, today);
                    results.Add($"✅ Nutrition service working - found {nutrition.TotalCalories} calories for today");
                }
                catch (Exception ex)
                {
                    results.Add($"❌ Nutrition service failed: {ex.Message}");
                }

                // Test water tracking service
                try
                {
                    var waterIntake = await _waterTrackingService.GetDailyWaterIntakeAsync(userId, today);
                    results.Add($"✅ Water tracking service working - found {waterIntake}ml for today");
                }
                catch (Exception ex)
                {
                    results.Add($"❌ Water tracking service failed: {ex.Message}");
                }

                // Test meal logs
                try
                {
                    var mealLogs = await _nutritionService.GetMealLogsAsync(userId, today);
                    results.Add($"✅ Meal logs service working - found {mealLogs.Count()} meals for today");
                }
                catch (Exception ex)
                {
                    results.Add($"❌ Meal logs service failed: {ex.Message}");
                }

                return Json(new { 
                    success = true, 
                    userId = userId,
                    testDate = today.ToString("yyyy-MM-dd"),
                    results = results 
                });
            }
            catch (Exception ex)
            {
                return Json(new { 
                    success = false, 
                    message = $"Database test failed: {ex.Message}",
                    innerException = ex.InnerException?.Message 
                });
            }
        }

        /// <summary>
        /// Seeds sample data for testing the statistics functionality.
        /// </summary>
        /// <returns>JSON response with seeding results.</returns>
        [HttpPost]
        public async Task<IActionResult> SeedSampleData()
        {
            try
            {
                var results = new List<string>();
                int userId = 1;
                var today = DateTime.Today;

                // Set user in session for proper functionality
                HttpContext.Session.SetString("UserId", userId.ToString());
                results.Add($"✅ Set user session: User {userId}");

                // Ensure database is created
                await _dbContext.Database.EnsureCreatedAsync();
                results.Add("✅ Database ensured created");

                // Check if user exists, create if not
                var user = await _dbContext.Users.FindAsync(userId);
                if (user == null)
                {
                    user = new UserModel
                    {
                        ID = userId,
                        Username = "TestUser",
                        Email = "test@example.com",
                        Password = "password123"
                    };
                    _dbContext.Users.Add(user);
                    await _dbContext.SaveChangesAsync();
                    results.Add("✅ Created test user");
                }
                else
                {
                    results.Add("✅ Test user already exists");
                }

                // Add sample nutrition data using services
                try
                {
                    for (int i = 0; i < 7; i++)
                    {
                        var date = today.AddDays(-i);
                        
                        // Check if nutrition data already exists for this date
                        try
                        {
                            var existingNutrition = await _nutritionService.GetDailyNutritionAsync(userId, date);
                            if (existingNutrition.TotalCalories > 0)
                            {
                                continue; // Skip if data already exists
                            }
                        }
                        catch
                        {
                            // No existing data, continue to create
                        }
                        
                        // Create nutrition data directly in database since service might not have a create method
                        var nutrition = new UserDailyNutritionModel
                        {
                            UserId = userId,
                            Date = date,
                            TotalCalories = 1800 + (i * 100),
                            TotalProteins = 120 + (i * 10),
                            TotalCarbohydrates = 200 + (i * 15),
                            TotalFats = 60 + (i * 5),
                            MealsConsumed = 3 + (i % 2),
                            WaterIntakeMl = 2000 + (i * 250)
                        };
                        
                        _dbContext.UserDailyNutrition.Add(nutrition);
                    }
                    await _dbContext.SaveChangesAsync();
                    results.Add("✅ Added sample nutrition data for 7 days");
                }
                catch (Exception ex)
                {
                    results.Add($"⚠️ Nutrition data seeding had issues: {ex.Message}");
                }

                // Add sample water intake using service
                try
                {
                    // Check if water data exists for today
                    var existingWater = 0;
                    try
                    {
                        existingWater = await _waterTrackingService.GetDailyWaterIntakeAsync(userId, today);
                    }
                    catch
                    {
                        // No existing data
                    }
                    
                    if (existingWater == 0)
                    {
                        // Add water intake entries using the service
                        await _waterTrackingService.AddWaterIntakeAsync(userId, 250, "Morning glass");
                        await _waterTrackingService.AddWaterIntakeAsync(userId, 500, "Lunch bottle");
                        await _waterTrackingService.AddWaterIntakeAsync(userId, 300, "Afternoon");
                        results.Add("✅ Added sample water intake for today using service");
                    }
                    else
                    {
                        results.Add("✅ Water intake data already exists for today");
                    }
                }
                catch (Exception ex)
                {
                    // Fallback to direct database insertion
                    try
                    {
                        var existingWater = _dbContext.UserWaterIntake
                            .Where(w => w.UserId == userId && w.ConsumedAt.Date == today)
                            .ToList();
                        
                        if (!existingWater.Any())
                        {
                            var waterEntries = new[]
                            {
                                new UserWaterIntakeModel { UserId = userId, AmountMl = 250, ConsumedAt = today.AddHours(8), Notes = "Morning glass" },
                                new UserWaterIntakeModel { UserId = userId, AmountMl = 500, ConsumedAt = today.AddHours(12), Notes = "Lunch bottle" },
                                new UserWaterIntakeModel { UserId = userId, AmountMl = 300, ConsumedAt = today.AddHours(15), Notes = "Afternoon" }
                            };
                            _dbContext.UserWaterIntake.AddRange(waterEntries);
                            await _dbContext.SaveChangesAsync();
                            results.Add("✅ Added sample water intake for today using database fallback");
                        }
                        else
                        {
                            results.Add("✅ Water intake data already exists for today");
                        }
                    }
                    catch (Exception dbEx)
                    {
                        results.Add($"⚠️ Water data seeding failed: {ex.Message} (DB fallback: {dbEx.Message})");
                    }
                }

                // Add sample meal logs using service
                try
                {
                    var existingMeals = await _nutritionService.GetMealLogsAsync(userId, today);
                    if (!existingMeals.Any())
                    {
                        // First, let's check if we have meals in the database
                        var meals = _dbContext.Meals.Take(3).ToList();
                        if (meals.Any())
                        {
                            foreach (var meal in meals)
                            {
                                try
                                {
                                    await _nutritionService.LogMealAsync(userId, meal.Id, 1.0, $"Sample {meal.Name}");
                                }
                                catch
                                {
                                    // Fallback to direct database insertion
                                    var mealLog = new UserMealLogModel
                                    {
                                        UserId = userId,
                                        MealId = meal.Id,
                                        ConsumedAt = today.AddHours(8 + (meals.IndexOf(meal) * 4)),
                                        PortionMultiplier = 1.0,
                                        Notes = $"Sample {meal.Name}"
                                    };
                                    _dbContext.UserMealLogs.Add(mealLog);
                                }
                            }
                            await _dbContext.SaveChangesAsync();
                            results.Add($"✅ Added {meals.Count} sample meal logs for today");
                        }
                        else
                        {
                            results.Add("⚠️ No meals found in database to create meal logs");
                        }
                    }
                    else
                    {
                        results.Add("✅ Meal logs already exist for today");
                    }
                }
                catch (Exception ex)
                {
                    results.Add($"⚠️ Meal logs seeding had issues: {ex.Message}");
                }

                return Json(new { 
                    success = true, 
                    message = "Sample data seeded successfully!",
                    results = results 
                });
            }
            catch (Exception ex)
            {
                return Json(new { 
                    success = false, 
                    message = $"Failed to seed sample data: {ex.Message}",
                    innerException = ex.InnerException?.Message 
                });
            }
        }

        /// <summary>
        /// Diagnostic test to check database connection and water intake table operations.
        /// </summary>
        /// <returns>JSON response with detailed database test results.</returns>
        [HttpGet]
        public async Task<IActionResult> TestDatabaseWater()
        {
            try
            {
                var results = new List<string>();
                int userId = 1;
                var today = DateTime.Today;

                // Test 1: Database connection
                try
                {
                    var canConnect = await _dbContext.Database.CanConnectAsync();
                    results.Add($"✅ Database connection: {canConnect}");
                }
                catch (Exception ex)
                {
                    results.Add($"❌ Database connection failed: {ex.Message}");
                    return Json(new { success = false, results = results });
                }

                // Test 2: Check if UserWaterIntake table exists
                try
                {
                    var tableExists = await _dbContext.UserWaterIntake.CountAsync() >= 0;
                    results.Add($"✅ UserWaterIntake table accessible: {tableExists}");
                }
                catch (Exception ex)
                {
                    results.Add($"❌ UserWaterIntake table access failed: {ex.Message}");
                }

                // Test 3: Direct database water intake insertion
                try
                {
                    var testWaterEntry = new UserWaterIntakeModel
                    {
                        UserId = userId,
                        AmountMl = 100,
                        ConsumedAt = DateTime.Now,
                        Notes = "Database test entry",
                        CreatedAt = DateTime.Now
                    };
                    
                    _dbContext.UserWaterIntake.Add(testWaterEntry);
                    await _dbContext.SaveChangesAsync();
                    results.Add($"✅ Direct database insertion successful: Entry ID {testWaterEntry.Id}");
                    
                    // Test 4: Direct database water intake retrieval
                    var retrievedEntries = await _dbContext.UserWaterIntake
                        .Where(w => w.UserId == userId && w.ConsumedAt.Date == DateTime.Today)
                        .ToListAsync();
                    results.Add($"✅ Direct database retrieval successful: Found {retrievedEntries.Count} entries for today");
                    
                    var totalWater = retrievedEntries.Sum(w => w.AmountMl);
                    results.Add($"✅ Total water from database: {totalWater}ml");
                    
                    // Clean up test entry
                    _dbContext.UserWaterIntake.Remove(testWaterEntry);
                    await _dbContext.SaveChangesAsync();
                    results.Add($"✅ Test entry cleaned up");
                }
                catch (Exception ex)
                {
                    results.Add($"❌ Direct database operations failed: {ex.Message}");
                    if (ex.InnerException != null)
                    {
                        results.Add($"❌ Inner exception: {ex.InnerException.Message}");
                    }
                }

                // Test 5: Service layer operations
                try
                {
                    // Try to get existing data
                    var serviceWaterToday = await _waterTrackingService.GetDailyWaterIntakeAsync(userId, today);
                    results.Add($"✅ Service GetDailyWaterIntakeAsync: {serviceWaterToday}ml");
                }
                catch (Exception ex)
                {
                    results.Add($"❌ Service GetDailyWaterIntakeAsync failed: {ex.Message}");
                }

                try
                {
                    // Try to add water via service
                    var addedEntry = await _waterTrackingService.AddWaterIntakeAsync(userId, 50, "Service test");
                    results.Add($"✅ Service AddWaterIntakeAsync successful: Entry ID {addedEntry.Id}");
                    
                    // Get updated total
                    var updatedTotal = await _waterTrackingService.GetDailyWaterIntakeAsync(userId, today);
                    results.Add($"✅ Updated total via service: {updatedTotal}ml");
                    
                    // Clean up
                    await _waterTrackingService.DeleteWaterIntakeAsync(userId, addedEntry.Id);
                    results.Add($"✅ Service test entry cleaned up");
                }
                catch (Exception ex)
                {
                    results.Add($"❌ Service AddWaterIntakeAsync failed: {ex.Message}");
                    if (ex.InnerException != null)
                    {
                        results.Add($"❌ Inner exception: {ex.InnerException.Message}");
                    }
                }

                return Json(new { 
                    success = true, 
                    message = "Database water test completed",
                    results = results 
                });
            }
            catch (Exception ex)
            {
                return Json(new { 
                    success = false, 
                    message = $"Database water test failed: {ex.Message}",
                    innerException = ex.InnerException?.Message 
                });
            }
        }

        /// <summary>
        /// Simple test to diagnose water persistence issue.
        /// </summary>
        /// <returns>JSON response with detailed test results.</returns>
        [HttpGet]
        public async Task<IActionResult> TestWaterPersistence()
        {
            try
            {
                var results = new List<string>();
                int userId = 1;
                
                // Ensure user is in session
                HttpContext.Session.SetString("UserId", userId.ToString());
                
                // Test 1: Try to add water via service
                results.Add("=== Testing Water Persistence ===");
                
                var initialTotal = await _waterTrackingService.GetDailyWaterIntakeAsync(userId, DateTime.Today);
                results.Add($"Initial water today: {initialTotal}ml");
                
                // Add test water entry
                var testAmount = 100;
                var testEntry = await _waterTrackingService.AddWaterIntakeAsync(userId, testAmount, "Persistence test");
                results.Add($"Added {testAmount}ml water via service. Entry ID: {testEntry.Id}");
                
                // Immediately check if it's there
                var newTotal = await _waterTrackingService.GetDailyWaterIntakeAsync(userId, DateTime.Today);
                results.Add($"New total after adding: {newTotal}ml");
                
                if (newTotal == initialTotal + testAmount)
                {
                    results.Add("✅ Water persistence working correctly!");
                }
                else
                {
                    results.Add($"❌ Water persistence failed. Expected: {initialTotal + testAmount}ml, Got: {newTotal}ml");
                }
                
                // Test 2: Check if entry exists in database
                var dbEntry = await _dbContext.UserWaterIntake
                    .FirstOrDefaultAsync(w => w.Id == testEntry.Id);
                
                if (dbEntry != null)
                {
                    results.Add($"✅ Entry found in database: {dbEntry.AmountMl}ml at {dbEntry.ConsumedAt}");
                }
                else
                {
                    results.Add("❌ Entry NOT found in database!");
                }
                
                // Clean up test entry
                await _waterTrackingService.DeleteWaterIntakeAsync(userId, testEntry.Id);
                results.Add("🧹 Test entry cleaned up");
                
                return Json(new { 
                    success = true, 
                    results = results,
                    message = "Water persistence test completed"
                });
            }
            catch (Exception ex)
            {
                return Json(new { 
                    success = false, 
                    message = $"Water persistence test failed: {ex.Message}",
                    innerException = ex.InnerException?.Message 
                });
            }
        }

        /// <summary>
        /// Simple test to check if the database tables exist and are accessible.
        /// </summary>
        /// <returns>JSON response with database test results.</returns>
        [HttpGet]
        public async Task<IActionResult> TestDbConnection()
        {
            try
            {
                var results = new List<string>();
                
                // Test 1: Can we connect to database?
                var canConnect = await _dbContext.Database.CanConnectAsync();
                results.Add($"Database connection: {canConnect}");
                
                // Test 2: Can we access Users table?
                var userCount = await _dbContext.Users.CountAsync();
                results.Add($"Users table: {userCount} users found");
                
                // Test 3: Can we access UserWaterIntake table?
                var waterCount = await _dbContext.UserWaterIntake.CountAsync();
                results.Add($"UserWaterIntake table: {waterCount} entries found");
                
                // Test 4: Can we access UserFavoriteMeals table (this works)?
                var favCount = await _dbContext.UserFavoriteMeals.CountAsync();
                results.Add($"UserFavoriteMeals table: {favCount} entries found");
                
                return Json(new { 
                    success = true, 
                    results = results,
                    message = "Database connection test completed"
                });
            }
            catch (Exception ex)
            {
                return Json(new { 
                    success = false, 
                    message = $"Database test failed: {ex.Message}",
                    innerException = ex.InnerException?.Message 
                });
            }
        }

        /// <summary>
        /// Test user data isolation - demonstrates that each user has separate water/meal data.
        /// </summary>
        /// <returns>JSON response showing user data isolation test results.</returns>
        [HttpGet]
        public async Task<IActionResult> TestUserDataIsolation()
        {
            try
            {
                var results = new List<string>();
                
                // Get the current logged-in user
                int currentUserId = int.Parse(HttpContext.Session.GetString("UserId"));
                results.Add($"=== Testing Data Isolation for User {currentUserId} ===");
                
                // Test 1: Create water data for current user
                try
                {
                    await _waterTrackingService.AddWaterIntakeAsync(currentUserId, 300, $"Test water for user {currentUserId}");
                    results.Add($"✅ Added test water for User {currentUserId}");
                }
                catch (Exception ex)
                {
                    results.Add($"❌ Failed to add water for User {currentUserId}: {ex.Message}");
                }
                
                // Test 2: Create water data for a different user (User 2)
                int otherUserId = currentUserId == 1 ? 2 : 1;
                try
                {
                    await _waterTrackingService.AddWaterIntakeAsync(otherUserId, 500, $"Test water for user {otherUserId}");
                    results.Add($"✅ Added test water for User {otherUserId}");
                }
                catch (Exception ex)
                {
                    results.Add($"❌ Failed to add water for User {otherUserId}: {ex.Message}");
                }
                
                // Test 3: Check what current user sees
                try
                {
                    var currentUserWater = await _waterTrackingService.GetDailyWaterIntakeAsync(currentUserId, DateTime.Today);
                    results.Add($"✅ User {currentUserId} sees: {currentUserWater}ml water today");
                }
                catch (Exception ex)
                {
                    results.Add($"❌ Failed to get water for current user: {ex.Message}");
                }
                
                // Test 4: Check what other user would see
                try
                {
                    var otherUserWater = await _waterTrackingService.GetDailyWaterIntakeAsync(otherUserId, DateTime.Today);
                    results.Add($"✅ User {otherUserId} sees: {otherUserWater}ml water today");
                }
                catch (Exception ex)
                {
                    results.Add($"❌ Failed to get water for other user: {ex.Message}");
                }
                
                // Test 5: Verify database isolation at raw level
                try
                {
                    var currentUserEntries = await _dbContext.UserWaterIntake
                        .Where(w => w.UserId == currentUserId && w.ConsumedAt.Date == DateTime.Today)
                        .ToListAsync();
                    
                    var otherUserEntries = await _dbContext.UserWaterIntake
                        .Where(w => w.UserId == otherUserId && w.ConsumedAt.Date == DateTime.Today)
                        .ToListAsync();
                    
                    results.Add($"✅ Database isolation: User {currentUserId} has {currentUserEntries.Count} entries, User {otherUserId} has {otherUserEntries.Count} entries");
                }
                catch (Exception ex)
                {
                    results.Add($"❌ Database isolation test failed: {ex.Message}");
                }
                
                // Test 6: Test meal data isolation
                try
                {
                    var currentUserMeals = await _nutritionService.GetMealLogsAsync(currentUserId, DateTime.Today);
                    var otherUserMeals = await _nutritionService.GetMealLogsAsync(otherUserId, DateTime.Today);
                    
                    results.Add($"✅ Meal isolation: User {currentUserId} has {currentUserMeals.Count()} meals, User {otherUserId} has {otherUserMeals.Count()} meals");
                }
                catch (Exception ex)
                {
                    results.Add($"❌ Meal isolation test failed: {ex.Message}");
                }
                
                return Json(new { 
                    success = true, 
                    currentUser = currentUserId,
                    message = "User data isolation test completed",
                    results = results 
                });
            }
            catch (Exception ex)
            {
                return Json(new { 
                    success = false, 
                    message = $"User data isolation test failed: {ex.Message}",
                    innerException = ex.InnerException?.Message 
                });
            }
        }

        /// <summary>
        /// Complete diagnostic test for water persistence issues.
        /// Tests the full cycle: add water → save to DB → retrieve from DB
        /// </summary>
        /// <returns>JSON response with detailed step-by-step diagnostic results.</returns>
        [HttpGet]
        public async Task<IActionResult> DiagnoseWaterPersistence()
        {
            try
            {
                var results = new List<string>();
                
                // Get current user from session
                int userId = int.Parse(HttpContext.Session.GetString("UserId"));
                var today = DateTime.Today;
                
                results.Add($"=== DIAGNOSING WATER PERSISTENCE FOR USER {userId} ===");
                results.Add($"Test Date: {today:yyyy-MM-dd}");
                
                // Step 1: Check initial state
                results.Add("--- STEP 1: Initial State ---");
                try
                {
                    var initialWater = await _waterTrackingService.GetDailyWaterIntakeAsync(userId, today);
                    results.Add($"✅ Initial water for user {userId}: {initialWater}ml");
                    
                    var initialEntries = await _dbContext.UserWaterIntake
                        .Where(w => w.UserId == userId && w.ConsumedAt.Date == today)
                        .ToListAsync();
                    results.Add($"✅ Initial DB entries: {initialEntries.Count} entries");
                    foreach(var entry in initialEntries)
                    {
                        results.Add($"   - Entry {entry.Id}: {entry.AmountMl}ml at {entry.ConsumedAt:HH:mm}");
                    }
                }
                catch (Exception ex)
                {
                    results.Add($"❌ Initial state check failed: {ex.Message}");
                }
                
                // Step 2: Add test water via service
                results.Add("--- STEP 2: Adding Water via Service ---");
                var testAmount = 150;
                UserWaterIntakeModel? createdEntry = null;
                try
                {
                    createdEntry = await _waterTrackingService.AddWaterIntakeAsync(userId, testAmount, "Diagnostic test");
                    results.Add($"✅ Service returned entry: ID={createdEntry.Id}, Amount={createdEntry.AmountMl}ml");
                }
                catch (Exception ex)
                {
                    results.Add($"❌ Service add failed: {ex.Message}");
                    if (ex.InnerException != null)
                        results.Add($"   Inner: {ex.InnerException.Message}");
                }
                
                // Step 3: Immediate verification in database
                results.Add("--- STEP 3: Immediate DB Verification ---");
                try
                {
                    var allEntries = await _dbContext.UserWaterIntake
                        .Where(w => w.UserId == userId && w.ConsumedAt.Date == today)
                        .ToListAsync();
                    results.Add($"✅ DB entries after add: {allEntries.Count} entries");
                    foreach(var entry in allEntries)
                    {
                        results.Add($"   - Entry {entry.Id}: {entry.AmountMl}ml at {entry.ConsumedAt:HH:mm}");
                    }
                    
                    var totalFromDb = allEntries.Sum(e => e.AmountMl);
                    results.Add($"✅ Total from direct DB query: {totalFromDb}ml");
                }
                catch (Exception ex)
                {
                    results.Add($"❌ DB verification failed: {ex.Message}");
                }
                
                // Step 4: Service retrieval test
                results.Add("--- STEP 4: Service Retrieval Test ---");
                try
                {
                    var serviceTotal = await _waterTrackingService.GetDailyWaterIntakeAsync(userId, today);
                    results.Add($"✅ Service retrieval: {serviceTotal}ml");
                    
                    var serviceEntries = await _waterTrackingService.GetWaterIntakeEntriesAsync(userId, today);
                    results.Add($"✅ Service entries: {serviceEntries.Count()} entries");
                    foreach(var entry in serviceEntries)
                    {
                        results.Add($"   - Entry {entry.Id}: {entry.AmountMl}ml at {entry.ConsumedAt:HH:mm}");
                    }
                }
                catch (Exception ex)
                {
                    results.Add($"❌ Service retrieval failed: {ex.Message}");
                }
                
                // Step 5: Repository-level test
                results.Add("--- STEP 5: Repository-Level Test ---");
                try
                {
                    var repoEntries = await _dbContext.UserWaterIntake
                        .Where(w => w.UserId == userId && w.ConsumedAt.Date == today)
                        .ToListAsync();
                    results.Add($"✅ Repository query: {repoEntries.Count} entries");
                    var repoTotal = repoEntries.Sum(e => e.AmountMl);
                    results.Add($"✅ Repository total: {repoTotal}ml");
                }
                catch (Exception ex)
                {
                    results.Add($"❌ Repository test failed: {ex.Message}");
                }
                
                // Step 6: Test with different user to verify isolation
                results.Add("--- STEP 6: User Isolation Test ---");
                try
                {
                    int otherUserId = userId == 1 ? 2 : 1;
                    var otherUserWater = await _waterTrackingService.GetDailyWaterIntakeAsync(otherUserId, today);
                    results.Add($"✅ Other user ({otherUserId}) water: {otherUserWater}ml");
                }
                catch (Exception ex)
                {
                    results.Add($"❌ Other user test failed: {ex.Message}");
                }
                
                // Clean up the test entry if it was created
                if (createdEntry != null && createdEntry.Id > 0)
                {
                    try
                    {
                        await _waterTrackingService.DeleteWaterIntakeAsync(userId, createdEntry.Id);
                        results.Add($"✅ Cleaned up test entry {createdEntry.Id}");
                    }
                    catch (Exception ex)
                    {
                        results.Add($"⚠️ Cleanup failed: {ex.Message}");
                    }
                }
                
                return Json(new { 
                    success = true, 
                    userId = userId,
                    testDate = today.ToString("yyyy-MM-dd"),
                    results = results 
                });
            }
            catch (Exception ex)
            {
                return Json(new { 
                    success = false, 
                    message = $"Diagnostic test failed: {ex.Message}",
                    innerException = ex.InnerException?.Message 
                });
            }
        }
    }
} 
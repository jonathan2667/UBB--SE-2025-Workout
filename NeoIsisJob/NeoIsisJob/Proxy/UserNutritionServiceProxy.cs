using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Workout.Core.Models;

namespace NeoIsisJob.Proxy
{
    /// <summary>
    /// Service proxy for user nutrition operations that communicates with the API.
    /// </summary>
    public class UserNutritionServiceProxy : BaseServiceProxy
    {
        private readonly string apiEndpoint = "UserNutrition";

        /// <summary>
        /// Initializes a new instance of the <see cref="UserNutritionServiceProxy"/> class.
        /// </summary>
        public UserNutritionServiceProxy() : base()
        {
        }

        /// <summary>
        /// Gets the daily nutrition summary for a user on a specific date.
        /// </summary>
        /// <param name="userId">The user identifier.</param>
        /// <param name="date">The date to get nutrition data for.</param>
        /// <returns>The daily nutrition summary.</returns>
        public async Task<UserDailyNutritionModel> GetDailyNutritionAsync(int userId, DateTime date)
        {
            try
            {
                var dateString = date.ToString("yyyy-MM-dd");
                var response = await httpClient.GetAsync($"{apiEndpoint}/{userId}/daily/{dateString}");
                
                if (response.IsSuccessStatusCode)
                {
                    var jsonString = await response.Content.ReadAsStringAsync();
                    var nutrition = JsonSerializer.Deserialize<UserDailyNutritionModel>(jsonString, jsonOptions);
                    return nutrition ?? new UserDailyNutritionModel { UserId = userId, Date = date };
                }
                
                return new UserDailyNutritionModel { UserId = userId, Date = date };
            }
            catch (HttpRequestException ex)
            {
                System.Diagnostics.Debug.WriteLine($"HTTP error getting daily nutrition: {ex.Message}");
                return new UserDailyNutritionModel { UserId = userId, Date = date };
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error getting daily nutrition: {ex.Message}");
                return new UserDailyNutritionModel { UserId = userId, Date = date };
            }
        }

        /// <summary>
        /// Gets nutrition data for a user over a specified number of days.
        /// </summary>
        /// <param name="userId">The user identifier.</param>
        /// <param name="days">The number of days to retrieve data for.</param>
        /// <returns>A collection of daily nutrition summaries.</returns>
        public async Task<IEnumerable<UserDailyNutritionModel>> GetNutritionDataAsync(int userId, int days)
        {
            try
            {
                var response = await httpClient.GetAsync($"{apiEndpoint}/{userId}/data/{days}");
                
                if (response.IsSuccessStatusCode)
                {
                    var jsonString = await response.Content.ReadAsStringAsync();
                    var nutritionData = JsonSerializer.Deserialize<IEnumerable<UserDailyNutritionModel>>(jsonString, jsonOptions);
                    return nutritionData ?? new List<UserDailyNutritionModel>();
                }
                
                return new List<UserDailyNutritionModel>();
            }
            catch (HttpRequestException ex)
            {
                System.Diagnostics.Debug.WriteLine($"HTTP error getting nutrition data: {ex.Message}");
                return new List<UserDailyNutritionModel>();
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error getting nutrition data: {ex.Message}");
                return new List<UserDailyNutritionModel>();
            }
        }

        /// <summary>
        /// Logs a meal as consumed by a user.
        /// </summary>
        /// <param name="userId">The user identifier.</param>
        /// <param name="mealId">The meal identifier.</param>
        /// <param name="portionMultiplier">The portion size multiplier.</param>
        /// <param name="notes">Optional notes about the meal consumption.</param>
        /// <returns>The created meal log entry.</returns>
        public async Task<UserMealLogModel> LogMealAsync(int userId, int mealId, double portionMultiplier = 1.0, string notes = null)
        {
            try
            {
                var logMealRequest = new
                {
                    MealId = mealId,
                    PortionMultiplier = portionMultiplier,
                    Notes = notes
                };

                var jsonContent = JsonSerializer.Serialize(logMealRequest, jsonOptions);
                var content = new StringContent(jsonContent, Encoding.UTF8, "application/json");

                var response = await httpClient.PostAsync($"{apiEndpoint}/{userId}/logmeal", content);
                
                if (response.IsSuccessStatusCode)
                {
                    var jsonString = await response.Content.ReadAsStringAsync();
                    var mealLog = JsonSerializer.Deserialize<UserMealLogModel>(jsonString, jsonOptions);
                    return mealLog;
                }
                
                throw new InvalidOperationException($"Failed to log meal. Status: {response.StatusCode}");
            }
            catch (HttpRequestException ex)
            {
                System.Diagnostics.Debug.WriteLine($"HTTP error logging meal: {ex.Message}");
                throw new InvalidOperationException("Failed to log meal due to network error.");
            }
        }

        /// <summary>
        /// Gets all meals logged by a user on a specific date.
        /// </summary>
        /// <param name="userId">The user identifier.</param>
        /// <param name="date">The date to get meal logs for.</param>
        /// <returns>A collection of meal log entries.</returns>
        public async Task<IEnumerable<UserMealLogModel>> GetMealLogsAsync(int userId, DateTime date)
        {
            try
            {
                var dateString = date.ToString("yyyy-MM-dd");
                var response = await httpClient.GetAsync($"{apiEndpoint}/{userId}/meallogs/{dateString}");
                
                if (response.IsSuccessStatusCode)
                {
                    var jsonString = await response.Content.ReadAsStringAsync();
                    var mealLogs = JsonSerializer.Deserialize<IEnumerable<UserMealLogModel>>(jsonString, jsonOptions);
                    return mealLogs ?? new List<UserMealLogModel>();
                }
                
                return new List<UserMealLogModel>();
            }
            catch (HttpRequestException ex)
            {
                System.Diagnostics.Debug.WriteLine($"HTTP error getting meal logs: {ex.Message}");
                return new List<UserMealLogModel>();
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error getting meal logs: {ex.Message}");
                return new List<UserMealLogModel>();
            }
        }

        /// <summary>
        /// Calculates weekly nutrition averages for a user.
        /// </summary>
        /// <param name="userId">The user identifier.</param>
        /// <param name="weekStartDate">The start date of the week.</param>
        /// <returns>The average daily nutrition for the week.</returns>
        public async Task<UserDailyNutritionModel> GetWeeklyAverageAsync(int userId, DateTime weekStartDate)
        {
            try
            {
                var dateString = weekStartDate.ToString("yyyy-MM-dd");
                var response = await httpClient.GetAsync($"{apiEndpoint}/{userId}/weekly/{dateString}");
                
                if (response.IsSuccessStatusCode)
                {
                    var jsonString = await response.Content.ReadAsStringAsync();
                    var weeklyAverage = JsonSerializer.Deserialize<UserDailyNutritionModel>(jsonString, jsonOptions);
                    return weeklyAverage ?? new UserDailyNutritionModel { UserId = userId, Date = weekStartDate };
                }
                
                return new UserDailyNutritionModel { UserId = userId, Date = weekStartDate };
            }
            catch (HttpRequestException ex)
            {
                System.Diagnostics.Debug.WriteLine($"HTTP error getting weekly average: {ex.Message}");
                return new UserDailyNutritionModel { UserId = userId, Date = weekStartDate };
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error getting weekly average: {ex.Message}");
                return new UserDailyNutritionModel { UserId = userId, Date = weekStartDate };
            }
        }

        /// <summary>
        /// Calculates monthly nutrition averages for a user.
        /// </summary>
        /// <param name="userId">The user identifier.</param>
        /// <param name="month">The month to calculate averages for.</param>
        /// <param name="year">The year to calculate averages for.</param>
        /// <returns>The average daily nutrition for the month.</returns>
        public async Task<UserDailyNutritionModel> GetMonthlyAverageAsync(int userId, int month, int year)
        {
            try
            {
                var response = await httpClient.GetAsync($"{apiEndpoint}/{userId}/monthly/{year}/{month}");
                
                if (response.IsSuccessStatusCode)
                {
                    var jsonString = await response.Content.ReadAsStringAsync();
                    var monthlyAverage = JsonSerializer.Deserialize<UserDailyNutritionModel>(jsonString, jsonOptions);
                    return monthlyAverage ?? new UserDailyNutritionModel { UserId = userId, Date = new DateTime(year, month, 1) };
                }
                
                return new UserDailyNutritionModel { UserId = userId, Date = new DateTime(year, month, 1) };
            }
            catch (HttpRequestException ex)
            {
                System.Diagnostics.Debug.WriteLine($"HTTP error getting monthly average: {ex.Message}");
                return new UserDailyNutritionModel { UserId = userId, Date = new DateTime(year, month, 1) };
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error getting monthly average: {ex.Message}");
                return new UserDailyNutritionModel { UserId = userId, Date = new DateTime(year, month, 1) };
            }
        }

        /// <summary>
        /// Gets the top meal types consumed by a user in the last specified days.
        /// </summary>
        /// <param name="userId">The user identifier.</param>
        /// <param name="days">The number of days to analyze (default is 30).</param>
        /// <returns>A dictionary of meal types and their consumption counts, ordered by frequency.</returns>
        public async Task<Dictionary<string, int>> GetTopMealTypesAsync(int userId, int days = 30)
        {
            try
            {
                var response = await httpClient.GetAsync($"{apiEndpoint}/{userId}/topmealtypes/{days}");
                
                if (response.IsSuccessStatusCode)
                {
                    var jsonString = await response.Content.ReadAsStringAsync();
                    var topMealTypes = JsonSerializer.Deserialize<Dictionary<string, int>>(jsonString, jsonOptions);
                    return topMealTypes ?? new Dictionary<string, int>();
                }
                
                return new Dictionary<string, int>();
            }
            catch (HttpRequestException ex)
            {
                System.Diagnostics.Debug.WriteLine($"HTTP error getting top meal types: {ex.Message}");
                return new Dictionary<string, int>();
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error getting top meal types: {ex.Message}");
                return new Dictionary<string, int>();
            }
        }

        /// <summary>
        /// Gets today's nutrition for a user.
        /// </summary>
        /// <param name="userId">The user identifier.</param>
        /// <returns>Today's nutrition summary.</returns>
        public async Task<UserDailyNutritionModel> GetTodayNutritionAsync(int userId)
        {
            try
            {
                var response = await httpClient.GetAsync($"{apiEndpoint}/{userId}/today");
                
                if (response.IsSuccessStatusCode)
                {
                    var jsonString = await response.Content.ReadAsStringAsync();
                    var todayNutrition = JsonSerializer.Deserialize<UserDailyNutritionModel>(jsonString, jsonOptions);
                    return todayNutrition ?? new UserDailyNutritionModel { UserId = userId, Date = DateTime.Today };
                }
                
                return new UserDailyNutritionModel { UserId = userId, Date = DateTime.Today };
            }
            catch (HttpRequestException ex)
            {
                System.Diagnostics.Debug.WriteLine($"HTTP error getting today's nutrition: {ex.Message}");
                return new UserDailyNutritionModel { UserId = userId, Date = DateTime.Today };
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error getting today's nutrition: {ex.Message}");
                return new UserDailyNutritionModel { UserId = userId, Date = DateTime.Today };
            }
        }
    }
} 
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
    /// Service proxy for water tracking operations that communicates with the API.
    /// </summary>
    public class WaterTrackingServiceProxy : BaseServiceProxy
    {
        private readonly string apiEndpoint = "WaterTracking";

        /// <summary>
        /// Initializes a new instance of the <see cref="WaterTrackingServiceProxy"/> class.
        /// </summary>
        public WaterTrackingServiceProxy() : base()
        {
        }

        /// <summary>
        /// Gets daily water intake for a user on a specific date.
        /// </summary>
        /// <param name="userId">The user identifier.</param>
        /// <param name="date">The date to get water intake for.</param>
        /// <returns>The daily water intake amount in milliliters.</returns>
        public async Task<int> GetDailyWaterIntakeAsync(int userId, DateTime date)
        {
            try
            {
                var dateString = date.ToString("yyyy-MM-dd");
                var response = await httpClient.GetAsync($"{apiEndpoint}/{userId}/daily/{dateString}");
                
                if (response.IsSuccessStatusCode)
                {
                    var jsonString = await response.Content.ReadAsStringAsync();
                    var waterIntake = JsonSerializer.Deserialize<int>(jsonString, jsonOptions);
                    return waterIntake;
                }
                
                return 0;
            }
            catch (HttpRequestException ex)
            {
                System.Diagnostics.Debug.WriteLine($"HTTP error getting daily water intake: {ex.Message}");
                return 0;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error getting daily water intake: {ex.Message}");
                return 0;
            }
        }

        /// <summary>
        /// Adds water intake for a user.
        /// </summary>
        /// <param name="userId">The user identifier.</param>
        /// <param name="amountMl">The amount of water in milliliters.</param>
        /// <param name="notes">Optional notes about the water intake.</param>
        /// <returns>The created water intake entry.</returns>
        public async Task<UserWaterIntakeModel> AddWaterIntakeAsync(int userId, int amountMl, string notes = null)
        {
            try
            {
                var waterIntakeRequest = new
                {
                    AmountMl = amountMl,
                    Notes = notes
                };

                var jsonContent = JsonSerializer.Serialize(waterIntakeRequest, jsonOptions);
                var content = new StringContent(jsonContent, Encoding.UTF8, "application/json");

                var response = await httpClient.PostAsync($"{apiEndpoint}/{userId}/add", content);
                
                if (response.IsSuccessStatusCode)
                {
                    var jsonString = await response.Content.ReadAsStringAsync();
                    var waterEntry = JsonSerializer.Deserialize<UserWaterIntakeModel>(jsonString, jsonOptions);
                    return waterEntry;
                }
                
                throw new InvalidOperationException($"Failed to add water intake. Status: {response.StatusCode}");
            }
            catch (HttpRequestException ex)
            {
                System.Diagnostics.Debug.WriteLine($"HTTP error adding water intake: {ex.Message}");
                throw new InvalidOperationException("Failed to add water intake due to network error.");
            }
        }

        /// <summary>
        /// Gets the water goal for a user.
        /// </summary>
        /// <param name="userId">The user identifier.</param>
        /// <returns>The water goal in milliliters.</returns>
        public async Task<int> GetWaterGoalAsync(int userId)
        {
            try
            {
                var response = await httpClient.GetAsync($"{apiEndpoint}/{userId}/goal");
                
                if (response.IsSuccessStatusCode)
                {
                    var jsonString = await response.Content.ReadAsStringAsync();
                    var goal = JsonSerializer.Deserialize<int>(jsonString, jsonOptions);
                    return goal;
                }
                
                return 2000; // Default water goal
            }
            catch (HttpRequestException ex)
            {
                System.Diagnostics.Debug.WriteLine($"HTTP error getting water goal: {ex.Message}");
                return 2000;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error getting water goal: {ex.Message}");
                return 2000;
            }
        }

        /// <summary>
        /// Sets the water goal for a user.
        /// </summary>
        /// <param name="userId">The user identifier.</param>
        /// <param name="goalMl">The water goal in milliliters.</param>
        /// <returns>True if successful, false otherwise.</returns>
        public async Task<bool> SetWaterGoalAsync(int userId, int goalMl)
        {
            try
            {
                var goalRequest = new
                {
                    GoalMl = goalMl
                };

                var jsonContent = JsonSerializer.Serialize(goalRequest, jsonOptions);
                var content = new StringContent(jsonContent, Encoding.UTF8, "application/json");

                var response = await httpClient.PostAsync($"{apiEndpoint}/{userId}/goal", content);
                return response.IsSuccessStatusCode;
            }
            catch (HttpRequestException ex)
            {
                System.Diagnostics.Debug.WriteLine($"HTTP error setting water goal: {ex.Message}");
                return false;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error setting water goal: {ex.Message}");
                return false;
            }
        }

        /// <summary>
        /// Gets water intake progress for a user on a specific date.
        /// </summary>
        /// <param name="userId">The user identifier.</param>
        /// <param name="date">The date to get progress for.</param>
        /// <returns>The water intake progress as a percentage.</returns>
        public async Task<double> GetWaterIntakeProgressAsync(int userId, DateTime date)
        {
            try
            {
                var dateString = date.ToString("yyyy-MM-dd");
                var response = await httpClient.GetAsync($"{apiEndpoint}/{userId}/progress/{dateString}");
                
                if (response.IsSuccessStatusCode)
                {
                    var jsonString = await response.Content.ReadAsStringAsync();
                    var progress = JsonSerializer.Deserialize<double>(jsonString, jsonOptions);
                    return progress;
                }
                
                return 0.0;
            }
            catch (HttpRequestException ex)
            {
                System.Diagnostics.Debug.WriteLine($"HTTP error getting water intake progress: {ex.Message}");
                return 0.0;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error getting water intake progress: {ex.Message}");
                return 0.0;
            }
        }

        /// <summary>
        /// Gets water intake history for a user over a specified number of days.
        /// </summary>
        /// <param name="userId">The user identifier.</param>
        /// <param name="days">The number of days to retrieve history for.</param>
        /// <returns>A dictionary of dates and water intake amounts.</returns>
        public async Task<Dictionary<DateTime, int>> GetWaterIntakeHistoryAsync(int userId, int days)
        {
            try
            {
                var response = await httpClient.GetAsync($"{apiEndpoint}/{userId}/history/{days}");
                
                if (response.IsSuccessStatusCode)
                {
                    var jsonString = await response.Content.ReadAsStringAsync();
                    var history = JsonSerializer.Deserialize<Dictionary<DateTime, int>>(jsonString, jsonOptions);
                    return history ?? new Dictionary<DateTime, int>();
                }
                
                return new Dictionary<DateTime, int>();
            }
            catch (HttpRequestException ex)
            {
                System.Diagnostics.Debug.WriteLine($"HTTP error getting water intake history: {ex.Message}");
                return new Dictionary<DateTime, int>();
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error getting water intake history: {ex.Message}");
                return new Dictionary<DateTime, int>();
            }
        }

        /// <summary>
        /// Gets water intake entries for a user on a specific date.
        /// </summary>
        /// <param name="userId">The user identifier.</param>
        /// <param name="date">The date to get entries for.</param>
        /// <returns>A collection of water intake entries.</returns>
        public async Task<IEnumerable<UserWaterIntakeModel>> GetWaterIntakeEntriesAsync(int userId, DateTime date)
        {
            try
            {
                var dateString = date.ToString("yyyy-MM-dd");
                var response = await httpClient.GetAsync($"{apiEndpoint}/{userId}/entries/{dateString}");
                
                if (response.IsSuccessStatusCode)
                {
                    var jsonString = await response.Content.ReadAsStringAsync();
                    var entries = JsonSerializer.Deserialize<IEnumerable<UserWaterIntakeModel>>(jsonString, jsonOptions);
                    return entries ?? new List<UserWaterIntakeModel>();
                }
                
                return new List<UserWaterIntakeModel>();
            }
            catch (HttpRequestException ex)
            {
                System.Diagnostics.Debug.WriteLine($"HTTP error getting water intake entries: {ex.Message}");
                return new List<UserWaterIntakeModel>();
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error getting water intake entries: {ex.Message}");
                return new List<UserWaterIntakeModel>();
            }
        }

        /// <summary>
        /// Gets today's water intake for a user.
        /// </summary>
        /// <param name="userId">The user identifier.</param>
        /// <returns>Today's water intake amount in milliliters.</returns>
        public async Task<int> GetTodayWaterIntakeAsync(int userId)
        {
            try
            {
                var response = await httpClient.GetAsync($"{apiEndpoint}/{userId}/today");
                
                if (response.IsSuccessStatusCode)
                {
                    var jsonString = await response.Content.ReadAsStringAsync();
                    var todayIntake = JsonSerializer.Deserialize<int>(jsonString, jsonOptions);
                    return todayIntake;
                }
                
                return 0;
            }
            catch (HttpRequestException ex)
            {
                System.Diagnostics.Debug.WriteLine($"HTTP error getting today's water intake: {ex.Message}");
                return 0;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error getting today's water intake: {ex.Message}");
                return 0;
            }
        }
    }
} 
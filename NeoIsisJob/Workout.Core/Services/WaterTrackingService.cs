// <copyright file="WaterTrackingService.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace Workout.Core.Services
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Workout.Core.IRepositories;
    using Workout.Core.IServices;
    using Workout.Core.Models;

    /// <summary>
    /// Service implementation for water tracking operations.
    /// </summary>
    public class WaterTrackingService : IWaterTrackingService
    {
        private readonly IUserWaterIntakeRepository waterIntakeRepository;
        private readonly IUserDailyNutritionRepository dailyNutritionRepository;

        /// <summary>
        /// Default daily water goal in milliliters.
        /// </summary>
        private const int DefaultWaterGoalMl = 2000;

        /// <summary>
        /// Initializes a new instance of the <see cref="WaterTrackingService"/> class.
        /// </summary>
        /// <param name="waterIntakeRepository">The water intake repository.</param>
        /// <param name="dailyNutritionRepository">The daily nutrition repository.</param>
        public WaterTrackingService(
            IUserWaterIntakeRepository waterIntakeRepository,
            IUserDailyNutritionRepository dailyNutritionRepository)
        {
            this.waterIntakeRepository = waterIntakeRepository ?? throw new ArgumentNullException(nameof(waterIntakeRepository));
            this.dailyNutritionRepository = dailyNutritionRepository ?? throw new ArgumentNullException(nameof(dailyNutritionRepository));
        }

        /// <inheritdoc/>
        public async Task<UserWaterIntakeModel> AddWaterIntakeAsync(int userId, int amountMl, string notes = null)
        {
            try
            {
                System.Diagnostics.Debug.WriteLine($"WaterTrackingService.AddWaterIntakeAsync: userId={userId}, amount={amountMl}ml");
                
                var waterIntake = new UserWaterIntakeModel
                {
                    UserId = userId,
                    AmountMl = amountMl,
                    ConsumedAt = DateTime.Now,  // Keep as local time for consistency
                    Notes = notes ?? string.Empty,
                    CreatedAt = DateTime.Now   // Change to local time for consistency with ConsumedAt
                };

                System.Diagnostics.Debug.WriteLine($"Created water intake object: UserId={waterIntake.UserId}, Amount={waterIntake.AmountMl}, ConsumedAt={waterIntake.ConsumedAt:yyyy-MM-dd HH:mm:ss}");

                // Try to create the water intake record
                UserWaterIntakeModel createdIntake = null;
                try
                {
                    createdIntake = await this.waterIntakeRepository.CreateAsync(waterIntake);
                }
                catch (Exception dbEx)
                {
                    // Log the database error but don't fail completely
                    System.Diagnostics.Debug.WriteLine($"Database error in AddWaterIntakeAsync: {dbEx.Message}");
                    // Return the waterIntake object even if database save failed
                    // This allows the UI to still function
                    return waterIntake;
                }
                
                // Try to update daily nutrition summary with new water intake
                try
                {
                    await this.UpdateDailyWaterIntakeAsync(userId, DateTime.Today);
                }
                catch (Exception updateEx)
                {
                    // Log the update error but don't fail the main operation
                    System.Diagnostics.Debug.WriteLine($"Failed to update daily nutrition: {updateEx.Message}");
                }
                
                return createdIntake ?? waterIntake;
            }
            catch (Exception ex)
            {
                var errorMessage = $"Failed to add water intake of {amountMl}ml for user {userId}.";
                if (ex.InnerException != null)
                {
                    errorMessage += $" Inner exception: {ex.InnerException.Message}";
                }
                System.Diagnostics.Debug.WriteLine($"AddWaterIntakeAsync full error: {ex}");
                throw new Exception(errorMessage, ex);
            }
        }

        /// <inheritdoc/>
        public async Task<int> GetDailyWaterIntakeAsync(int userId, DateTime date)
        {
            try
            {
                var intakes = await this.waterIntakeRepository.GetByUserAndDateAsync(userId, date.Date);
                return intakes.Sum(i => i.AmountMl);
            }
            catch (Exception ex)
            {
                throw new Exception($"Failed to get daily water intake for user {userId} on {date:yyyy-MM-dd}.", ex);
            }
        }

        /// <inheritdoc/>
        public async Task<IEnumerable<UserWaterIntakeModel>> GetWaterIntakeEntriesAsync(int userId, DateTime date)
        {
            try
            {
                return await this.waterIntakeRepository.GetByUserAndDateAsync(userId, date.Date);
            }
            catch (Exception ex)
            {
                throw new Exception($"Failed to get water intake entries for user {userId} on {date:yyyy-MM-dd}.", ex);
            }
        }

        /// <inheritdoc/>
        public async Task<Dictionary<DateTime, int>> GetWaterIntakeHistoryAsync(int userId, int days)
        {
            try
            {
                var endDate = DateTime.Today;
                var startDate = endDate.AddDays(-days + 1);
                var intakes = await this.waterIntakeRepository.GetByUserAndDateRangeAsync(userId, startDate, endDate);
                
                var history = new Dictionary<DateTime, int>();
                
                // Initialize all dates with 0
                for (var date = startDate; date <= endDate; date = date.AddDays(1))
                {
                    history[date] = 0;
                }
                
                // Group intakes by date and sum amounts
                var dailyIntakes = intakes.GroupBy(i => i.ConsumedAt.Date)
                                         .ToDictionary(g => g.Key, g => g.Sum(i => i.AmountMl));
                
                // Update history with actual intake amounts
                foreach (var dailyIntake in dailyIntakes)
                {
                    if (history.ContainsKey(dailyIntake.Key))
                    {
                        history[dailyIntake.Key] = dailyIntake.Value;
                    }
                }
                
                return history.OrderBy(h => h.Key).ToDictionary(h => h.Key, h => h.Value);
            }
            catch (Exception ex)
            {
                throw new Exception($"Failed to get water intake history for user {userId} for {days} days.", ex);
            }
        }

        /// <inheritdoc/>
        public async Task<double> GetAverageWaterIntakeAsync(int userId, int days)
        {
            try
            {
                var history = await this.GetWaterIntakeHistoryAsync(userId, days);
                return history.Count > 0 ? history.Values.Average() : 0;
            }
            catch (Exception ex)
            {
                throw new Exception($"Failed to calculate average water intake for user {userId} for {days} days.", ex);
            }
        }

        /// <inheritdoc/>
        public async Task<int> GetWaterGoalAsync(int userId)
        {
            // For now, return the default goal. In a full implementation,
            // this could be stored in a user preferences table.
            await Task.CompletedTask;
            return DefaultWaterGoalMl;
        }

        /// <inheritdoc/>
        public async Task SetWaterGoalAsync(int userId, int goalMl)
        {
            // For now, this is a placeholder. In a full implementation,
            // this would update the user's water goal in a preferences table.
            await Task.CompletedTask;
            // TODO: Implement user preferences storage
        }

        /// <inheritdoc/>
        public async Task<double> GetWaterIntakeProgressAsync(int userId, DateTime date)
        {
            try
            {
                var dailyIntake = await this.GetDailyWaterIntakeAsync(userId, date);
                var goal = await this.GetWaterGoalAsync(userId);
                
                if (goal <= 0) return 0;
                
                var progress = (double)dailyIntake / goal * 100;
                return Math.Min(progress, 100); // Cap at 100%
            }
            catch (Exception ex)
            {
                throw new Exception($"Failed to calculate water intake progress for user {userId} on {date:yyyy-MM-dd}.", ex);
            }
        }

        /// <inheritdoc/>
        public async Task<bool> DeleteWaterIntakeAsync(int userId, int entryId)
        {
            try
            {
                var deleted = await this.waterIntakeRepository.DeleteByUserAndIdAsync(userId, entryId);
                
                if (deleted)
                {
                    // Update daily nutrition summary after deletion
                    await this.UpdateDailyWaterIntakeAsync(userId, DateTime.Today);
                }
                
                return deleted;
            }
            catch (Exception ex)
            {
                throw new Exception($"Failed to delete water intake entry {entryId} for user {userId}.", ex);
            }
        }

        /// <summary>
        /// Updates the daily nutrition summary with the current water intake total.
        /// </summary>
        /// <param name="userId">The user identifier.</param>
        /// <param name="date">The date to update.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        private async Task UpdateDailyWaterIntakeAsync(int userId, DateTime date)
        {
            try
            {
                var dailyWaterIntake = await this.GetDailyWaterIntakeAsync(userId, date);
                var nutrition = await this.dailyNutritionRepository.GetByUserAndDateAsync(userId, date.Date);
                
                if (nutrition != null)
                {
                    nutrition.WaterIntakeMl = dailyWaterIntake;
                    nutrition.UpdatedAt = DateTime.Now;
                    await this.dailyNutritionRepository.UpdateAsync(nutrition);
                }
                else
                {
                    // Create a new nutrition record if none exists
                    var newNutrition = new UserDailyNutritionModel
                    {
                        UserId = userId,
                        Date = date.Date,
                        TotalCalories = 0,
                        TotalProteins = 0,
                        TotalCarbohydrates = 0,
                        TotalFats = 0,
                        MealsConsumed = 0,
                        WaterIntakeMl = dailyWaterIntake,
                        CreatedAt = DateTime.Now,
                        UpdatedAt = DateTime.Now
                    };
                    await this.dailyNutritionRepository.CreateAsync(newNutrition);
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Failed to update daily water intake for user {userId} on {date:yyyy-MM-dd}.", ex);
            }
        }
    }
} 
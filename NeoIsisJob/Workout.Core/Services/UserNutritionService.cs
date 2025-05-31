// <copyright file="UserNutritionService.cs" company="PlaceholderCompany">
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
    /// Service implementation for user nutrition operations.
    /// </summary>
    public class UserNutritionService : IUserNutritionService
    {
        private readonly IUserDailyNutritionRepository dailyNutritionRepository;
        private readonly IUserMealLogRepository mealLogRepository;
        private readonly IRepository<MealModel> mealRepository;

        /// <summary>
        /// Initializes a new instance of the <see cref="UserNutritionService"/> class.
        /// </summary>
        /// <param name="dailyNutritionRepository">The daily nutrition repository.</param>
        /// <param name="mealLogRepository">The meal log repository.</param>
        /// <param name="mealRepository">The meal repository.</param>
        public UserNutritionService(
            IUserDailyNutritionRepository dailyNutritionRepository,
            IUserMealLogRepository mealLogRepository,
            IRepository<MealModel> mealRepository)
        {
            this.dailyNutritionRepository = dailyNutritionRepository ?? throw new ArgumentNullException(nameof(dailyNutritionRepository));
            this.mealLogRepository = mealLogRepository ?? throw new ArgumentNullException(nameof(mealLogRepository));
            this.mealRepository = mealRepository ?? throw new ArgumentNullException(nameof(mealRepository));
        }

        /// <inheritdoc/>
        public async Task<UserDailyNutritionModel> GetDailyNutritionAsync(int userId, DateTime date)
        {
            try
            {
                var nutrition = await this.dailyNutritionRepository.GetByUserAndDateAsync(userId, date.Date);
                if (nutrition == null)
                {
                    // Create and return a new daily nutrition record if none exists
                    return await this.UpdateDailyNutritionAsync(userId, date.Date);
                }
                return nutrition;
            }
            catch (Exception ex)
            {
                throw new Exception($"Failed to get daily nutrition for user {userId} on {date:yyyy-MM-dd}.", ex);
            }
        }

        /// <inheritdoc/>
        public async Task<IEnumerable<UserDailyNutritionModel>> GetNutritionDataAsync(int userId, int days)
        {
            try
            {
                var endDate = DateTime.Today;
                var startDate = endDate.AddDays(-days + 1);
                return await this.dailyNutritionRepository.GetByUserAndDateRangeAsync(userId, startDate, endDate);
            }
            catch (Exception ex)
            {
                throw new Exception($"Failed to get nutrition data for user {userId} for {days} days.", ex);
            }
        }

        /// <inheritdoc/>
        public async Task<UserMealLogModel> LogMealAsync(int userId, int mealId, double portionMultiplier = 1.0, string notes = null)
        {
            try
            {
                var mealLog = new UserMealLogModel
                {
                    UserId = userId,
                    MealId = mealId,
                    ConsumedAt = DateTime.Now,
                    PortionMultiplier = portionMultiplier,
                    Notes = notes,
                    CreatedAt = DateTime.Now
                };

                var createdLog = await this.mealLogRepository.CreateAsync(mealLog);
                
                // Update daily nutrition summary
                await this.UpdateDailyNutritionAsync(userId, DateTime.Today);
                
                return createdLog;
            }
            catch (Exception ex)
            {
                throw new Exception($"Failed to log meal {mealId} for user {userId}.", ex);
            }
        }

        /// <inheritdoc/>
        public async Task<IEnumerable<UserMealLogModel>> GetMealLogsAsync(int userId, DateTime date)
        {
            try
            {
                return await this.mealLogRepository.GetByUserAndDateWithMealsAsync(userId, date.Date);
            }
            catch (Exception ex)
            {
                throw new Exception($"Failed to get meal logs for user {userId} on {date:yyyy-MM-dd}.", ex);
            }
        }

        /// <inheritdoc/>
        public async Task<UserDailyNutritionModel> GetWeeklyAverageAsync(int userId, DateTime weekStartDate)
        {
            try
            {
                var weekEndDate = weekStartDate.AddDays(6);
                var weekData = await this.dailyNutritionRepository.GetByUserAndDateRangeAsync(userId, weekStartDate, weekEndDate);
                
                if (!weekData.Any())
                {
                    return new UserDailyNutritionModel { UserId = userId, Date = weekStartDate };
                }

                return new UserDailyNutritionModel
                {
                    UserId = userId,
                    Date = weekStartDate,
                    TotalCalories = (int)weekData.Average(d => d.TotalCalories),
                    TotalProteins = weekData.Average(d => d.TotalProteins),
                    TotalCarbohydrates = weekData.Average(d => d.TotalCarbohydrates),
                    TotalFats = weekData.Average(d => d.TotalFats),
                    MealsConsumed = (int)weekData.Average(d => d.MealsConsumed),
                    WaterIntakeMl = (int)weekData.Average(d => d.WaterIntakeMl)
                };
            }
            catch (Exception ex)
            {
                throw new Exception($"Failed to calculate weekly average for user {userId} starting {weekStartDate:yyyy-MM-dd}.", ex);
            }
        }

        /// <inheritdoc/>
        public async Task<UserDailyNutritionModel> GetMonthlyAverageAsync(int userId, int month, int year)
        {
            try
            {
                var monthStart = new DateTime(year, month, 1);
                var monthEnd = monthStart.AddMonths(1).AddDays(-1);
                var monthData = await this.dailyNutritionRepository.GetByUserAndDateRangeAsync(userId, monthStart, monthEnd);
                
                if (!monthData.Any())
                {
                    return new UserDailyNutritionModel { UserId = userId, Date = monthStart };
                }

                return new UserDailyNutritionModel
                {
                    UserId = userId,
                    Date = monthStart,
                    TotalCalories = (int)monthData.Average(d => d.TotalCalories),
                    TotalProteins = monthData.Average(d => d.TotalProteins),
                    TotalCarbohydrates = monthData.Average(d => d.TotalCarbohydrates),
                    TotalFats = monthData.Average(d => d.TotalFats),
                    MealsConsumed = (int)monthData.Average(d => d.MealsConsumed),
                    WaterIntakeMl = (int)monthData.Average(d => d.WaterIntakeMl)
                };
            }
            catch (Exception ex)
            {
                throw new Exception($"Failed to calculate monthly average for user {userId} for {month}/{year}.", ex);
            }
        }

        /// <inheritdoc/>
        public async Task<Dictionary<string, int>> GetTopMealTypesAsync(int userId, int days = 30)
        {
            try
            {
                var endDate = DateTime.Today;
                var startDate = endDate.AddDays(-days + 1);
                var mealLogs = await this.mealLogRepository.GetByUserAndDateRangeAsync(userId, startDate, endDate);
                
                var mealTypeCount = new Dictionary<string, int>();
                
                foreach (var log in mealLogs)
                {
                    var meal = await this.mealRepository.GetByIdAsync(log.MealId);
                    if (meal != null && !string.IsNullOrEmpty(meal.Type))
                    {
                        if (mealTypeCount.ContainsKey(meal.Type))
                        {
                            mealTypeCount[meal.Type]++;
                        }
                        else
                        {
                            mealTypeCount[meal.Type] = 1;
                        }
                    }
                }
                
                return mealTypeCount.OrderByDescending(x => x.Value).ToDictionary(x => x.Key, x => x.Value);
            }
            catch (Exception ex)
            {
                throw new Exception($"Failed to get top meal types for user {userId} for {days} days.", ex);
            }
        }

        /// <inheritdoc/>
        public async Task<UserDailyNutritionModel> UpdateDailyNutritionAsync(int userId, DateTime date)
        {
            try
            {
                var mealLogs = await this.mealLogRepository.GetByUserAndDateAsync(userId, date.Date);
                
                double totalCalories = 0;
                double totalProteins = 0;
                double totalCarbohydrates = 0;
                double totalFats = 0;
                int mealsConsumed = mealLogs.Count();

                foreach (var log in mealLogs)
                {
                    var meal = await this.mealRepository.GetByIdAsync(log.MealId);
                    if (meal != null)
                    {
                        totalCalories += meal.Calories * log.PortionMultiplier;
                        totalProteins += meal.Proteins * log.PortionMultiplier;
                        totalCarbohydrates += meal.Carbohydrates * log.PortionMultiplier;
                        totalFats += meal.Fats * log.PortionMultiplier;
                    }
                }

                var existingNutrition = await this.dailyNutritionRepository.GetByUserAndDateAsync(userId, date.Date);
                
                if (existingNutrition != null)
                {
                    existingNutrition.TotalCalories = (int)totalCalories;
                    existingNutrition.TotalProteins = totalProteins;
                    existingNutrition.TotalCarbohydrates = totalCarbohydrates;
                    existingNutrition.TotalFats = totalFats;
                    existingNutrition.MealsConsumed = mealsConsumed;
                    existingNutrition.UpdatedAt = DateTime.Now;
                    
                    return await this.dailyNutritionRepository.UpdateAsync(existingNutrition);
                }
                else
                {
                    var newNutrition = new UserDailyNutritionModel
                    {
                        UserId = userId,
                        Date = date.Date,
                        TotalCalories = (int)totalCalories,
                        TotalProteins = totalProteins,
                        TotalCarbohydrates = totalCarbohydrates,
                        TotalFats = totalFats,
                        MealsConsumed = mealsConsumed,
                        WaterIntakeMl = 0, // Water intake is handled separately
                        CreatedAt = DateTime.Now,
                        UpdatedAt = DateTime.Now
                    };
                    
                    return await this.dailyNutritionRepository.CreateAsync(newNutrition);
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Failed to update daily nutrition for user {userId} on {date:yyyy-MM-dd}.", ex);
            }
        }
    }
} 
// <copyright file="IUserNutritionService.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace Workout.Core.IServices
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Workout.Core.Models;

    /// <summary>
    /// Interface for user nutrition service operations.
    /// </summary>
    public interface IUserNutritionService
    {
        /// <summary>
        /// Gets the daily nutrition summary for a user on a specific date.
        /// </summary>
        /// <param name="userId">The user identifier.</param>
        /// <param name="date">The date to get nutrition data for.</param>
        /// <returns>The daily nutrition summary or null if no data exists.</returns>
        Task<UserDailyNutritionModel> GetDailyNutritionAsync(int userId, DateTime date);

        /// <summary>
        /// Gets nutrition data for a user over a specified number of days.
        /// </summary>
        /// <param name="userId">The user identifier.</param>
        /// <param name="days">The number of days to retrieve data for.</param>
        /// <returns>A collection of daily nutrition summaries.</returns>
        Task<IEnumerable<UserDailyNutritionModel>> GetNutritionDataAsync(int userId, int days);

        /// <summary>
        /// Logs a meal as consumed by a user.
        /// </summary>
        /// <param name="userId">The user identifier.</param>
        /// <param name="mealId">The meal identifier.</param>
        /// <param name="portionMultiplier">The portion size multiplier.</param>
        /// <param name="notes">Optional notes about the meal consumption.</param>
        /// <returns>The created meal log entry.</returns>
        Task<UserMealLogModel> LogMealAsync(int userId, int mealId, double portionMultiplier = 1.0, string notes = null);

        /// <summary>
        /// Gets all meals logged by a user on a specific date.
        /// </summary>
        /// <param name="userId">The user identifier.</param>
        /// <param name="date">The date to get meal logs for.</param>
        /// <returns>A collection of meal log entries.</returns>
        Task<IEnumerable<UserMealLogModel>> GetMealLogsAsync(int userId, DateTime date);

        /// <summary>
        /// Calculates weekly nutrition averages for a user.
        /// </summary>
        /// <param name="userId">The user identifier.</param>
        /// <param name="weekStartDate">The start date of the week.</param>
        /// <returns>The average daily nutrition for the week.</returns>
        Task<UserDailyNutritionModel> GetWeeklyAverageAsync(int userId, DateTime weekStartDate);

        /// <summary>
        /// Calculates monthly nutrition averages for a user.
        /// </summary>
        /// <param name="userId">The user identifier.</param>
        /// <param name="month">The month to calculate averages for.</param>
        /// <param name="year">The year to calculate averages for.</param>
        /// <returns>The average daily nutrition for the month.</returns>
        Task<UserDailyNutritionModel> GetMonthlyAverageAsync(int userId, int month, int year);

        /// <summary>
        /// Gets the top meal types consumed by a user in the last specified days.
        /// </summary>
        /// <param name="userId">The user identifier.</param>
        /// <param name="days">The number of days to analyze (default is 30).</param>
        /// <returns>A dictionary of meal types and their consumption counts, ordered by frequency.</returns>
        Task<Dictionary<string, int>> GetTopMealTypesAsync(int userId, int days = 30);

        /// <summary>
        /// Gets today's nutrition summary for a user.
        /// </summary>
        /// <param name="userId">The user identifier.</param>
        /// <returns>Today's nutrition summary.</returns>
        Task<UserDailyNutritionModel> GetTodayNutritionAsync(int userId);

        /// <summary>
        /// Updates the daily nutrition summary for a user on a specific date based on meal logs.
        /// </summary>
        /// <param name="userId">The user identifier.</param>
        /// <param name="date">The date to update nutrition data for.</param>
        /// <returns>The updated daily nutrition summary.</returns>
        Task<UserDailyNutritionModel> UpdateDailyNutritionAsync(int userId, DateTime date);
    }
} 
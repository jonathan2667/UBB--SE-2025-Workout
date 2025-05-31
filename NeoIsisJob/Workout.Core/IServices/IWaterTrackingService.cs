// <copyright file="IWaterTrackingService.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace Workout.Core.IServices
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Workout.Core.Models;

    /// <summary>
    /// Interface for water tracking service operations.
    /// </summary>
    public interface IWaterTrackingService
    {
        /// <summary>
        /// Adds a water intake entry for a user.
        /// </summary>
        /// <param name="userId">The user identifier.</param>
        /// <param name="amountMl">The amount of water in milliliters.</param>
        /// <param name="notes">Optional notes about the water intake.</param>
        /// <returns>The created water intake entry.</returns>
        Task<UserWaterIntakeModel> AddWaterIntakeAsync(int userId, int amountMl, string notes = null);

        /// <summary>
        /// Gets the total water intake for a user on a specific date.
        /// </summary>
        /// <param name="userId">The user identifier.</param>
        /// <param name="date">The date to get water intake for.</param>
        /// <returns>The total water intake in milliliters.</returns>
        Task<int> GetDailyWaterIntakeAsync(int userId, DateTime date);

        /// <summary>
        /// Gets all water intake entries for a user on a specific date.
        /// </summary>
        /// <param name="userId">The user identifier.</param>
        /// <param name="date">The date to get water intake entries for.</param>
        /// <returns>A collection of water intake entries.</returns>
        Task<IEnumerable<UserWaterIntakeModel>> GetWaterIntakeEntriesAsync(int userId, DateTime date);

        /// <summary>
        /// Gets water intake data for a user over a specified number of days.
        /// </summary>
        /// <param name="userId">The user identifier.</param>
        /// <param name="days">The number of days to retrieve data for.</param>
        /// <returns>A dictionary with dates and total water intake amounts.</returns>
        Task<Dictionary<DateTime, int>> GetWaterIntakeHistoryAsync(int userId, int days);

        /// <summary>
        /// Gets the average daily water intake for a user over a specified period.
        /// </summary>
        /// <param name="userId">The user identifier.</param>
        /// <param name="days">The number of days to calculate the average for.</param>
        /// <returns>The average daily water intake in milliliters.</returns>
        Task<double> GetAverageWaterIntakeAsync(int userId, int days);

        /// <summary>
        /// Gets the user's daily water goal in milliliters (default 2000ml).
        /// </summary>
        /// <param name="userId">The user identifier.</param>
        /// <returns>The daily water goal in milliliters.</returns>
        Task<int> GetWaterGoalAsync(int userId);

        /// <summary>
        /// Sets the user's daily water goal in milliliters.
        /// </summary>
        /// <param name="userId">The user identifier.</param>
        /// <param name="goalMl">The daily water goal in milliliters.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        Task SetWaterGoalAsync(int userId, int goalMl);

        /// <summary>
        /// Calculates the water intake progress percentage for a user on a specific date.
        /// </summary>
        /// <param name="userId">The user identifier.</param>
        /// <param name="date">The date to calculate progress for.</param>
        /// <returns>The progress percentage (0-100).</returns>
        Task<double> GetWaterIntakeProgressAsync(int userId, DateTime date);

        /// <summary>
        /// Deletes a water intake entry.
        /// </summary>
        /// <param name="userId">The user identifier.</param>
        /// <param name="entryId">The water intake entry identifier.</param>
        /// <returns>True if the entry was deleted successfully, false otherwise.</returns>
        Task<bool> DeleteWaterIntakeAsync(int userId, int entryId);
    }
} 
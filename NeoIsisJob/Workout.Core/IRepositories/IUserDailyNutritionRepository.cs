// <copyright file="IUserDailyNutritionRepository.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace Workout.Core.IRepositories
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Workout.Core.Models;

    /// <summary>
    /// Repository interface for user daily nutrition operations.
    /// </summary>
    public interface IUserDailyNutritionRepository : IRepository<UserDailyNutritionModel>
    {
        /// <summary>
        /// Gets the daily nutrition record for a specific user and date.
        /// </summary>
        /// <param name="userId">The user identifier.</param>
        /// <param name="date">The date to get nutrition data for.</param>
        /// <returns>The daily nutrition record or null if not found.</returns>
        Task<UserDailyNutritionModel> GetByUserAndDateAsync(int userId, DateTime date);

        /// <summary>
        /// Gets nutrition records for a user within a date range.
        /// </summary>
        /// <param name="userId">The user identifier.</param>
        /// <param name="startDate">The start date of the range.</param>
        /// <param name="endDate">The end date of the range.</param>
        /// <returns>A collection of nutrition records within the date range.</returns>
        Task<IEnumerable<UserDailyNutritionModel>> GetByUserAndDateRangeAsync(int userId, DateTime startDate, DateTime endDate);

        /// <summary>
        /// Gets all nutrition records for a specific user.
        /// </summary>
        /// <param name="userId">The user identifier.</param>
        /// <returns>A collection of all nutrition records for the user.</returns>
        Task<IEnumerable<UserDailyNutritionModel>> GetByUserIdAsync(int userId);
    }
} 
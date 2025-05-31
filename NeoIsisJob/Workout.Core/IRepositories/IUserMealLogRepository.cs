// <copyright file="IUserMealLogRepository.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace Workout.Core.IRepositories
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Workout.Core.Models;

    /// <summary>
    /// Repository interface for user meal log operations.
    /// </summary>
    public interface IUserMealLogRepository : IRepository<UserMealLogModel>
    {
        /// <summary>
        /// Gets all meal log entries for a user on a specific date.
        /// </summary>
        /// <param name="userId">The user identifier.</param>
        /// <param name="date">The date to get meal log entries for.</param>
        /// <returns>A collection of meal log entries for the specified date.</returns>
        Task<IEnumerable<UserMealLogModel>> GetByUserAndDateAsync(int userId, DateTime date);

        /// <summary>
        /// Gets meal log entries for a user within a date range.
        /// </summary>
        /// <param name="userId">The user identifier.</param>
        /// <param name="startDate">The start date of the range.</param>
        /// <param name="endDate">The end date of the range.</param>
        /// <returns>A collection of meal log entries within the date range.</returns>
        Task<IEnumerable<UserMealLogModel>> GetByUserAndDateRangeAsync(int userId, DateTime startDate, DateTime endDate);

        /// <summary>
        /// Gets all meal log entries for a specific user.
        /// </summary>
        /// <param name="userId">The user identifier.</param>
        /// <returns>A collection of all meal log entries for the user.</returns>
        Task<IEnumerable<UserMealLogModel>> GetByUserIdAsync(int userId);

        /// <summary>
        /// Gets meal log entries for a specific meal.
        /// </summary>
        /// <param name="mealId">The meal identifier.</param>
        /// <returns>A collection of meal log entries for the specified meal.</returns>
        Task<IEnumerable<UserMealLogModel>> GetByMealIdAsync(int mealId);

        /// <summary>
        /// Gets meal log entries with full meal details for a user on a specific date.
        /// </summary>
        /// <param name="userId">The user identifier.</param>
        /// <param name="date">The date to get meal log entries for.</param>
        /// <returns>A collection of meal log entries with meal details.</returns>
        Task<IEnumerable<UserMealLogModel>> GetByUserAndDateWithMealsAsync(int userId, DateTime date);
    }
} 
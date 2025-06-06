// <copyright file="IUserWaterIntakeRepository.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace Workout.Core.IRepositories
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Workout.Core.Models;

    /// <summary>
    /// Repository interface for user water intake operations.
    /// </summary>
    public interface IUserWaterIntakeRepository : IRepository<UserWaterIntakeModel>
    {
        /// <summary>
        /// Gets all water intake entries for a user on a specific date.
        /// </summary>
        /// <param name="userId">The user identifier.</param>
        /// <param name="date">The date to get water intake entries for.</param>
        /// <returns>A collection of water intake entries for the specified date.</returns>
        Task<IEnumerable<UserWaterIntakeModel>> GetByUserAndDateAsync(int userId, DateTime date);

        /// <summary>
        /// Gets water intake entries for a user within a date range.
        /// </summary>
        /// <param name="userId">The user identifier.</param>
        /// <param name="startDate">The start date of the range.</param>
        /// <param name="endDate">The end date of the range.</param>
        /// <returns>A collection of water intake entries within the date range.</returns>
        Task<IEnumerable<UserWaterIntakeModel>> GetByUserAndDateRangeAsync(int userId, DateTime startDate, DateTime endDate);

        /// <summary>
        /// Gets all water intake entries for a specific user.
        /// </summary>
        /// <param name="userId">The user identifier.</param>
        /// <returns>A collection of all water intake entries for the user.</returns>
        Task<IEnumerable<UserWaterIntakeModel>> GetByUserIdAsync(int userId);

        /// <summary>
        /// Deletes a water intake entry by user ID and entry ID.
        /// </summary>
        /// <param name="userId">The user identifier.</param>
        /// <param name="entryId">The water intake entry identifier.</param>
        /// <returns>True if the entry was deleted successfully, false otherwise.</returns>
        Task<bool> DeleteByUserAndIdAsync(int userId, int entryId);
    }
} 
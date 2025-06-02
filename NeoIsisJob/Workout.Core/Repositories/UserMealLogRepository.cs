// <copyright file="UserMealLogRepository.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace Workout.Core.Repositories
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Microsoft.EntityFrameworkCore;
    using Workout.Core.Data;
    using Workout.Core.IRepositories;
    using Workout.Core.Models;

    /// <summary>
    /// Repository implementation for user meal log operations.
    /// </summary>
    public class UserMealLogRepository : IUserMealLogRepository
    {
        private readonly WorkoutDbContext context;

        /// <summary>
        /// Initializes a new instance of the <see cref="UserMealLogRepository"/> class.
        /// </summary>
        /// <param name="context">The database context.</param>
        public UserMealLogRepository(WorkoutDbContext context)
        {
            this.context = context;
        }

        /// <inheritdoc/>
        public async Task<UserMealLogModel> CreateAsync(UserMealLogModel entity)
        {
            try
            {
                entity.CreatedAt = DateTime.UtcNow;
                await this.context.UserMealLogs.AddAsync(entity);
                await this.context.SaveChangesAsync();
                return entity;
            }
            catch (Exception ex)
            {
                throw new Exception($"Failed to create meal log record.", ex);
            }
        }

        /// <inheritdoc/>
        public async Task<bool> DeleteAsync(int id)
        {
            try
            {
                var entity = await this.context.UserMealLogs.FindAsync(id);
                if (entity == null)
                {
                    return false;
                }

                this.context.UserMealLogs.Remove(entity);
                await this.context.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                throw new Exception($"Failed to delete meal log record {id}.", ex);
            }
        }

        /// <inheritdoc/>
        public async Task<IEnumerable<UserMealLogModel>> GetAllAsync()
        {
            try
            {
                return await this.context.UserMealLogs
                    .Include(m => m.Meal)
                    .OrderByDescending(m => m.ConsumedAt)
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                throw new Exception("Failed to get all meal log records.", ex);
            }
        }

        /// <inheritdoc/>
        public async Task<UserMealLogModel?> GetByIdAsync(int id)
        {
            try
            {
                return await this.context.UserMealLogs
                    .Include(m => m.Meal)
                    .FirstOrDefaultAsync(m => m.Id == id);
            }
            catch (Exception ex)
            {
                throw new Exception($"Failed to get meal log record {id}.", ex);
            }
        }

        /// <inheritdoc/>
        public async Task<UserMealLogModel> UpdateAsync(UserMealLogModel entity)
        {
            try
            {
                this.context.UserMealLogs.Update(entity);
                await this.context.SaveChangesAsync();
                return entity;
            }
            catch (Exception ex)
            {
                throw new Exception($"Failed to update meal log record.", ex);
            }
        }

        /// <inheritdoc/>
        public async Task<IEnumerable<UserMealLogModel>> GetByUserAndDateAsync(int userId, DateTime date)
        {
            try
            {
                return await this.context.UserMealLogs
                    .Where(m => m.UserId == userId && m.ConsumedAt.Date == date.Date)
                    .OrderBy(m => m.ConsumedAt)
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                throw new Exception($"Failed to get meal log entries for user {userId} on {date:yyyy-MM-dd}.", ex);
            }
        }

        /// <inheritdoc/>
        public async Task<IEnumerable<UserMealLogModel>> GetByUserAndDateRangeAsync(int userId, DateTime startDate, DateTime endDate)
        {
            try
            {
                return await this.context.UserMealLogs
                    .Where(m => m.UserId == userId && m.ConsumedAt.Date >= startDate.Date && m.ConsumedAt.Date <= endDate.Date)
                    .OrderBy(m => m.ConsumedAt)
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                throw new Exception($"Failed to get meal log entries for user {userId} between {startDate:yyyy-MM-dd} and {endDate:yyyy-MM-dd}.", ex);
            }
        }

        /// <inheritdoc/>
        public async Task<IEnumerable<UserMealLogModel>> GetByUserIdAsync(int userId)
        {
            try
            {
                return await this.context.UserMealLogs
                    .Where(m => m.UserId == userId)
                    .OrderByDescending(m => m.ConsumedAt)
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                throw new Exception($"Failed to get all meal log entries for user {userId}.", ex);
            }
        }

        /// <inheritdoc/>
        public async Task<IEnumerable<UserMealLogModel>> GetByMealIdAsync(int mealId)
        {
            try
            {
                return await this.context.UserMealLogs
                    .Where(m => m.MealId == mealId)
                    .OrderByDescending(m => m.ConsumedAt)
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                throw new Exception($"Failed to get meal log entries for meal {mealId}.", ex);
            }
        }

        /// <inheritdoc/>
        public async Task<IEnumerable<UserMealLogModel>> GetByUserAndDateWithMealsAsync(int userId, DateTime date)
        {
            try
            {
                return await this.context.UserMealLogs
                    .Include(m => m.Meal)
                    .Where(m => m.UserId == userId && m.ConsumedAt.Date == date.Date)
                    .OrderBy(m => m.ConsumedAt)
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                throw new Exception($"Failed to get meal log entries with meal details for user {userId} on {date:yyyy-MM-dd}.", ex);
            }
        }
    }
} 
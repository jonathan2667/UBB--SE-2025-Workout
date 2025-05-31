// <copyright file="UserDailyNutritionRepository.cs" company="PlaceholderCompany">
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
    /// Repository implementation for user daily nutrition operations.
    /// </summary>
    public class UserDailyNutritionRepository : IUserDailyNutritionRepository
    {
        private readonly WorkoutDbContext context;

        /// <summary>
        /// Initializes a new instance of the <see cref="UserDailyNutritionRepository"/> class.
        /// </summary>
        /// <param name="context">The database context.</param>
        public UserDailyNutritionRepository(WorkoutDbContext context)
        {
            this.context = context;
        }

        /// <inheritdoc/>
        public async Task<UserDailyNutritionModel> CreateAsync(UserDailyNutritionModel entity)
        {
            try
            {
                entity.CreatedAt = DateTime.UtcNow;
                entity.UpdatedAt = DateTime.UtcNow;
                await this.context.UserDailyNutrition.AddAsync(entity);
                await this.context.SaveChangesAsync();
                return entity;
            }
            catch (Exception ex)
            {
                throw new Exception($"Failed to create daily nutrition record.", ex);
            }
        }

        /// <inheritdoc/>
        public async Task<bool> DeleteAsync(int id)
        {
            try
            {
                var entity = await this.context.UserDailyNutrition.FindAsync(id);
                if (entity == null)
                {
                    return false;
                }

                this.context.UserDailyNutrition.Remove(entity);
                await this.context.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                throw new Exception($"Failed to delete daily nutrition record {id}.", ex);
            }
        }

        /// <inheritdoc/>
        public async Task<IEnumerable<UserDailyNutritionModel>> GetAllAsync()
        {
            try
            {
                return await this.context.UserDailyNutrition
                    .OrderByDescending(n => n.Date)
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                throw new Exception("Failed to get all daily nutrition records.", ex);
            }
        }

        /// <inheritdoc/>
        public async Task<UserDailyNutritionModel?> GetByIdAsync(int id)
        {
            try
            {
                return await this.context.UserDailyNutrition.FindAsync(id);
            }
            catch (Exception ex)
            {
                throw new Exception($"Failed to get daily nutrition record {id}.", ex);
            }
        }

        /// <inheritdoc/>
        public async Task<UserDailyNutritionModel> UpdateAsync(UserDailyNutritionModel entity)
        {
            try
            {
                entity.UpdatedAt = DateTime.UtcNow;
                this.context.UserDailyNutrition.Update(entity);
                await this.context.SaveChangesAsync();
                return entity;
            }
            catch (Exception ex)
            {
                throw new Exception($"Failed to update daily nutrition record.", ex);
            }
        }

        /// <inheritdoc/>
        public async Task<UserDailyNutritionModel> GetByUserAndDateAsync(int userId, DateTime date)
        {
            try
            {
                return await this.context.UserDailyNutrition
                    .FirstOrDefaultAsync(n => n.UserId == userId && n.Date.Date == date.Date);
            }
            catch (Exception ex)
            {
                throw new Exception($"Failed to get daily nutrition for user {userId} on {date:yyyy-MM-dd}.", ex);
            }
        }

        /// <inheritdoc/>
        public async Task<IEnumerable<UserDailyNutritionModel>> GetByUserAndDateRangeAsync(int userId, DateTime startDate, DateTime endDate)
        {
            try
            {
                return await this.context.UserDailyNutrition
                    .Where(n => n.UserId == userId && n.Date.Date >= startDate.Date && n.Date.Date <= endDate.Date)
                    .OrderBy(n => n.Date)
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                throw new Exception($"Failed to get nutrition data for user {userId} between {startDate:yyyy-MM-dd} and {endDate:yyyy-MM-dd}.", ex);
            }
        }

        /// <inheritdoc/>
        public async Task<IEnumerable<UserDailyNutritionModel>> GetByUserIdAsync(int userId)
        {
            try
            {
                return await this.context.UserDailyNutrition
                    .Where(n => n.UserId == userId)
                    .OrderByDescending(n => n.Date)
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                throw new Exception($"Failed to get all nutrition data for user {userId}.", ex);
            }
        }
    }
} 
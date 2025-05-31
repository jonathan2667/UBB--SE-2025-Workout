// <copyright file="UserWaterIntakeRepository.cs" company="PlaceholderCompany">
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
    /// Repository implementation for user water intake operations.
    /// </summary>
    public class UserWaterIntakeRepository : IUserWaterIntakeRepository
    {
        private readonly WorkoutDbContext context;

        /// <summary>
        /// Initializes a new instance of the <see cref="UserWaterIntakeRepository"/> class.
        /// </summary>
        /// <param name="context">The database context.</param>
        public UserWaterIntakeRepository(WorkoutDbContext context)
        {
            this.context = context;
        }

        /// <inheritdoc/>
        public async Task<UserWaterIntakeModel> CreateAsync(UserWaterIntakeModel entity)
        {
            try
            {
                entity.CreatedAt = DateTime.UtcNow;
                await this.context.UserWaterIntake.AddAsync(entity);
                await this.context.SaveChangesAsync();
                return entity;
            }
            catch (Exception ex)
            {
                throw new Exception($"Failed to create water intake record.", ex);
            }
        }

        /// <inheritdoc/>
        public async Task<bool> DeleteAsync(int id)
        {
            try
            {
                var entity = await this.context.UserWaterIntake.FindAsync(id);
                if (entity == null)
                {
                    return false;
                }

                this.context.UserWaterIntake.Remove(entity);
                await this.context.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                throw new Exception($"Failed to delete water intake record {id}.", ex);
            }
        }

        /// <inheritdoc/>
        public async Task<IEnumerable<UserWaterIntakeModel>> GetAllAsync()
        {
            try
            {
                return await this.context.UserWaterIntake
                    .OrderByDescending(w => w.ConsumedAt)
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                throw new Exception("Failed to get all water intake records.", ex);
            }
        }

        /// <inheritdoc/>
        public async Task<UserWaterIntakeModel?> GetByIdAsync(int id)
        {
            try
            {
                return await this.context.UserWaterIntake.FindAsync(id);
            }
            catch (Exception ex)
            {
                throw new Exception($"Failed to get water intake record {id}.", ex);
            }
        }

        /// <inheritdoc/>
        public async Task<UserWaterIntakeModel> UpdateAsync(UserWaterIntakeModel entity)
        {
            try
            {
                this.context.UserWaterIntake.Update(entity);
                await this.context.SaveChangesAsync();
                return entity;
            }
            catch (Exception ex)
            {
                throw new Exception($"Failed to update water intake record.", ex);
            }
        }

        /// <inheritdoc/>
        public async Task<IEnumerable<UserWaterIntakeModel>> GetByUserAndDateAsync(int userId, DateTime date)
        {
            try
            {
                return await this.context.UserWaterIntake
                    .Where(w => w.UserId == userId && w.ConsumedAt.Date == date.Date)
                    .OrderBy(w => w.ConsumedAt)
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                throw new Exception($"Failed to get water intake entries for user {userId} on {date:yyyy-MM-dd}.", ex);
            }
        }

        /// <inheritdoc/>
        public async Task<IEnumerable<UserWaterIntakeModel>> GetByUserAndDateRangeAsync(int userId, DateTime startDate, DateTime endDate)
        {
            try
            {
                return await this.context.UserWaterIntake
                    .Where(w => w.UserId == userId && w.ConsumedAt.Date >= startDate.Date && w.ConsumedAt.Date <= endDate.Date)
                    .OrderBy(w => w.ConsumedAt)
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                throw new Exception($"Failed to get water intake entries for user {userId} between {startDate:yyyy-MM-dd} and {endDate:yyyy-MM-dd}.", ex);
            }
        }

        /// <inheritdoc/>
        public async Task<IEnumerable<UserWaterIntakeModel>> GetByUserIdAsync(int userId)
        {
            try
            {
                return await this.context.UserWaterIntake
                    .Where(w => w.UserId == userId)
                    .OrderByDescending(w => w.ConsumedAt)
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                throw new Exception($"Failed to get all water intake entries for user {userId}.", ex);
            }
        }

        /// <inheritdoc/>
        public async Task<bool> DeleteByUserAndIdAsync(int userId, int entryId)
        {
            try
            {
                var entry = await this.context.UserWaterIntake
                    .FirstOrDefaultAsync(w => w.Id == entryId && w.UserId == userId);

                if (entry == null)
                {
                    return false;
                }

                this.context.UserWaterIntake.Remove(entry);
                await this.context.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                throw new Exception($"Failed to delete water intake entry {entryId} for user {userId}.", ex);
            }
        }
    }
} 
// <copyright file="MealService.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace Workout.Core.Services
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Workout.Core.IRepositories;
    using Workout.Core.IServices;
    using Workout.Core.Models;
    using Workout.Core.Utils.Filters;

    /// <summary>
    /// Service class for handling Meal-related operations.
    /// </summary>
    public class MealService : IService<MealModel>
    {
        private readonly IRepository<MealModel> mealRepository;

        /// <summary>
        /// Initializes a new instance of the <see cref="MealService"/> class.
        /// </summary>
        /// <param name="mealRepository">The meal repository.</param>
        public MealService(IRepository<MealModel> mealRepository)
        {
            this.mealRepository = mealRepository ?? throw new ArgumentNullException(nameof(mealRepository));
        }

        /// <inheritdoc/>
        public async Task<MealModel> CreateAsync(MealModel entity)
        {
            try
            {
                return await this.mealRepository.CreateAsync(entity);
            }
            catch (Exception ex)
            {
                throw new Exception("Failed to create meal.", ex);
            }
        }

        /// <inheritdoc/>
        public async Task<bool> DeleteAsync(int id)
        {
            try
            {
                return await this.mealRepository.DeleteAsync(id);
            }
            catch (Exception ex)
            {
                throw new Exception($"Failed to delete meal with ID {id}.", ex);
            }
        }

        /// <inheritdoc/>
        public async Task<IEnumerable<MealModel>> GetAllAsync()
        {
            try
            {
                return await this.mealRepository.GetAllAsync();
            }
            catch (Exception ex)
            {
                throw new Exception("Failed to retrieve meals.", ex);
            }
        }

        /// <inheritdoc/>
        public async Task<MealModel> GetByIdAsync(int id)
        {
            try
            {
                return await this.mealRepository.GetByIdAsync(id);
            }
            catch (Exception ex)
            {
                throw new Exception($"Failed to retrieve meal with ID {id}.", ex);
            }
        }

        /// <inheritdoc/>
        public async Task<MealModel> UpdateAsync(MealModel entity)
        {
            try
            {
                return await this.mealRepository.UpdateAsync(entity);
            }
            catch (Exception ex)
            {
                throw new Exception($"Failed to update meal with ID {entity.Id}.", ex);
            }
        }

        /// <inheritdoc/>
        public async Task<IEnumerable<MealModel>> GetFilteredAsync(IFilter filter)
        {
            return await this.mealRepository.GetAllFilteredAsync(filter);
        }
    }
}

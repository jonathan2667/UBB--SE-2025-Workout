// <copyright file="MealServiceProxy.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace NeoIsisJob.Proxy
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Microsoft.Extensions.Configuration;
    using Workout.Core.IServices;
    using Workout.Core.Models;
    using Workout.Core.Utils.Filters;

    /// <summary>
    /// Proxy service for handling API calls related to meals.
    /// </summary>
    public class MealServiceProxy : BaseServiceProxy, IService<MealModel>
    {
        private const string BaseRoute = "meal";

        /// <summary>
        /// Initializes a new instance of the <see cref="MealServiceProxy"/> class.
        /// </summary>
        /// <param name="configuration">The application configuration.</param>
        public MealServiceProxy(IConfiguration configuration = null)
            : base(configuration)
        {
        }

        /// <inheritdoc/>
        public async Task<MealModel> CreateAsync(MealModel entity)
        {
            try
            {
                return await this.PostAsync<MealModel>(BaseRoute, entity);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error creating meal: {ex.Message}");
                throw;
            }
        }

        /// <inheritdoc/>
        public async Task<bool> DeleteAsync(int id)
        {
            try
            {
                await this.DeleteAsync($"{BaseRoute}/{id}");
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error deleting meal: {ex.Message}");
                return false;
            }
        }

        /// <inheritdoc/>
        public async Task<IEnumerable<MealModel>> GetAllAsync()
        {
            try
            {
                var results = await this.GetAsync<IEnumerable<MealModel>>(BaseRoute);
                return results ?? new List<MealModel>();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error fetching all meals: {ex.Message}");
                return new List<MealModel>();
            }
        }

        /// <inheritdoc/>
        public async Task<MealModel> GetByIdAsync(int id)
        {
            try
            {
                return await this.GetAsync<MealModel>($"{BaseRoute}/{id}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error fetching meal by ID: {ex.Message}");
                throw;
            }
        }

        /// <inheritdoc/>
        public async Task<MealModel> UpdateAsync(MealModel entity)
        {
            try
            {
                return await this.PutAsync<MealModel>($"{BaseRoute}/{entity.Id}", entity);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error updating meal: {ex.Message}");
                throw;
            }
        }

        /// <inheritdoc/>
        public async Task<IEnumerable<MealModel>> GetFilteredAsync(IFilter filter)
        {
            try
            {
                var results = await this.PostAsync<IEnumerable<MealModel>>($"{BaseRoute}/filter", filter);
                return results ?? new List<MealModel>();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error filtering meals: {ex.Message}");
                return new List<MealModel>();
            }
        }
    }
}

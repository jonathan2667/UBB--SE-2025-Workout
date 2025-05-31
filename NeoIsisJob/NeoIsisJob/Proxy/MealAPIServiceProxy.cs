// <copyright file="MealAPIServiceProxy.cs" company="PlaceholderCompany">
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
    /// API-based proxy for meal service that connects to the server.
    /// </summary>
    public class MealAPIServiceProxy : BaseServiceProxy, IService<MealModel>
    {
        private const string EndpointName = "meal";

        /// <summary>
        /// Initializes a new instance of the <see cref="MealAPIServiceProxy"/> class.
        /// </summary>
        /// <param name="configuration">The configuration.</param>
        public MealAPIServiceProxy(IConfiguration configuration = null)
            : base(configuration)
        {
        }

        /// <inheritdoc/>
        public async Task<IEnumerable<MealModel>> GetAllAsync()
        {
            try
            {
                var results = await GetAsync<IList<MealModel>>(EndpointName);
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
                return await GetAsync<MealModel>($"{EndpointName}/{id}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error fetching meal {id}: {ex.Message}");
                return null;
            }
        }

        /// <inheritdoc/>
        public async Task<MealModel> CreateAsync(MealModel meal)
        {
            try
            {
                return await PostAsync<MealModel>(EndpointName, meal);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error creating meal: {ex.Message}");
                throw;
            }
        }

        /// <inheritdoc/>
        public async Task<MealModel> UpdateAsync(MealModel meal)
        {
            try
            {
                return await PutAsync<MealModel>($"{EndpointName}/{meal.Id}", meal);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error updating meal {meal.Id}: {ex.Message}");
                throw;
            }
        }

        /// <inheritdoc/>
        public async Task<bool> DeleteAsync(int id)
        {
            try
            {
                await DeleteAsync($"{EndpointName}/{id}");
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error deleting meal {id}: {ex.Message}");
                return false;
            }
        }

        /// <inheritdoc/>
        public async Task<IEnumerable<MealModel>> GetFilteredAsync(IFilter filter)
        {
            try
            {
                return await PostAsync<IList<MealModel>>($"{EndpointName}/filter", filter);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error filtering meals: {ex.Message}");
                return new List<MealModel>();
            }
        }

        /// <summary>
        /// Gets meals by type.
        /// </summary>
        /// <param name="type">The meal type.</param>
        /// <returns>A collection of meals of the specified type.</returns>
        public async Task<IEnumerable<MealModel>> GetByTypeAsync(string type)
        {
            var filter = new MealFilter { Type = type };
            return await GetFilteredAsync(filter);
        }
    }
} 
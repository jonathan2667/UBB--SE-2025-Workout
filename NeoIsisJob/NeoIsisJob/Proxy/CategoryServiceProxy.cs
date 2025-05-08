// <copyright file="CategoryServiceProxy.cs" company="PlaceholderCompany">
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

    /// <summary>
    /// Proxy class for calling category-related API endpoints.
    /// </summary>
    public class CategoryServiceProxy : BaseServiceProxy, IService<CategoryModel>
    {
        private const string BaseRoute = "category";

        /// <summary>
        /// Initializes a new instance of the <see cref="CategoryServiceProxy"/> class.
        /// </summary>
        /// <param name="configuration">Optional configuration instance.</param>
        public CategoryServiceProxy(IConfiguration configuration = null)
            : base(configuration)
        {
        }

        /// <inheritdoc/>
        public async Task<CategoryModel> CreateAsync(CategoryModel entity)
        {
            try
            {
                return await this.PostAsync<CategoryModel>($"{BaseRoute}", entity);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error creating category: {ex.Message}");
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
                Console.WriteLine($"Error deleting category: {ex.Message}");
                return false;
            }
        }

        /// <inheritdoc/>
        public async Task<IEnumerable<CategoryModel>> GetAllAsync()
        {
            try
            {
                return await this.GetAsync<IEnumerable<CategoryModel>>($"{BaseRoute}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error fetching all categories: {ex.Message}");
                return new List<CategoryModel>();
            }
        }

        /// <inheritdoc/>
        public async Task<CategoryModel> GetByIdAsync(int id)
        {
            try
            {
                return await this.GetAsync<CategoryModel>($"{BaseRoute}/{id}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error fetching category by ID: {ex.Message}");
                throw;
            }
        }

        /// <inheritdoc/>
        public async Task<CategoryModel> UpdateAsync(CategoryModel entity)
        {
            try
            {
                return await this.PutAsync<CategoryModel>($"{BaseRoute}/{entity.ID}", entity);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error updating category: {ex.Message}");
                throw;
            }
        }
    }
}

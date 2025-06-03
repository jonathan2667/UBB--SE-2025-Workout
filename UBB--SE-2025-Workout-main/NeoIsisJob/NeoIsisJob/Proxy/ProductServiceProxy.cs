// <copyright file="ProductServiceProxy.cs" company="PlaceholderCompany">
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
    /// Proxy service for handling API calls related to products.
    /// </summary>
    public class ProductServiceProxy : BaseServiceProxy, IService<ProductModel>
    {
        private const string BaseRoute = "product";

        /// <summary>
        /// Initializes a new instance of the <see cref="ProductServiceProxy"/> class.
        /// </summary>
        /// <param name="configuration">The application configuration.</param>
        public ProductServiceProxy(IConfiguration configuration = null)
            : base(configuration)
        {
        }

        /// <inheritdoc/>
        public async Task<ProductModel> CreateAsync(ProductModel entity)
        {
            try
            {
                return await this.PostAsync<ProductModel>(BaseRoute, entity);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error creating product: {ex.Message}");
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
                Console.WriteLine($"Error deleting product: {ex.Message}");
                return false;
            }
        }

        /// <inheritdoc/>
        public async Task<IEnumerable<ProductModel>> GetAllAsync()
        {
            try
            {
                var results = await this.GetAsync<IEnumerable<ProductModel>>(BaseRoute);
                return results ?? new List<ProductModel>();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error fetching all products: {ex.Message}");
                return new List<ProductModel>();
            }
        }

        /// <inheritdoc/>
        public async Task<ProductModel> GetByIdAsync(int id)
        {
            try
            {
                var result = await this.GetAsync<ProductModel>($"{BaseRoute}/{id}");
                return result;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error fetching product by ID: {ex.Message}");
                throw;
            }
        }

        /// <inheritdoc/>
        public async Task<ProductModel> UpdateAsync(ProductModel entity)
        {
            try
            {
                return await this.PutAsync<ProductModel>($"{BaseRoute}/{entity.ID}", entity);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error updating product: {ex.Message}");
                throw;
            }
        }

        /// <inheritdoc/>
        public async Task<IEnumerable<ProductModel>> GetFilteredAsync(IFilter filter)
        {
            try
            {
                var results = await this.PostAsync<IEnumerable<ProductModel>>($"{BaseRoute}/filter", filter);
                return results ?? new List<ProductModel>();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error fetching filtered products: {ex.Message}");
                return new List<ProductModel>();
            }
        }
    }
}

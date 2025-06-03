// <copyright file="WishlistServiceProxy.cs" company="PlaceholderCompany">
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
    /// Proxy class for calling wishlist-related API endpoints.
    /// </summary>
    public class WishlistServiceProxy : BaseServiceProxy, IService<WishlistItemModel>
    {
        private const string BaseRoute = "wishlist";

        /// <summary>
        /// Initializes a new instance of the <see cref="WishlistServiceProxy"/> class.
        /// </summary>
        /// <param name="configuration">Optional configuration for endpoint base URL.</param>
        public WishlistServiceProxy(IConfiguration configuration = null)
            : base(configuration)
        {
        }

        /// <inheritdoc/>
        public async Task<WishlistItemModel> CreateAsync(WishlistItemModel entity)
        {
            try
            {
                return await this.PostAsync<WishlistItemModel>($"{BaseRoute}", entity);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error creating wishlist item: {ex.Message}");
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
                Console.WriteLine($"Error deleting wishlist item: {ex.Message}");
                return false;
            }
        }

        /// <inheritdoc/>
        public async Task<IEnumerable<WishlistItemModel>> GetAllAsync()
        {
            try
            {
                return await this.GetAsync<IEnumerable<WishlistItemModel>>($"{BaseRoute}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error retrieving wishlist items: {ex.Message}");
                return new List<WishlistItemModel>();
            }
        }

        /// <inheritdoc/>
        public async Task<WishlistItemModel> GetByIdAsync(int id)
        {
            try
            {
                return await this.GetAsync<WishlistItemModel>($"{BaseRoute}/{id}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error retrieving wishlist item by ID: {ex.Message}");
                throw;
            }
        }

        /// <inheritdoc/>
        public async Task<WishlistItemModel> UpdateAsync(WishlistItemModel entity)
        {
            try
            {
                return await this.PutAsync<WishlistItemModel>($"{BaseRoute}/{entity.ID}", entity);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error updating wishlist item: {ex.Message}");
                throw;
            }
        }
    }
}

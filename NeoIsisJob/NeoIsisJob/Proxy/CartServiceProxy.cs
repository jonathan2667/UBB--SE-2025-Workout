// <copyright file="CartServiceProxy.cs" company="PlaceholderCompany">
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
    /// Proxy class for calling cart-related API endpoints.
    /// </summary>
    public class CartServiceProxy : BaseServiceProxy, IService<CartItemModel>
    {
        /// <summary>
        /// The base route for the cart controller.
        /// </summary>
        public const string BaseRoute = "cart";

        /// <summary>
        /// Initializes a new instance of the <see cref="CartServiceProxy"/> class.
        /// </summary>
        /// <param name="configuration">Optional configuration instance.</param>
        public CartServiceProxy(IConfiguration configuration = null)
            : base(configuration)
        {
        }

        /// <inheritdoc/>
        public async Task<CartItemModel> CreateAsync(CartItemModel entity)
        {
            try
            {
                return await this.PostAsync<CartItemModel>($"{BaseRoute}", entity);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error creating cart item: {ex.Message}");
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
                Console.WriteLine($"Error deleting cart item: {ex.Message}");
                return false;
            }
        }

        /// <inheritdoc/>
        public async Task<IEnumerable<CartItemModel>> GetAllAsync()
        {
            try
            {
                return await this.GetAsync<IEnumerable<CartItemModel>>($"{BaseRoute}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error retrieving cart items: {ex.Message}");
                return new List<CartItemModel>();
            }
        }

        /// <inheritdoc/>
        public async Task<CartItemModel> GetByIdAsync(int id)
        {
            try
            {
                return await this.GetAsync<CartItemModel>($"{BaseRoute}/{id}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error retrieving cart item by ID: {ex.Message}");
                throw;
            }
        }

        /// <inheritdoc/>
        public async Task<CartItemModel> UpdateAsync(CartItemModel entity)
        {
            try
            {
                return await this.PutAsync<CartItemModel>($"{BaseRoute}/{entity.ID}", entity);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error updating cart item: {ex.Message}");
                throw;
            }
        }

        /// <summary>
        /// Resets the cart by deleting all cart items.
        /// </summary>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        public async Task ResetCart()
        {
            try
            {
                await this.DeleteAsync($"{BaseRoute}/reset");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error resetting cart: {ex.Message}");
                throw;
            }
        }
    }
}

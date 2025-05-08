// <copyright file="OrderServiceProxy.cs" company="PlaceholderCompany">
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
    /// Proxy service for handling order-related operations.
    /// </summary>
    public class OrderServiceProxy : BaseServiceProxy, IService<OrderModel>
    {
        private const string BaseRoute = "order";

        /// <summary>
        /// Initializes a new instance of the <see cref="OrderServiceProxy"/> class.
        /// </summary>
        /// <param name="configuration">The configuration instance.</param>
        public OrderServiceProxy(IConfiguration configuration = null)
            : base(configuration)
        {
        }

        /// <inheritdoc/>
        public async Task<OrderModel> CreateAsync(OrderModel entity)
        {
            try
            {
                return await this.PostAsync<OrderModel>($"{BaseRoute}", entity);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error creating order: {ex.Message}");
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
                Console.WriteLine($"Error deleting order: {ex.Message}");
                return false;
            }
        }

        /// <inheritdoc/>
        public async Task<IEnumerable<OrderModel>> GetAllAsync()
        {
            try
            {
                return await this.GetAsync<IEnumerable<OrderModel>>($"{BaseRoute}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error fetching all orders: {ex.Message}");
                return new List<OrderModel>();
            }
        }

        /// <inheritdoc/>
        public async Task<OrderModel> GetByIdAsync(int id)
        {
            try
            {
                return await this.GetAsync<OrderModel>($"{BaseRoute}/{id}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error fetching order by ID: {ex.Message}");
                throw;
            }
        }

        /// <inheritdoc/>
        public async Task<OrderModel> UpdateAsync(OrderModel entity)
        {
            try
            {
                return await this.PutAsync<OrderModel>($"{BaseRoute}/{entity.ID}", entity);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error updating order: {ex.Message}");
                throw;
            }
        }

        /// <summary>
        /// Creates an order from the current cart items.
        /// </summary>
        /// <returns>A task that represents the asynchronous operation.</returns>
        public async Task CreateOrderFromCartAsync()
        {
            try
            {
                await this.PostAsync($"{BaseRoute}/from-cart", null);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error creating order from cart: {ex.Message}");
                throw;
            }
        }
    }
}

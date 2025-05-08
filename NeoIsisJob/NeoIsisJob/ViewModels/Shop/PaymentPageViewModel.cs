// <copyright file="PaymentPageViewModel.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace Workout.Core.ViewModel
{
    using System;
    using System.Collections.Generic;
    using System.Configuration;
    using System.Diagnostics;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using Workout.Core.Data.Database;
    using Workout.Core.Infrastructure.Session;
    using Workout.Core.Models;
    using Workout.Core.Repository;
    using Workout.Core.Service;

    /// <summary>
    /// ViewModel responsible for managing payment operations.
    /// </summary>
    public class PaymentPageViewModel
    {
        private readonly IService<OrderModel> orderService;
        private readonly IService<CartItemModel> cartService;

        /// <summary>
        /// Initializes a new instance of the <see cref="PaymentPageViewModel"/> class.
        /// </summary>
        public PaymentPageViewModel()
        {
            string? connectionString = ConfigurationManager.ConnectionStrings["DefaultConnection"]?.ConnectionString;
            if (string.IsNullOrEmpty(connectionString))
            {
                throw new InvalidOperationException("The connection string 'DefaultConnection' is not configured or is null.");
            }

            DbConnectionFactory dbConnectionFactory = new DbConnectionFactory(connectionString);
            DbService dbService = new DbService(dbConnectionFactory);
            SessionManager sessionManager = new SessionManager();
            IRepository<OrderModel> orderRepository = new OrderRepository(dbService, sessionManager);
            IRepository<CartItemModel> cartRepository = new CartItemRepository(dbService, sessionManager);
            this.orderService = new OrderService(orderRepository);
            this.cartService = new CartService(cartRepository);
        }

        /// <summary>
        /// Creates a new order asynchronously.
        /// </summary>
        /// <param name="cartItems">The cart items to include in the order.</param>
        /// <param name="totalAmount">The total amount of the order.</param>
        /// <returns>The created order.</returns>
        public async Task<OrderModel> CreateOrderAsync(IEnumerable<CartItemModel> cartItems, decimal totalAmount)
        {
            var order = new OrderModel(null, cartItems, totalAmount, DateTime.Now);
            return await this.orderService.CreateAsync(order);
        }

        /// <summary>
        /// Clears the cart after successful payment asynchronously.
        /// </summary>
        /// <returns>True if the cart was cleared successfully.</returns>
        public async Task<bool> ClearCartAsync()
        {
            IEnumerable<CartItemModel> cartItems = await this.cartService.GetAllAsync();
            foreach (var item in cartItems)
            {
                await this.cartService.DeleteAsync(item.ID);
            }

            return true;
        }

        /// <summary>
        /// Sends the order by creating it from the cart.
        /// </summary>
        /// <returns>A <see cref="Task"/> representing the result of the asynchronous operation.</returns>
        public async Task<bool> SendOrder()
        {
            try
            {
                await ((OrderService)this.orderService).CreateOrderFromCartAsync();
                return true;
            }
            catch (Exception exception)
            {
                Debug.WriteLine($"Error creating order: {exception.Message}");
                return false;
            }
        }
    }
}

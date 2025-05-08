// <copyright file="PaymentPageViewModel.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace WorkoutApp.ViewModel
{
    using System;
    using System.Collections.Generic;
    using System.Configuration;
    using System.Diagnostics;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using WorkoutApp.Data.Database;
    using WorkoutApp.Infrastructure.Session;
    using WorkoutApp.Models;
    using WorkoutApp.Repository;
    using WorkoutApp.Service;

    /// <summary>
    /// Represents the view model for the payment page.
    /// </summary>
    public class PaymentPageViewModel
    {
        private readonly IService<Order> orderService;

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

            DbConnectionFactory dbConnectionFactory = new (connectionString);
            DbService dbService = new (dbConnectionFactory);
            SessionManager sessionManager = new ();
            IRepository<CartItem> cartRepository = new CartRepository(dbService, sessionManager);
            IRepository<Order> orderRepository = new OrderRepository(dbService, sessionManager);
            this.orderService = new OrderService(orderRepository, cartRepository);
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

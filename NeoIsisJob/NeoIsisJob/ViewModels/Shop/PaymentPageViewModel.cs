// <copyright file="PaymentPageViewModel.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>
using Workout.Core.Models;

namespace NeoIsisJob.ViewModels.Shop
{
    using NeoIsisJob.Proxy;
    using System;
    using System.Collections.Generic;
    using System.Configuration;
    using System.Diagnostics;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    /// <summary>
    /// ViewModel responsible for managing payment operations.
    /// </summary>
    public class PaymentPageViewModel
    {
        private readonly OrderServiceProxy orderServiceProxy;
        private readonly CartServiceProxy cartServiceProxy;
        private readonly int userID = 1; // This should be replaced with the actual user ID from the session or authentication context.

        /// <summary>
        /// Initializes a new instance of the <see cref="PaymentPageViewModel"/> class.
        /// </summary>
        public PaymentPageViewModel()
        {
            this.orderServiceProxy = new OrderServiceProxy();
            this.cartServiceProxy = new CartServiceProxy();
        }

        /// <summary>
        /// Creates a new order asynchronously.
        /// </summary>
        /// <param name="cartItems">The cart items to include in the order.</param>
        /// <param name="totalAmount">The total amount of the order.</param>
        /// <returns>The created order.</returns>
        public async Task<OrderModel> CreateOrderAsync(IEnumerable<CartItemModel> cartItems, decimal totalAmount)
        {
            var order = new OrderModel(this.userID, DateTime.Now);
            return await this.orderServiceProxy.CreateAsync(order);
        }

        /// <summary>
        /// Clears the cart after successful payment asynchronously.
        /// </summary>
        /// <returns>True if the cart was cleared successfully.</returns>
        public async Task<bool> ClearCartAsync()
        {
            IEnumerable<CartItemModel> cartItems = await this.cartServiceProxy.GetAllAsync();
            foreach (var item in cartItems)
            {
                await this.cartServiceProxy.DeleteAsync(item.ID);
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
                await ((OrderServiceProxy)this.orderServiceProxy).CreateOrderFromCartAsync();
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

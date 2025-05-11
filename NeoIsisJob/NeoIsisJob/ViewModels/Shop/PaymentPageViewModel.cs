// <copyright file="PaymentPageViewModel.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

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
    /// Represents the view model for the payment page.
    /// </summary>
    public class PaymentPageViewModel
    {
        private readonly OrderServiceProxy orderServiceProxy;

        /// <summary>
        /// Initializes a new instance of the <see cref="PaymentPageViewModel"/> class.
        /// </summary>
        public PaymentPageViewModel()
        {
            this.orderServiceProxy = new OrderServiceProxy();
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

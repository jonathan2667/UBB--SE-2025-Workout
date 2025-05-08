// <copyright file="CartViewModel.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace Workout.Core.ViewModel
{
    using System;
    using System.Collections.Generic;
    using System.Configuration;
    using System.Threading.Tasks;
    using Workout.Core.Data.Database;
    using Workout.Core.Infrastructure.Session;
    using Workout.Core.Models;
    using Workout.Core.Repository;
    using Workout.Core.Service;

    /// <summary>
    /// ViewModel responsible for managing cart operations.
    /// </summary>
    public class CartViewModel
    {
        private readonly IService<CartItemModel> cartService;

        /// <summary>
        /// Initializes a new instance of the <see cref="CartViewModel"/> class.
        /// </summary>
        public CartViewModel()
        {
            string? connectionString = ConfigurationManager.ConnectionStrings["DefaultConnection"]?.ConnectionString;
            if (string.IsNullOrEmpty(connectionString))
            {
                throw new InvalidOperationException("The connection string 'DefaultConnection' is not configured or is null.");
            }

            DbConnectionFactory dbConnectionFactory = new DbConnectionFactory(connectionString);
            DbService dbService = new DbService(dbConnectionFactory);
            SessionManager sessionManager = new SessionManager();
            IRepository<CartItemModel> cartRepository = new CartItemRepository(dbService, sessionManager);
            this.cartService = new CartService(cartRepository);
        }

        /// <summary>
        /// Retrieves all cart items asynchronously.
        /// </summary>
        /// <returns>A collection of cart items.</returns>
        public async Task<IEnumerable<CartItemModel>> GetAllCartItemsAsync()
        {
            IEnumerable<CartItemModel> cartItems = await this.cartService.GetAllAsync();
            return cartItems;
        }

        /// <summary>
        /// Adds a product to the cart asynchronously.
        /// </summary>
        /// <param name="product">The product to add.</param>
        /// <param name="quantity">The quantity to add.</param>
        /// <returns>The created cart item.</returns>
        public async Task<CartItemModel> AddToCartAsync(ProductModel product, int quantity)
        {
            return await this.cartService.CreateAsync(new CartItemModel(null, product, quantity));
        }

        /// <summary>
        /// Updates a cart item's quantity asynchronously.
        /// </summary>
        /// <param name="cartItemId">The cart item ID.</param>
        /// <param name="quantity">The new quantity.</param>
        /// <returns>The updated cart item.</returns>
        public async Task<CartItemModel> UpdateCartItemQuantityAsync(int cartItemId, int quantity)
        {
            var cartItem = await this.cartService.GetByIdAsync(cartItemId);
            if (cartItem == null)
            {
                throw new ArgumentException($"Cart item with ID {cartItemId} not found.");
            }

            cartItem.Quantity = quantity;
            return await this.cartService.UpdateAsync(cartItem);
        }

        /// <summary>
        /// Removes a cart item asynchronously.
        /// </summary>
        /// <param name="cartItemId">The cart item ID to remove.</param>
        /// <returns>True if the item was removed successfully.</returns>
        public async Task<bool> RemoveFromCartAsync(int cartItemId)
        {
            return await this.cartService.DeleteAsync(cartItemId);
        }

        /// <summary>
        /// Calculates the total price of all items in the cart asynchronously.
        /// </summary>
        /// <returns>The total price.</returns>
        public async Task<decimal> CalculateTotalAsync()
        {
            IEnumerable<CartItemModel> cartItems = await this.cartService.GetAllAsync();
            return cartItems.Sum(item => item.Product.Price * item.Quantity);
        }
    }
}

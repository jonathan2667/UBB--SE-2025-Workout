// <copyright file="CartViewModel.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>


namespace NeoIsisJob.ViewModels.Shop
{
    using global::Workout.Core.Models;
    using NeoIsisJob.Proxy;
    using System;
    using System.Collections.Generic;
    using System.Configuration;
    using System.Linq;
    using System.Threading.Tasks;



    /// <summary>
    /// ViewModel responsible for managing cart operations.
    /// </summary>
    public class CartViewModel
    {
        private readonly CartServiceProxy cartServiceProxy;
        private readonly int userID = 1; // This should be replaced with the actual user ID from the session or authentication context.
        /// <summary>
        /// Initializes a new instance of the <see cref="CartViewModel"/> class.
        /// </summary>
        public CartViewModel()
        {
            this.cartServiceProxy = new CartServiceProxy();
        }

        /// <summary>
        /// Retrieves all cart items asynchronously.
        /// </summary>
        /// <returns>A collection of cart items.</returns>
        public async Task<IEnumerable<CartItemModel>> GetAllCartItemsAsync()
        {
            IEnumerable<CartItemModel> cartItems = await this.cartServiceProxy.GetAllAsync();
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
            return await this.cartServiceProxy.CreateAsync(new CartItemModel(this.userID, product.ID));
        }

        /// <summary>
        /// Updates a cart item's quantity asynchronously.
        /// </summary>
        /// <param name="cartItemId">The cart item ID.</param>
        /// <param name="quantity">The new quantity.</param>
        /// <returns>The updated cart item.</returns>
        public async Task<CartItemModel> UpdateCartItemQuantityAsync(int cartItemId, int quantity)
        {
            var cartItem = await this.cartServiceProxy.GetByIdAsync(cartItemId);
            if (cartItem == null)
            {
                throw new ArgumentException($"Cart item with ID {cartItemId} not found.");
            }

            return await this.cartServiceProxy.UpdateAsync(cartItem);
        }

        /// <summary>
        /// Removes a cart item asynchronously.
        /// </summary>
        /// <param name="cartItemId">The cart item ID to remove.</param>
        /// <returns>True if the item was removed successfully.</returns>
        public async Task<bool> RemoveFromCartAsync(int cartItemId)
        {
            return await this.cartServiceProxy.DeleteAsync(cartItemId);
        }

        /// <summary>
        /// Calculates the total price of all items in the cart asynchronously.
        /// </summary>
        /// <returns>The total price.</returns>
        public async Task<decimal> CalculateTotalAsync()
        {
            IEnumerable<CartItemModel> cartItems = await this.cartServiceProxy.GetAllAsync();
            return cartItems.Sum(item => item.Product.Price);
        }
    }
}

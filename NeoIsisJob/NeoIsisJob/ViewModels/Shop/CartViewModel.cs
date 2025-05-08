// <copyright file="CartViewModel.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

using Workout.Core.Models;

namespace NeoIsisJob.ViewModels.Shop
{
    using NeoIsisJob.Proxy;
    using System;
    using System.Collections.Generic;
    using System.Configuration;
    using System.Threading.Tasks;



    /// <summary>
    /// ViewModel responsible for managing cart operations such as adding, retrieving, and removing cart items.
    /// </summary>
    public class CartViewModel
    {
        /// <summary>
        /// Service used to interact with cart data.
        /// </summary>
        private readonly CartServiceProxy cartServiceProxy;
        private readonly int userId = 1; // This should be replaced with the actual user ID from the session or authentication context.

        /// <summary>
        /// Initializes a new instance of the <see cref="CartViewModel"/> class.
        /// </summary>
        public CartViewModel()
        {
            this.cartServiceProxy = new CartServiceProxy();
        }

        /// <summary>
        /// Gets the total price of all items in the cart.
        /// </summary>
        public decimal TotalPrice { get; private set; } = 0;

        /// <summary>
        /// Retrieves all cart items and updates the total price.
        /// </summary>
        /// <returns>A collection of <see cref="CartItem"/> objects.</returns>
        public async Task<IEnumerable<CartItemModel>> GetAllProductsFromCartAsync()
        {
            IEnumerable<CartItemModel> cartItems = await this.cartServiceProxy.GetAllAsync();
            this.ComputeTotalPrice(cartItems);
            return cartItems;
        }

        /// <summary>
        /// Adds a product to the cart.
        /// </summary>
        /// <param name="product">The product to add.</param>
        /// <returns>The newly added <see cref="CartItem"/>.</returns>
        public async Task<CartItemModel> AddProductToCart(ProductModel product)
        {
            return await this.cartServiceProxy.CreateAsync(new CartItemModel(this.userId, product.ID));
        }

        /// <summary>
        /// Removes a product from the cart by its ID.
        /// </summary>
        /// <param name="cartItemID">The ID of the cart item to remove.</param>
        /// <returns><c>true</c> if successfully removed; otherwise, <c>false</c>.</returns>
        public async Task<bool> RemoveProductFromCart(int cartItemID)
        {
            return await this.cartServiceProxy.DeleteAsync(cartItemID);
        }

        /// <summary>
        /// Computes the total price of the provided cart items.
        /// </summary>
        /// <param name="cartItems">The cart items to total.</param>
        private void ComputeTotalPrice(IEnumerable<CartItemModel> cartItems)
        {
            this.TotalPrice = 0;
            foreach (CartItemModel item in cartItems)
            {
                this.TotalPrice += item.Product.Price;
            }
        }
    }
}

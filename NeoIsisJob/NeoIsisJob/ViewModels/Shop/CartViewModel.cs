// <copyright file="CartViewModel.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace WorkoutApp.ViewModel
{
    using System;
    using System.Collections.Generic;
    using System.Configuration;
    using System.Threading.Tasks;
    using WorkoutApp.Data.Database;
    using WorkoutApp.Infrastructure.Session;
    using WorkoutApp.Models;
    using WorkoutApp.Repository;
    using WorkoutApp.Service;

    /// <summary>
    /// ViewModel responsible for managing cart operations such as adding, retrieving, and removing cart items.
    /// </summary>
    public class CartViewModel
    {
        /// <summary>
        /// Service used to interact with cart data.
        /// </summary>
        private readonly IService<CartItem> cartService;

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
            IRepository<CartItem> cartRepository = new CartRepository(dbService, sessionManager);
            this.cartService = new CartService(cartRepository);
        }

        /// <summary>
        /// Gets the total price of all items in the cart.
        /// </summary>
        public decimal TotalPrice { get; private set; } = 0;

        /// <summary>
        /// Retrieves all cart items and updates the total price.
        /// </summary>
        /// <returns>A collection of <see cref="CartItem"/> objects.</returns>
        public async Task<IEnumerable<CartItem>> GetAllProductsFromCartAsync()
        {
            IEnumerable<CartItem> cartItems = await this.cartService.GetAllAsync();
            this.ComputeTotalPrice(cartItems);
            return cartItems;
        }

        /// <summary>
        /// Adds a product to the cart.
        /// </summary>
        /// <param name="product">The product to add.</param>
        /// <returns>The newly added <see cref="CartItem"/>.</returns>
        public async Task<CartItem> AddProductToCart(Product product)
        {
            return await this.cartService.CreateAsync(new CartItem(null, product, 1));
        }

        /// <summary>
        /// Removes a product from the cart by its ID.
        /// </summary>
        /// <param name="cartItemID">The ID of the cart item to remove.</param>
        /// <returns><c>true</c> if successfully removed; otherwise, <c>false</c>.</returns>
        public async Task<bool> RemoveProductFromCart(int cartItemID)
        {
            return await this.cartService.DeleteAsync(cartItemID);
        }

        /// <summary>
        /// Computes the total price of the provided cart items.
        /// </summary>
        /// <param name="cartItems">The cart items to total.</param>
        private void ComputeTotalPrice(IEnumerable<CartItem> cartItems)
        {
            this.TotalPrice = 0;
            foreach (CartItem item in cartItems)
            {
                this.TotalPrice += item.Product.Price;
            }
        }
    }
}

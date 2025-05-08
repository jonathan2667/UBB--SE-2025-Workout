// <copyright file="WishlistViewModel.cs" company="PlaceholderCompany">
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
    /// ViewModel responsible for managing wishlist operations.
    /// </summary>
    public class WishlistViewModel
    {
        private readonly IService<WishlistItemModel> wishlistService;

        /// <summary>
        /// Initializes a new instance of the <see cref="WishlistViewModel"/> class.
        /// </summary>
        public WishlistViewModel()
        {
            string? connectionString = ConfigurationManager.ConnectionStrings["DefaultConnection"]?.ConnectionString;
            if (string.IsNullOrEmpty(connectionString))
            {
                throw new InvalidOperationException("The connection string 'DefaultConnection' is not configured or is null.");
            }

            DbConnectionFactory dbConnectionFactory = new DbConnectionFactory(connectionString);
            DbService dbService = new DbService(dbConnectionFactory);
            SessionManager sessionManager = new SessionManager();
            IRepository<WishlistItemModel> wishlistRepository = new WishlistItemRepository(dbService, sessionManager);
            this.wishlistService = new WishlistService(wishlistRepository);
        }

        /// <summary>
        /// Retrieves all products from the wishlist asynchronously.
        /// </summary>
        /// <returns>A collection of wishlist items.</returns>
        public async Task<IEnumerable<WishlistItemModel>> GetAllProductsFromWishlistAsync()
        {
            IEnumerable<WishlistItemModel> wishlistItems = await this.wishlistService.GetAllAsync();
            return wishlistItems;
        }

        /// <summary>
        /// Adds a product to the wishlist.
        /// </summary>
        /// <param name="product">The product to add.</param>
        /// <returns>The created wishlist item.</returns>
        public async Task<WishlistItemModel> AddProductToWishlist(ProductModel product)
        {
            return await this.wishlistService.CreateAsync(new WishlistItemModel(null, product, 1));
        }

        /// <summary>
        /// Removes a product from the wishlist.
        /// </summary>
        /// <param name="wishlistItemID">The ID of the wishlist item to remove.</param>
        /// <returns>True if the item was removed successfully.</returns>
        public async Task<bool> RemoveProductFromWishlist(int wishlistItemID)
        {
            return await this.wishlistService.DeleteAsync(wishlistItemID);
        }

        /// <summary>
        /// Retrieves a product from the wishlist by product ID.
        /// </summary>
        /// <param name="productId">The product ID to find.</param>
        /// <returns>The wishlist item, or <c>null</c> if not found.</returns>
        public async Task<WishlistItemModel?> GetProductFromWishlist(int productId)
        {
            IEnumerable<WishlistItemModel> wishlistItems = await this.wishlistService.GetAllAsync();
            foreach (WishlistItemModel item in wishlistItems)
            {
                if (item.Product.ID == productId)
                {
                    return item;
                }
            }

            return null;
        }
    }
}
